using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRManagement.WebApp.Helpers
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> GetGenderList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "აირჩიეთ სქესი" },
                new SelectListItem { Value = "1", Text = "მამრობითი" },
                new SelectListItem { Value = "2", Text = "მდედრობითი" }
            };
        }

        public static List<SelectListItem> GetEmployeeStatusList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "აირჩიეთ სტატუსი" },
                new SelectListItem { Value = "0", Text = "არააქტიური" },
                new SelectListItem { Value = "1", Text = "შტატში" },
                new SelectListItem { Value = "2", Text = "შტატგარეშე" },
                new SelectListItem { Value = "3", Text = "გათავისუფლებული" }
            };
        }
    }
}