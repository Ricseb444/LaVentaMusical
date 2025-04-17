using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MimeKit;

namespace Utilidades
{
    public class EmailSender
    {
        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, byte[] attachment, string filename)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("nfeliz994@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            builder.Attachments.Add(filename, attachment, new ContentType("application", "pdf"));
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient(); 
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("nfeliz994@gmail.com", "qewj wqul vtow kgkt");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
