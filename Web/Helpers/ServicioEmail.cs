
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace Web.Helpers
{
    public class ServicioEmail
    {

        public void SendEmailGmail(String receptor, String asunto, String mensaje)
        {
            var aux = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            MimeMessage mail = new MimeMessage();

            String usermail = aux.GetSection("gmail:usuariogmail").Value;

            String passwordmail = aux.GetSection("gmail:passwordgmail").Value;

            mail.From.Add(MailboxAddress.Parse(usermail));
            mail.To.Add(MailboxAddress.Parse(receptor));
            mail.Subject = asunto;
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = mensaje
            };
            String smtpserver = aux.GetSection("gmail:hostGmail").Value;
            int port = int.Parse(aux.GetSection("gmail:portGmail").Value);
            bool ssl = bool.Parse(aux.GetSection("gmail:sslGmail").Value);
            bool defaultcreadentials = bool.Parse(aux.GetSection("gmail:defaultcredentialsGmail").Value);

            using var smtpClient = new SmtpClient();
            smtpClient.Connect(
                smtpserver,
                port,
                SecureSocketOptions.StartTls
                );
            smtpClient.Authenticate(usermail, passwordmail);
            smtpClient.Send(mail);
            smtpClient.Disconnect(true);
        }
        /*
        public void SendEmailOutlook(String receptor, String asunto, String mensaje)
        {
            MailMessage mail = new MailMessage();

            String usermail = this.configuration["usuariooutlook"];
            String passwordmail = this.configuration["passwordoutlook"];

            mail.From = new MailAddress(usermail);
            mail.To.Add(new MailAddress(receptor));
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

            String smtpserver = this.configuration["host"];
            int port = int.Parse(this.configuration["port"]);
            bool ssl = bool.Parse(this.configuration["ssl"]);
            bool defaultcreadentials = bool.Parse(this.configuration["defaultcredentials"]);

            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = smtpserver;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
            smtpClient.UseDefaultCredentials = defaultcreadentials;


            NetworkCredential usercredential = new NetworkCredential(usermail, passwordmail);

            smtpClient.Credentials = usercredential;
            smtpClient.Send(mail);
        }
        */
    }
}
