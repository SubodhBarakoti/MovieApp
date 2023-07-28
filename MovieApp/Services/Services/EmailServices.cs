using SendGrid.Helpers.Mail;
using SendGrid;
using Services.Interface;
using Common.ViewModels;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Services.Services
{
    public class EmailServices : IEmailServices
    {
        public async Task<SendGrid.Response> SendEmail(EmailServiceViewModel emailservice)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var subject = emailservice.Subject;
            var from = new EmailAddress("parasbarakoti177@gmail.com", "MovieApp");
            var to = new EmailAddress(emailservice.ReceiverEmail, emailservice.UserName);
            var plainTextContent = emailservice.Message;
            var htmlContent = emailservice.HtmlContent;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response;
        }
        public async Task<string> SendSMTPEmail(EmailServiceViewModel emailservice)
        {
            string senderEmail = "parasbarakoti177@gmail.com";
            string senderPassword = "qzhtnqhhsyfttdxs";

            string receipentEmail = emailservice.ReceiverEmail;
            string subject = emailservice.Subject;
            string body = emailservice.HtmlContent;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(receipentEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return "Mail Sent Successfully";
            }
            catch (Exception ex)
            {
                return $"Error Sending Mail {ex.Message}";
            }
            finally
            {
                mailMessage.Dispose();
                smtpClient.Dispose();
            }
        }


        public async Task<string> BackGroundTaskEmail(BackGroundTaskEmail email)
        {
            string senderEmail = "parasbarakoti177@gmail.com";
            string senderPassword = "qzhtnqhhsyfttdxs";

            

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = email.Subject,
                Body = email.Message
            };
            foreach(var receipentEmail in email.ReceiverEmail)
                mailMessage.To.Add(receipentEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                return "Mail Sent Successfully";
            }
            catch (Exception ex)
            {
                return $"Error Sending Mail {ex.Message}";
            }
            finally
            {
                mailMessage.Dispose();
                smtpClient.Dispose();
            }
        }
    }
}
