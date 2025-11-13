using System.ComponentModel.DataAnnotations;

namespace HRManagement.Web.Models.API
{
    public class RegisterViewModel
    {
        [Display(Name = "პირადი ნომერი")]
        public string PersonalNumber { get; set; } = string.Empty;

        [Display(Name = "სახელი")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "გვარი")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "სქესი")]
        public int Gender { get; set; }

        [Display(Name = "დაბადების თარიღი")]
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Display(Name = "ელ.ფოსტა")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "პაროლი")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "პაროლის დადასტურება")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserInfo User { get; set; } = new();
    }

    public class UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
