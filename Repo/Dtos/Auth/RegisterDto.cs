using Shared.Helpers.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Auth
{
   public class RegisterDto
    {
        [NoSpaces]
        [Required]
        public string FirstName { get; set; }=string.Empty;
        [NoSpaces]
        [Required]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        [NoSpaces]
        [Required]
        public string Email { get; set; } = string.Empty;
        [NoSpaces]
        [Required]
        public string Password { get; set; } = string.Empty;
        [Compare("Password",ErrorMessage ="Password do not match")]
        [NoSpaces]
        [Required]
        public string ConfirmPassword {  get; set; } = string.Empty;
        [MaxLength(11), MinLength(11)]
        [NoSpaces]
        [Required]
        public string PhoneNumber {  get; set; } = string.Empty;
        [MaxLength(11), MinLength(11)]
        [NoSpaces]
        [Required]
        public string FatherNumber {  get; set; } = string.Empty;
        public string? PersonalPhoto { get; set; }
    }
}
