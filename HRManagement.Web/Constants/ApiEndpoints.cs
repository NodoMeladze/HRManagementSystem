namespace HRManagement.Web.Constants
{
    public static class ApiEndpoints
    {
        public const string Employees = "/api/employee";
        public const string EmployeeById = "/api/employee/{0}";

        public const string Positions = "/api/position";
        public const string PositionById = "/api/position/{0}";
        public const string PositionTree = "/api/position/tree";

        public const string Register = "/api/auth/register";
        public const string Login = "/api/auth/login";
        public const string CurrentUser = "/api/auth/me";
    }
}