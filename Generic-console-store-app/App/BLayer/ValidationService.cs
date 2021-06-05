using System.ComponentModel;
using System.Text.RegularExpressions;
using System;

namespace Service
{
    public static class ValidationService
    {

        public static bool ValidatePersonName(string input)
        {
            string pattern = @"^[A-Z][A-Za-z]+ [A-Z][A-Za-z]+$";
            return ValidateFromRegex(input, pattern);
        }
        public static bool ValidateCityName(string input)
        {
            string pattern = @"^[A-Za-z \.']+$";
            return ValidateFromRegex(input, pattern);
        }
        public static bool ValidateString(string input)
        {
            return !String.IsNullOrWhiteSpace(input);
        }
        public static bool ValidateDouble(string input)
        {
            string pattern = @"^(-?)(0|([1-9][0-9]*))(\.[0-9]+)?$";
            return ValidateFromRegex(input, pattern);
        }
        public static bool ValidateAddress(string input)
        {
            string pattern= @"^[#.0-9a-zA-Z\s,-]+$";
            if(ValidateString(input)){
                return ValidateFromRegex(input, pattern);
            }else{
                return false;
            }
        }
        public static bool ValidateInt(string input)
        {
            string pattern = @"^[+-]?[0-9]+$";
            return ValidateFromRegex(input, pattern);
        }
        public static bool ValidatePositiveInt(string input)
        {
            string pattern = @"^[+]?[0-9]+$";
            return ValidateFromRegex(input, pattern);
        }
        public static bool ValidateNegativeInt(string input)
        {
            string pattern = @"^[-][0-9]+$";
            return ValidateFromRegex(input, pattern);
        }     
        public static bool ValidateIntWithinRange(string input, int low, int high)
        {
            string pattern= @"^[+-]?[0-9]+$";
            if(ValidateFromRegex(input, pattern))
            {
                return ValidateWithinRange(int.Parse(input), low, high);
            }else{
                return false;
            }
        }
        private static bool ValidateFromRegex(string input, string pattern)
        {
            Regex rx = new Regex(pattern);
            return rx.IsMatch(input);
        }
        private static bool ValidateWithinRange(int input, int low, int high)
        {
            return ((input <= high)&&(input >= low));
        }
        
        
    }
}