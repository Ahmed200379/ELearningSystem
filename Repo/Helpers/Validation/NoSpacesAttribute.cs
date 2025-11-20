using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers.Validation
{
   public class NoSpacesAttribute:ValidationAttribute
    {
        public NoSpacesAttribute(){
            ErrorMessage = "Spaced does not allowed.";
        }
        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            var str = value.ToString();
            return !str.Contains(" ");
        }
    }
}
