using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationDb;
using NotificationDb.Repositories;
using SenderService.Configuration;
using SenderService.Utility;
using SenderService.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SenderService.Services
{
    public interface IEmailService
    {
        Task<int> Send(EmailDto dto);
        Task Send(BulkEmailDto dto);
        Task<int> Send();
    }
    public class EmailService : IEmailService
    {
        private readonly SmarterMail _smartMail;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailRepository _repository;
        private string _token;
        public EmailService(IOptions<SmarterMail> mailOption, IWebHostEnvironment environment, IEmailRepository repository)
        {
            _smartMail = mailOption.Value;
            _environment = environment;
            _repository = repository;
        }

        public async Task<int> Send()
        {
            try
            {
                using (var tokenResponse = await HttpHelper.SendRequest(HttpMethod.Post, _smartMail.AuthApiUrl, null, new { username = _smartMail.Email, password = _smartMail.Password }))
                {
                    var tokenResult = await tokenResponse.Content.ReadAsStringAsync();
                    var tokenData = JsonConvert.DeserializeObject<SmartMailUserAuthentication>(tokenResult);
                    //client.Credentials = new NetworkCredential(_smartMail.Email, _smartMail.Password, _smartMail.AuthApiUrl);
                    //var oauth2 = new SaslMechanismOAuth2(_smartMail.Email, tokenData.AccessToken);
                    //client.Authenticate(oauth2);
                    
                    using (SmtpClient client = new SmtpClient(_smartMail.SendEmailApiUrl))
                    {
                        var from = new MailAddress("notification@mail.eyeelectronics.net");
                        var to = new MailAddress("abdulalimmasud@gmail.com");
                        var message = new MailMessage(from, to);
                        message.Subject = "Mttt";
                        message.Body = "Hello Mr! How are you?";
                        //return await Task.FromResult(200);
                        client.Timeout = 400000;
                        client.EnableSsl = true;
                        client.SendAsync(message,  $"Bearer {tokenData.AccessToken}");
                    }
                }
                return 200;  
            }          
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> Send(EmailDto dto)
        {
            int statusCode = 400;
            try
            {
                using(var tokenResponse = await HttpHelper.SendRequest(HttpMethod.Post, _smartMail.AuthApiUrl, null, new { username = _smartMail.Email, password = _smartMail.Password }))
                {
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        var tokenResult = await tokenResponse.Content.ReadAsStringAsync();
                        var tokenData = JsonConvert.DeserializeObject<SmartMailUserAuthentication>(tokenResult);
                        _token = tokenData.AccessToken;
                        var messageHTML = generateMailBody(dto.RecipientName, dto.Message);
                        var content = new EmailContentDto(dto.RecipientEmail, dto.Subject, messageHTML);
                        using (var mailResponse = await HttpHelper.SendRequest(HttpMethod.Post, _smartMail.SendEmailApiUrl, tokenData.AccessToken, content, "text/html"))
                        {
                            if (mailResponse.IsSuccessStatusCode)
                            {
                                statusCode = 200;
                                await _repository.Insert(dto.RecipientEmail, dto.RecipientName, dto.Message, statusCode, dto.AppId, dto.Subject, dto.CustomerId);
                            }
                            else
                            {
                                await _repository.Insert(dto.RecipientEmail, dto.RecipientName, dto.Message, (int)tokenResponse.StatusCode, dto.AppId, dto.Subject, dto.CustomerId, await mailResponse.Content.ReadAsStringAsync());
                            }                            
                        }
                    }
                    else
                    {
                        await _repository.Insert(dto.RecipientEmail, dto.RecipientName, dto.Message, (int)tokenResponse.StatusCode, dto.AppId, dto.Subject, dto.CustomerId, await tokenResponse.Content.ReadAsStringAsync());
                    }
                }
            }
            catch(Exception ex)
            {
                await _repository.Insert(dto.RecipientEmail, dto.RecipientName, dto.Message, statusCode, dto.AppId, dto.Subject, dto.CustomerId, ex.ToString());
            }
            return await Task.FromResult(statusCode);
        }

        public async Task Send(BulkEmailDto dto)
        {
            using (var tokenResponse = await HttpHelper.SendRequest(HttpMethod.Post, _smartMail.AuthApiUrl, null, new { username = _smartMail.Email, password = _smartMail.Password }))
            {
                var tokenResult = await tokenResponse.Content.ReadAsStringAsync();
                var tokenData = JsonConvert.DeserializeObject<SmartMailUserAuthentication>(tokenResult);
                _token = tokenData.AccessToken;
                var logs = new List<EmailLog>();
                foreach (var item in dto.Mails)
                {
                    var log = new EmailLog
                    {
                        AppId = dto.AppId,
                        CustomerId = dto.CustomerId,
                        RecipientName = item.RecipientName,
                        Email = item.RecipientEmail,
                        Message = item.Message,
                        Subject = item.Subject,
                        StatusCode = 200
                    };
                    try
                    {
                        var messageHTML = generateMailBody(item.RecipientName, item.Message);
                        var content = new EmailContentDto(item.RecipientEmail, item.Subject, messageHTML);
                        using (var mailResponse = await HttpHelper.SendRequest(HttpMethod.Post, _smartMail.SendEmailApiUrl, tokenData.AccessToken, content))
                        {
                            if (!mailResponse.IsSuccessStatusCode)
                            {
                                log.StatusCode = (int)mailResponse.StatusCode;
                                log.ExceptionMessage = await mailResponse.Content.ReadAsStringAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.StatusCode = 400;
                        log.ExceptionMessage = ex.ToString();
                    }
                    logs.Add(log);
                }
                await _repository.Insert(logs);
            }            
        }
        #region private
        private AlternateView getMessage(string headline, string name, string body)
        {
            string rootPath = _environment.ContentRootPath;
            var templatePath = Path.Combine(rootPath, "Content/Files/email_template.html");
            LinkedResource resource = new LinkedResource($"{rootPath}/Content/Images/logo.jpg");
            resource.ContentId = "eyeLogo";
            var txt = File.ReadAllText(templatePath);
            var str = txt.Replace("{headline}", headline).Replace("{name}", name).Replace("{body}", body);
            AlternateView av = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
            av.LinkedResources.Add(resource);
            return av;
        }
        private string getTemplateText()
        {
            string rootPath = _environment.ContentRootPath;
            var templatePath = Path.Combine(rootPath, "Content/Files/email_template.html");
            var txt = File.ReadAllText(templatePath);
            return txt;
        }
        private string generateMessage(string recipientName, string message)
        {
            var txt = getTemplateText();
            var str = txt.Replace("{name}", recipientName).Replace("{body}", message);
            return str;
        }
        private string generateMailBody(string recipientName, string message)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine($"<p class=\"MsoNormal\">Dear {recipientName},<u></u><u></u></p>{message}");
            body.AppendLine("<br /><br />");
            body.AppendLine("<p>Please do not reply this email.</p><br />");
            body.AppendLine("<p><strong>EYE ELECTRONICS</strong><br />PHONE: +880-9678771425<br />Corporate Address: 14A/B, 1st Colony, Mirpur, Dhaka, Bangladesh</p>");
            //signature.AppendLine("<p><img src=\"https://eyeelectronics.net/images/eye_logo.png\" alt =\"Eye Electronics Logo\" width =\"180\" /></p>");
            body.AppendLine("<br /><br />");
            body.AppendLine("<small>DISCLAIMER: Strictly confidential to recipient, unauthorized disclosure prohibited. All rights reserved. No liability assumed for any material, representations or information transmitted(including potentially malicious or harmful software). Contents and attachments, if any, subject to independent verification.</small>");

            return body.ToString();
        }

        #endregion
    }
}
