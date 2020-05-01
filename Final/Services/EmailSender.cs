using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Final.Services
{
    public class EmailSender: IEmailSender
    {
        
        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string password;
        
        public EmailSender(string host, int port, bool enableSsl, string userName, string password) {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSsl;
            this.userName = userName;
            this.password = password;
        }
        
        public Task SendEmailAsync(string email, string subject, string htmlMessage) {
            var client = new SmtpClient(host, port) {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = enableSSL
            };
            return client.SendMailAsync(
                new MailMessage(userName, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}