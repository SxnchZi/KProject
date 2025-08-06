using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace RealEstateBackend.Services
{
    public class EmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendContactRequestEmailAsync(string name, string phone, string comment)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RealEstate Site", _emailConfig.From));
            message.To.Add(new MailboxAddress("Manager", _emailConfig.To));
            message.Subject = "Новая заявка с сайта";

            message.Body = new TextPart("html")
            {
                Text = $@"<h1>Новая заявка</h1>
                        <p><strong>Имя:</strong> {name}</p>
                        <p><strong>Телефон:</strong> {phone}</p>
                        <p><strong>Комментарий:</strong> {comment}</p>
                        <p><em>{DateTime.Now}</em></p>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, _emailConfig.UseSsl);
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public class EmailConfiguration
    {
        public string From { get; set; }
        public string To { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public bool Pool { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}