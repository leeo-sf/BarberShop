using barber_shop.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace barber_shop.Integration.Email
{
    public interface IEmail
    {
        Task SendReportByEmail(string emailTo, string subject, string body, string attachments);
        Task SendMessagePromotional(PromotionalMessage obj, User[] clients);
    }

    public class Email : IEmail
    {
        private readonly IConfiguration _configuration;
        private readonly string _username;
        private readonly string _password;
        private readonly string _provaider;

        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
            _username = _configuration.GetSection("EmailBarberShop").GetSection("Username").Value;
            _password = _configuration.GetSection("EmailBarberShop").GetSection("Password").Value;
            _provaider = _configuration.GetSection("EmailBarberShop").GetSection("Provaider").Value;
        }

        public async Task SendReportByEmail(string emailTo, string subject, string body, string attachments)
        {
            var message = PrepareMessageForSendingWithReport(emailTo, subject, body, attachments);
            SendEmailBySmtp(message);
        }

        public async Task SendMessagePromotional(PromotionalMessage obj, User[] clients)
        {
            var message = PreparePromotionalMessageForSending(obj, clients);
            SendEmailBySmtp(message);
        }

        private MailMessage PreparePromotionalMessageForSending(PromotionalMessage obj, User[] clients)
        {
            var pathTemplateMessagePromotional = @"C:\PromotionalEmailTemplate.cshtml";
            string htmlBody = System.IO.File.ReadAllText(pathTemplateMessagePromotional);
            if (obj.Image is not null)
            {
                htmlBody = htmlBody.Replace("##Image##", ConvertImageToBase64(obj.Image));
            }
            htmlBody = htmlBody.Replace("##TitleEmail##", obj.TitleEmail);
            htmlBody = htmlBody.Replace("##EmailBody##", obj.EmailBody);
            var mail = new MailMessage();
            mail.From = new MailAddress(_username);
            foreach (var client in clients)
            {
                if (ValidateEmail(client.Profile.Email))
                {
                    mail.To.Add(client.Profile.Email);
                }
            }
            mail.Subject = obj.Subject;
            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            return mail;
        }

        private MailMessage PrepareMessageForSendingWithReport(string emailTo, string subject, string body, string attachments)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_username);
            if (ValidateEmail(emailTo))
            {
                mail.To.Add(emailTo);
            }
            else
            {
                throw new Exception("Email inválido");
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.Attachments.Add(AddAttachment(attachments));
            return mail;
        }

        private bool ValidateEmail(string email)
        {
            Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
            if (expression.IsMatch(email))
            {
                return true;
            }
            return false;
        }

        private Attachment AddAttachment(string attachment)
        {
            var data = new Attachment(attachment, MediaTypeNames.Application.Octet);
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(attachment);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachment);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(attachment);

            return data;
        }

        private void SendEmailBySmtp(MailMessage message)
        {
            SmtpClient smtpClient = new SmtpClient(_provaider, 587);
            smtpClient.EnableSsl = true;
            //smtpClient.Timeout = 50000;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_username, _password);
            smtpClient.Send(message);
            smtpClient.Dispose();
        }

        private string ConvertImageToBase64(byte[] Image)
        {
            var base64 = Convert.ToBase64String(Image);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
            return imgSrc;
        }
    }
}
