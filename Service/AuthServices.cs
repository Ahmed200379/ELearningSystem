using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using NETCore.MailKit.Core;
using Shared.Dtos;
using Shared.Dtos.Auth;
using Shared.Helpers;
namespace Services
{
    public class AuthServices : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly PasswordValidator<User> _passwordValidator;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;
        private readonly IJwtRepo _jwtRepo;
        public AuthServices(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            PasswordValidator<User> passwordValidator,
            IPasswordHasher<User> passwordHasher,
            IMemoryCache memoryCache,
            IEmailService emailService,
            IJwtRepo jwtRepo)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _memoryCache = memoryCache;
            _emailService = emailService;
            _jwtRepo = jwtRepo;
        }

        public async Task<GeneralResponseDto> ForgetPassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User not found with this email. "
                };
            }
            var otp = Random.Shared.Next(100000, 999999).ToString();
            _memoryCache.Set(Email, otp, TimeSpan.FromMinutes(15));
            var emailBody = $"Your OTP code is:\n\t{otp}\n. It is valid for 15 minutes.\n do'nt share this code with anybody";
            await _emailService.SendAsync(Email, "Password Reset OTP", emailBody, true);
            return new GeneralResponseDto
            {
                IsSuccess = false,
                message = "OTP has been sent to your email address."
            };
        }

        public async Task<GeneralResponseDto> Login(LoginDto loginDto )
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user,loginDto.Password))
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Invalid email or password."
                };
            }
            var token = await _jwtRepo.GenerateToken(user);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Login successful.",
                data = token
            };
        }

        public async Task<GeneralResponseDto> Register(RegisterDto registerDto)
        {
            var userExists= await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User already exists"
                };
            }
            var validationPassword = await _passwordValidator.ValidateAsync(_userManager, null, registerDto.Password);
            if (!validationPassword.Succeeded)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Password is not valid",
                    errors = validationPassword.Errors.Select(e => e.Description).ToList()
                };
            }
           
          //  _memoryCache.Set($"Register_{newUser.Email}", newUser, TimeSpan.FromMinutes(15));
            var otp= Random.Shared.Next(100000, 999999).ToString();
            var cashedUser = new CashedUser
            {
                registerDto = registerDto,
                Otp = otp,
                Password = registerDto.Password
            };
            _memoryCache.Set(registerDto.Email,cashedUser, TimeSpan.FromMinutes(15));

            var emailBody = $"Your OTP code is:\n\t{otp}\n. It is valid for 15 minutes.\n do'nt share this code with anybody";
             await _emailService.SendAsync(registerDto.Email, "Account Verification", emailBody, true);

            return new GeneralResponseDto
            {
                IsSuccess = false,
                message = " Please verify your email using the OTP sent to your email address."
            };
        }

        public async Task<GeneralResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User not found with this email."
                };
            }
            var validationPassword = await _passwordValidator.ValidateAsync(_userManager, user, resetPasswordDto.Password);
            if (!validationPassword.Succeeded)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Password is not valid",
                    errors = validationPassword.Errors.Select(e => e.Description).ToList()
                };
            }
            user.PasswordHash = _passwordHasher.HashPassword(user, resetPasswordDto.Password);
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Password reset failed.",
                    errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Password has been reset successfully. "
            };

        }

        public async Task<GeneralResponseDto> VarifyOtpForPassword(VerifyOtpDto verifyOtpDto)
        {
            if (!_memoryCache.TryGetValue(verifyOtpDto.Email, out string? otp))
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "OTP has expired or is invalid."
                };
            }
            if (otp != verifyOtpDto.Otp)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Invalid OTP."
                };
            }
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "OTP verified successfully."
            };
        }

        public async Task<GeneralResponseDto> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            if (!_memoryCache.TryGetValue(verifyOtpDto.Email, out CashedUser? cashedUser))
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "OTP has expired or is invalid."
                };
            }
            if (cashedUser!.Otp != verifyOtpDto.Otp)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Invalid OTP."
                };
            }
            var newUser = new User
            {
                UserName = cashedUser.registerDto.FirstName + " " + cashedUser.registerDto.LastName,
                Email = cashedUser.registerDto.Email,
                PhoneNumber = cashedUser.registerDto.PhoneNumber,
                Id = Guid.NewGuid().ToString(),
                PasswordHash = cashedUser.registerDto.Password,
                ParentNumber = cashedUser.registerDto.FatherNumber,
                photoUrl = cashedUser.registerDto.PersonalPhoto
            };
            var result = await _userManager.CreateAsync(newUser, cashedUser.Password);
            if(!result.Succeeded)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User creation failed.",
                    errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            var token = await _jwtRepo.GenerateToken(newUser);
            _memoryCache.Remove(verifyOtpDto.Email);


            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "User registered successfully.",
                data=token,
            };

        }

    }
}
