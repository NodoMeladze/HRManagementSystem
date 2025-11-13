using System.ComponentModel.DataAnnotations;

namespace HRManagement.Web.Models.API
{
    public class EmployeeViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "პირადი ნომერი")]
        public string PersonalNumber { get; set; } = string.Empty;

        [Display(Name = "სახელი")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "გვარი")]
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "სქესი")]
        public int Gender { get; set; }

        public string GenderDisplay { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "დაბადების თარიღი")]
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Display(Name = "ელ.ფოსტა")]
        public string? Email { get; set; }

        [Display(Name = "თანამდებობა")]
        public Guid PositionId { get; set; }

        public string PositionName { get; set; } = string.Empty;

        [Display(Name = "სტატუსი")]
        public int Status { get; set; }

        public string StatusDisplay { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "გათავისუფლების თარიღი")]
        public DateOnly? ReleaseDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
