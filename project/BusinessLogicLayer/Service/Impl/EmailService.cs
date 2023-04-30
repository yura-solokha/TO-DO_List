using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model;
using Task = System.Threading.Tasks.Task;
using System.Configuration;
using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.Service.Impl
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmail(string subject, string body, string email)
        {
            string userName = _config["SMTPConfig:UserName"];
            string password = _config["SMTPConfig:Password"];
            string host = _config["SMTPConfig:Host"];
            int port = _config.GetValue<int>("SMTPConfig:Port");
            string senderAddress = _config["SMTPConfig:SenderAddress"];
            string senderDisplayName = _config["SMTPConfig:SenderDisplayName"];

            MailMessage mail = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(senderAddress, senderDisplayName),
                To = { email },
                IsBodyHtml = true
            };

            var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = true
            };

            smtp.Send(mail);
        }
        public async Task SendResetPasswordEmail(User user, string resetToken, string newPassword)
        {
            string subject = "ToDoList Reset Password Verification";
            string bodyPath = File.ReadAllText("EmailTemplate/VerificationEmail.html");

            string localhost_port = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
              ?.Split(';')
              ?.Select(url => new Uri(url).Port.ToString())
              ?.FirstOrDefault();

            string appDomain = string.Format(_config.GetSection("Emails:AppDomain").Value, localhost_port);
            string confirmationLink = _config.GetSection("Emails:ForgotPassword").Value;
            string verificationURL = string.Format(appDomain + confirmationLink, user.Id, resetToken, newPassword);

            string body = bodyPath.Replace("{{VerificationURL}}", verificationURL).Replace("{{UserName}}", user.FirstName);

            SendEmail(subject, body, user.Email);
        }
    }
}
