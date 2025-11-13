using System.ComponentModel.DataAnnotations;

namespace HRManagement.WebApp.Models.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "პირადი ნომერი სავალდებულოა")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "პირადი ნომერი უნდა შედგებოდეს 11 ციფრისგან")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "პირადი ნომერი უნდა შეიცავდეს მხოლოდ ციფრებს")]
        [Display(Name = "პირადი ნომერი")]
        public string PersonalNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "სახელი სავალდებულოა")]
        [StringLength(100, ErrorMessage = "სახელი არ უნდა აღემატებოდეს 100 სიმბოლოს")]
        [Display(Name = "სახელი")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "გვარი სავალდებულოა")]
        [StringLength(100, ErrorMessage = "გვარი არ უნდა აღემატებოდეს 100 სიმბოლოს")]
        [Display(Name = "გვარი")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "სქესი სავალდებულოა")]
        [Display(Name = "სქესი")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "დაბადების თარიღი სავალდებულოა")]
        [DataType(DataType.Date)]
        [Display(Name = "დაბადების თარიღი")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "ელ.ფოსტა სავალდებულოა")]
        [EmailAddress(ErrorMessage = "ელ.ფოსტის ფორმატი არასწორია")]
        [StringLength(255, ErrorMessage = "ელ.ფოსტა არ უნდა აღემატებოდეს 255 სიმბოლოს")]
        [Display(Name = "ელ.ფოსტა")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "პაროლი სავალდებულოა")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "პაროლი უნდა შედგებოდეს მინიმუმ 6 სიმბოლოსგან")]
        [DataType(DataType.Password)]
        [Display(Name = "პაროლი")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "პაროლის დადასტურება სავალდებულოა")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "პაროლები არ ემთხვევა")]
        [Display(Name = "გაიმეორეთ პაროლი")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
