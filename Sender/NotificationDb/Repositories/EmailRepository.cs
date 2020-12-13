using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDb.Repositories
{
    public interface IEmailRepository
    {
        Task<EmailLog> Insert(string email, string recipientName, string message, int statusCode, int appId, string subject = null, int? customerId = null, string exceptionMessage = null);
        Task Insert(List<EmailLog> logs);
    }
    public class EmailRepository: IEmailRepository
    {
        private readonly NotificationDbContext _context;
        public EmailRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<EmailLog> Insert(string email, string recipientName, string message, int statusCode, int appId, string subject = null, int? customerId = null, string exceptionMessage = null)
        {
            var mailLog = new EmailLog
            {
                Email = email,
                RecipientName = recipientName,
                Message = message,
                Subject = subject,
                StatusCode = statusCode,
                CustomerId = customerId,
                AppId = appId,
                ExceptionMessage = exceptionMessage
            };
            _context.EmailLog.Add(mailLog);
            await _context.SaveChangesAsync();
            return await Task.FromResult(mailLog);
        }

        public async Task Insert(List<EmailLog> logs)
        {
            _context.EmailLog.AddRange(logs);
            await _context.SaveChangesAsync();
        }
    }
}
