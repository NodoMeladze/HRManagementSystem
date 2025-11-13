using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.WebApp.Models.ViewModels.Position
{
    public class CreatePositionViewModel
    {
        [Required(ErrorMessage = "თანამდებობის დასახელება სავალდებულოა")]
        [StringLength(200, ErrorMessage = "თანამდებობის დასახელება არ უნდა აღემატებოდეს 200 სიმბოლოს")]
        [Display(Name = "თანამდებობა")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "მშობელი თანამდებობა (არასავალდებულო)")]
        public Guid? ParentId { get; set; }

        public List<SelectListItem> Positions { get; set; } = new();
    }
}