using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.utilities
{
    public class EmailSender
    {
        public void SendEmail(String email, String subject, String htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("tatvatestmail@gmail.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text=htmlMessage};

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com",587,MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("tatvatestmail@gmail.com", "");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);

            }

        }
    }
}
