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

namespace BusinessLogicLayer.Service.Impl
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmail(User user, string resetToken, string newPassword)
        {
            string senderAddress = _config["SMTPConfig:SenderAddress"];
            string senderDisplayName = _config["SMTPConfig:SenderDisplayName"];
            string userName = _config["SMTPConfig:UserName"];
            string password = _config["SMTPConfig:Password"];
            string host = _config["SMTPConfig:Host"];
            int port = _config.GetValue<int>("SMTPConfig:Port");

            string subject = "ToDoList Reset Password Verification";
            string body = File.ReadAllText("EmailTemplate/VerificationEmail.html");

            string appDomain = _config.GetSection("Emails:AppDomain").Value;
            string confirmationLink = _config.GetSection("Emails:ForgotPassword").Value;
            string verificationURL = string.Format(appDomain + confirmationLink, user.Id, resetToken, newPassword);

            MailMessage mail = new MailMessage
            {
                Subject = subject,
                Body = body.Replace("{{VerificationURL}}", verificationURL).Replace("{{UserName}}", user.FirstName),
                From = new MailAddress(senderAddress, senderDisplayName),
                To = { user.Email },
                IsBodyHtml = true
            };

            var smtp = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = true
            };

            smtp.Send(mail);
        }
    }
}
