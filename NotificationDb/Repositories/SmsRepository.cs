using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationDb.Repositories
{
    public interface ISmsRepository
    {
        Task<SmsLog> Insert(string mobileNumber, string message, int statusCode, int appId, string exceptionMessage = null, string responseJson = null);
    }
    public class SmsRepository: ISmsRepository
    {
        private readonly NotificationDbContext _context;
        public SmsRepository(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<SmsLog> Insert(string mobileNumber, string message, int statusCode, int appId, string exceptionMessage = null, string responseJson = null)
        {
            var smsLog = new SmsLog
            {
                MobileNumber = mobileNumber,
                Message = message,
                StatusCode = statusCode,
                AppId = appId,
                ExceptionMessage = exceptionMessage,
                ResponseJson = responseJson
            };
            _context.SmsLog.Add(smsLog);
            await _context.SaveChangesAsync();
            return await Task.FromResult(smsLog);
        }
    }
}
