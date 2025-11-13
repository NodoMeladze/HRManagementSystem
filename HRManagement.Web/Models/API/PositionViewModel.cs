using System.ComponentModel.DataAnnotations;

namespace HRManagement.Web.Models.API
{
    public class PositionViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "დასახელება")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "მშობელი თანამდებობა")]
        public Guid? ParentId { get; set; }

        public string? ParentName { get; set; }

        public int Level { get; set; }

        public List<PositionViewModel> Children { get; set; } = [];

        public int EmployeeCount { get; set; }
    }
}
