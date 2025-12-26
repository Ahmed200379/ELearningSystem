using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using NETCore.MailKit.Core;
using Shared.Dtos;
using Shared.Dtos.Auth;
using Shared.Dtos.Material;
using Shared.Helpers;
using System.IdentityModel.Tokens.Jwt;
namespace Services
{
    public class AuthServices : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;
        private readonly IJwtRepo _jwtRepo;
        private readonly IImageStorage _imageStorage;
        public AuthServices(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMemoryCache memoryCache,
            IEmailService emailService,
            IJwtRepo jwtRepo,
            IImageStorage imageStorage)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _memoryCache = memoryCache;
            _emailService = emailService;
            _jwtRepo = jwtRepo;
            _imageStorage = imageStorage;
        }

        public async Task<GeneralResponseDto> ForgetPassword(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User not found with this email."
                };
            }
            var otp = Random.Shared.Next(100000, 999999).ToString();
            _memoryCache.Set(Email, otp, TimeSpan.FromMinutes(15));
            var emailBody = $"Your OTP code is:\n\t{otp}\n. It is valid for 15 minutes.\n do'nt share this code with anybody";
            await _emailService.SendAsync(Email, "Password Reset OTP", emailBody, true);
            return new GeneralResponseDto
            {
                IsSuccess = true,
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
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Login successful.",
                data = tokenString
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
            var pathFolder = $"UploadedFiles/PersonalImages{registerDto.PhoneNumber}";
            var filePath = await _imageStorage.SaveFile(registerDto.PersonalPhoto, pathFolder);
            var otp= Random.Shared.Next(100000, 999999).ToString();
            var cashedUser = new CashedUser
            {
                registerDto = registerDto,
                Otp = otp,
                Password = registerDto.Password,
                FilePath=filePath
            };
            _memoryCache.Set(registerDto.Email,cashedUser, TimeSpan.FromMinutes(15));

            var emailBody = $"Your OTP code is:\n\t{otp}\n. It is valid for 15 minutes.\n do'nt share this code with anybody";
             await _emailService.SendAsync(registerDto.Email, "Account Verification", emailBody, true);

            return new GeneralResponseDto
            {
                IsSuccess = true,
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
            _memoryCache.TryGetValue(resetPasswordDto.Email, out string? cachedvalue);
            if (cachedvalue != "Verified")
            {
                _memoryCache.Remove(resetPasswordDto.Email);
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "OTP has expired or is invalid."
                };
               
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                resetPasswordDto.Password
            );
            if (!result.Succeeded)
            {
                _memoryCache.Remove(resetPasswordDto.Email);
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Password reset failed.",
                    errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            _memoryCache.Remove(resetPasswordDto.Email);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Password has been reset successfully. "
            };

        }

        public async Task<GeneralResponseDto> VerifyOtpForPassword(VerifyOtpDto verifyOtpDto)
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
            _memoryCache.Remove(verifyOtpDto.Email);
            _memoryCache.Set(verifyOtpDto.Email, "Verified", TimeSpan.FromMinutes(15));
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
                FirstName = cashedUser.registerDto.FirstName,
                SecondName=cashedUser.registerDto.LastName,
                UserName = cashedUser.registerDto.FirstName+cashedUser.registerDto.LastName,
                Email = cashedUser.registerDto.Email,
                PhoneNumber = cashedUser.registerDto.PhoneNumber,
                Id = Guid.NewGuid().ToString(),
                ParentNumber = cashedUser.registerDto.FatherNumber,
                photoUrl = cashedUser.FilePath,
            };
            var result = await _userManager.CreateAsync(newUser, cashedUser.Password);
            var roleResult = await _userManager.AddToRoleAsync(newUser, "Student");
            if (!result.Succeeded || !roleResult.Succeeded)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "User creation failed.",
                    errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            var token = await _jwtRepo.GenerateToken(newUser);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _memoryCache.Remove(verifyOtpDto.Email);


            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "User registered successfully.",
                data=tokenString,
            };

        }

    }
}
