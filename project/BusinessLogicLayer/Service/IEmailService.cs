using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace BusinessLogicLayer.Service
{
    public interface IEmailService
    {
        Task SendEmail(string subject, string body, string email);
        Task SendResetPasswordEmail(User user, string resetToken, string newPassword);
    }
}
