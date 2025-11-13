using System.ComponentModel.DataAnnotations;

namespace HRManagement.Web.Models.API
{
    public class LoginViewModel
    {
        [Display(Name = "ელ.ფოსტა ან პირადი ნომერი")]
        public string Username { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "პაროლი")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "დამახსოვრება")]
        public bool RememberMe { get; set; }
    }
}
