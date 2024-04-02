using library_management.Data.ViewModel.Email;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text;
using library_management.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace library_management.Services
{
    public class EmailServices : IEmailServices
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfiguration _smtpconfig;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailServices(IOptions<SMTPConfiguration> smtpconfig,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _smtpconfig = smtpconfig.Value;
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task SendEmailConfirmationAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                //await SendEmailConfirmationAsync(user, token);
                string appDomain = _configuration.GetSection("Application:AppDomain").Value ?? "";
                string confirmLink = _configuration.GetSection("Application:EmailConfirmation").Value ?? "";
                EmailMessage emailMessage = new EmailMessage
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("{{UserName}}",user.FirstName),
                        new KeyValuePair<string, string>("{{Link}}",string.Format(appDomain+confirmLink,user.Id,token))
                    }
                };
                emailMessage.Subject = UpdatePlaceHolders("Hellow {{UserName}}! Confirm Your email", emailMessage.PlaceHolders);
                emailMessage.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirmation"), emailMessage.PlaceHolders);
                await SendEmailAsync(emailMessage);
            }
        }
        /// <summary>
        /// Get email body from templates.
        /// </summary>
        /// <param name="tempemailName"></param>
        /// <returns> read the body from template </returns>
        private string GetEmailBody(string tempemailName)
        {
            var body = File.ReadAllText(string.Format(templatePath, tempemailName));
            return body;
        }
        /// <summary>
        /// after registration sending email confirmation message 
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task SendEmailAsync(EmailMessage emailMessage)
        {
            MailMessage mailMessage = new MailMessage
            {
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                From = new MailAddress(_smtpconfig.SenderEmail, _smtpconfig.SenderName),
                IsBodyHtml = _smtpconfig.IsBodyHTML
            };
            //// for multiple email sending
            foreach (var toEmail in emailMessage.ToEmails)
            {
                mailMessage.To.Add(toEmail);
            }
            //mailMessage.To.Add(emailMessage.ToEmails);
            NetworkCredential networkCredential = new NetworkCredential(_smtpconfig.SmtpUsername, _smtpconfig.SmtpPassword);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpconfig.SmtpServer,
                Port = _smtpconfig.SmtpPort,
                EnableSsl = _smtpconfig.EnableSSL,
                Credentials = networkCredential
            };
            mailMessage.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mailMessage);
        }
        /// <summary>
        /// Update the Email body
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keyValuePairs"></param>
        /// <returns> return the email body after modification </returns>
        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var PlaceHolder in keyValuePairs)
                {
                    if (text.Contains(PlaceHolder.Key))
                    {
                        text = text.Replace(PlaceHolder.Key, PlaceHolder.Value);
                    }
                }
            }
            return text;
        }
        
    }

}
