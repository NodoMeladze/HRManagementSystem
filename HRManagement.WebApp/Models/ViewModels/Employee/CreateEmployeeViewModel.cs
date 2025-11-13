using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.WebApp.Models.ViewModels.Employee
{
    public class CreateEmployeeViewModel
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

        [EmailAddress(ErrorMessage = "ელ.ფოსტის ფორმატი არასწორია")]
        [StringLength(255, ErrorMessage = "ელ.ფოსტა არ უნდა აღემატებოდეს 255 სიმბოლოს")]
        [Display(Name = "ელ.ფოსტა")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "თანამდებობა სავალდებულოა")]
        [Display(Name = "თანამდებობა")]
        public Guid PositionId { get; set; }

        [Required(ErrorMessage = "სტატუსი სავალდებულოა")]
        [Display(Name = "სტატუსი")]
        public int Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "გათავისუფლების თარიღი")]
        public DateTime? ReleaseDate { get; set; }

        public List<SelectListItem> Positions { get; set; } = new();
        public List<SelectListItem> Statuses { get; set; } = new();
        public List<SelectListItem> Genders { get; set; } = new();
    }
}