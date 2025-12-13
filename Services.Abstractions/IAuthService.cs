using Shared.Dtos;
using Shared.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
   public interface IAuthService
    {
        public Task<GeneralResponseDto> Register(RegisterDto registerDto);
        public Task<GeneralResponseDto> VerifyOtp(VerifyOtpDto verifyOtpDto);
        public Task<GeneralResponseDto> Login(LoginDto loginDto);
        public Task<GeneralResponseDto> ForgetPassword(string Email);
        public Task<GeneralResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto);
        public Task<GeneralResponseDto> VerifyOtpForPassword(VerifyOtpDto verifyOtpDto);
    }
}
