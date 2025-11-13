using System.ComponentModel.DataAnnotations;

namespace HRManagement.WebApp.Models.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "მომხმარებლის სახელი სავალდებულოა")]
        [Display(Name = "მომხმარებლის სახელი ან ელ.ფოსტა")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "პაროლი სავალდებულოა")]
        [DataType(DataType.Password)]
        [Display(Name = "პაროლი")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
