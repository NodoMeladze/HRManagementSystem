namespace HRManagement.Web.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public int? StatusCode { get; set; }

        public string ErrorTitle => StatusCode switch
        {
            404 => "გვერდი ვერ მოიძებნა",
            401 => "არ გაქვთ წვდომა",
            403 => "წვდომა აკრძალულია",
            500 => "სერვერის შეცდომა",
            _ => "მოხდა შეცდომა"
        };

        public string ErrorMessage => StatusCode switch
        {
            404 => "თქვენ მიერ მოთხოვნილი გვერდი ვერ მოიძებნა",
            401 => "თქვენ უნდა შეხვიდეთ სისტემაში ამ გვერდის სანახავად",
            403 => "თქვენ არ გაქვთ ამ რესურსზე წვდომის უფლება",
            500 => "სერვერზე მოხდა შეცდომა. გთხოვთ სცადოთ მოგვიანებით",
            _ => "მოხდა მოულოდნელი შეცდომა"
        };

        public string ErrorIcon => StatusCode switch
        {
            404 => "bi-search",
            401 => "bi-lock",
            403 => "bi-shield-exclamation",
            500 => "bi-exclamation-triangle",
            _ => "bi-exclamation-circle"
        };
    }
}