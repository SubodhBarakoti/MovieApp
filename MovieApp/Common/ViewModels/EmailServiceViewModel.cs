namespace Common.ViewModels
{
    public class EmailServiceViewModel
    {
        public string Subject { get; set; }
        public string ReceiverEmail { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string HtmlContent { get; set; }
    }
}
