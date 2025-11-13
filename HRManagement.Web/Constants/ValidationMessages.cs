namespace HRManagement.Web.Constants
{
    public static class ValidationMessages
    {
        public const string AgeOutOfRange = "ასაკი უნდა იყოს 18-დან 100 წლამდე";

        public const string ReleaseDateRequired = "გათავისუფლების თარიღი სავალდებულოა როდესაც სტატუსია 'გათავისუფლებული'";

        public const string IdMismatch = "ID-ები არ ემთხვევა";

        public const string EmployeeCreated = "თანამშრომელი წარმატებით დაემატა. გააქტიურდება 1 საათში.";
        public const string EmployeeUpdated = "თანამშრომელი წარმატებით განახლდა";
        public const string EmployeeDeleted = "თანამშრომელი წარმატებით წაიშალა";

        public const string PositionCreated = "თანამდებობა წარმატებით დაემატა";
        public const string PositionDeleted = "თანამდებობა წარმატებით წაიშალა";

        public const string EmployeeNotFound = "თანამშრომელი ვერ მოიძებნა";
        public const string EmployeeDeleteFailed = "თანამშრომლის წაშლა ვერ მოხერხდა";

        public const string PositionNotFound = "თანამდებობა ვერ მოიძებნა";
        public const string PositionDeleteFailed = "თანამდებობის წაშლა ვერ მოხერხდა";

        public const string RegistrationSuccess = "რეგისტრაცია წარმატებით დასრულდა!";
        public const string LoginSuccess = "წარმატებით შეხვედით სისტემაში!";
        public const string LogoutSuccess = "თქვენ წარმატებით გახვედით სისტემიდან";
    }
}