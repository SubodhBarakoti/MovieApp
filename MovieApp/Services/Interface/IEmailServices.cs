using Common.ViewModels;

namespace Services.Interface
{
    public interface IEmailServices
    {
        Task<string> BackGroundTaskEmail(BackGroundTaskEmail email);
        Task<SendGrid.Response> SendEmail(EmailServiceViewModel emailservice);
        Task<string> SendSMTPEmail(EmailServiceViewModel emailservice);
    }
}