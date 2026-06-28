using Enums;
using Models;
using System;

namespace Services.EducationAccounts.Utils
{
    public static class EducationAccountValidationHelper
    {
        public static string? ValidateCitizenEligibility(Citizen citizen, DateOnly todaySgt)
        {
            if (citizen.EducationAccount != null)
                return "An education account already exists for this NRIC.";

            var age = todaySgt.Year - citizen.DateOfBirth.Year;
            if (todaySgt < citizen.DateOfBirth.AddYears(age)) age--;
        
            if (age is < 16 or >= 31)
                return "Citizen must be between 16 and 30 years old.";

            return null;
        }
    }
}
