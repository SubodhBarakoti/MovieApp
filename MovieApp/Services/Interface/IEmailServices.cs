using Common.ViewModels;

namespace Services.Interface
{
    public interface IEmailServices
    {
        Task<SendGrid.Response> SendEmail(EmailServiceViewModel emailservice);
        Task<string> SendSMTPEmail(EmailServiceViewModel emailservice);
    }
}