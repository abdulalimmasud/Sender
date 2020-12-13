using AstManagement.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.AssetDB.Repositories
{
    public interface IAttachmentLogRepository
    {
        Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int simCardId);
        Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int assetId, int appId);
        AttachmentLog Insert(int simCardId, int assetId, int appId, int userId, string username, int addOrRemove = 1);
        AttachmentLog Insert(Attachment attachment, int addOrRemove = 1);
        Task<AttachmentLog> InsertAsync(Attachment attachment, int addOrRemove = 1);
    }
    public class AttachmentLogRepository: IAttachmentLogRepository
    {
        private readonly EyeAssetDbContext _context;
        public AttachmentLogRepository(EyeAssetDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int simCardId)
        {
            var data = await _context.AttachmentLogs.Join(_context.SimCards, l => l.SimCardId, s => s.Id, (l, s) => new { l, s })
                            .Where(x => x.l.SimCardId == simCardId)
                            .Select(x => new AttachmentHistoryDto
                            {
                                SimCardId = simCardId,
                                MobileNumber = x.s.MobileNumber,
                                SimNumber = x.s.SimNumber,
                                AppId = x.l.AppId,
                                AppName = x.l.App.Name,
                                AssetId = x.l.AssetId,
                                LogType = x.l.LogType,
                                LogTime = x.l.LogTime,
                                UserId = x.l.UserId,
                                Username = x.l.UserName
                            }).ToListAsync();

            return await Task.FromResult(data);
        }

        public async Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int assetId, int appId)
        {
            var data = await _context.AttachmentLogs.Join(_context.SimCards, l => l.SimCardId, s => s.Id, (l, s) => new { l, s })
                            .Where(x => x.l.AppId == appId && x.l.AssetId == assetId)
                            .Select(x => new AttachmentHistoryDto
                            {
                                SimCardId = x.l.SimCardId,
                                MobileNumber = x.s.MobileNumber,
                                SimNumber = x.s.SimNumber,
                                AppId = x.l.AppId,
                                AppName = x.l.App.Name,
                                AssetId = x.l.AssetId,
                                LogType = x.l.LogType,
                                LogTime = x.l.LogTime,
                                UserId = x.l.UserId,
                                Username = x.l.UserName
                            }).ToListAsync();

            return await Task.FromResult(data);
        }

        public AttachmentLog Insert(int simCardId, int assetId, int appId, int userId, string username, int addOrRemove = 1)
        {
            var entity = new AttachmentLog
            {
                SimCardId = simCardId,
                AssetId = assetId,
                AppId = appId,
                UserName = username,
                LogType = addOrRemove,
                UserId = userId
            };
            _context.AttachmentLogs.Add(entity);
            return entity;
        }
        public AttachmentLog Insert(Attachment attachment, int addOrRemove = 1)
        {
            var entity = new AttachmentLog
            {
                SimCardId = attachment.SimCardId,
                AssetId = attachment.AssetId,
                AppId = attachment.AppId,
                LogType = addOrRemove,
                UserName = attachment.UserName,
                UserId = attachment.UserId
            };
            _context.AttachmentLogs.Add(entity);
            return entity;
        }
        public async Task<AttachmentLog> InsertAsync(Attachment attachment, int addOrRemove = 1)
        {
            var entity = new AttachmentLog
            {
                SimCardId = attachment.SimCardId,
                AssetId = attachment.AssetId,
                AppId = attachment.AppId,
                LogType = addOrRemove,
                UserName = attachment.UserName,
                UserId = attachment.UserId
            };
            await _context.AttachmentLogs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }
    }
}
