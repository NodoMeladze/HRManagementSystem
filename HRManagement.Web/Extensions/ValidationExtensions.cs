namespace HRManagement.Web.Extensions
{
    public static class ValidationExtensions
    {
        public static int CalculateAge(this DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        public static bool IsValidAge(this DateOnly birthDate, int minAge = 18, int maxAge = 100)
        {
            var age = birthDate.CalculateAge();
            return age >= minAge && age <= maxAge;
        }
    }
}