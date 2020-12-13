using AstManagement.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.AssetDB.Repositories
{
    public interface IAttachmentRepository
    {
        Task<int> DeleteAsync(int simCardId, int userId, string username);
        Attachment Get(int simCardId);
        Task<Attachment> GetAsync(int simCardId);
        Task<AttachmentResponseShortDto> GetAsync(int assetId, int appId);
        Task<AttachmentResponseShortDto> GetAsync(string search);
        Task<EntityList<AttachmentDto>> GetAsync(int page, int pageSize, string search = "");
        Attachment Insert(Attachment entity);
        Attachment Insert(int simCardId, int assetId, int appId, int userId, string username);
        Task<Attachment> InsertAsync(Attachment entity);
        Task<Attachment> InsertAsync(int simCardId, int assetId, int appId, int userId, string username);
        Attachment Update(Attachment updatedEntity, Attachment existingEntity = null);
        Attachment Update(int simCardId, int assetId, int appId, int userId, string username, Attachment existingEntity = null);
    }
    public class AttachmentRepository: IAttachmentRepository
    {
        private readonly EyeAssetDbContext _context;
        private readonly IAttachmentLogRepository _logRepository;
        public AttachmentRepository(EyeAssetDbContext context, IAttachmentLogRepository logRepository)
        {
            _context = context;
            _logRepository = logRepository;
        }

        public async Task<int> DeleteAsync(int simCardId, int userId, string username)
        {
            using (var tran = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var attachment = await _context.Attachments.FirstOrDefaultAsync(x => x.SimCardId == simCardId);
                    if (attachment != null)
                    {
                        var sim = _context.SimCards.FirstOrDefault(x => x.Id == simCardId);
                        sim.IsActive = 0;
                        sim.UpdateTime = DateTime.Now;
                        sim.UpdateByUser = username;
                        sim.UpdateBy = userId;
                        _context.Entry(sim).State = EntityState.Modified;
                        _context.Remove(attachment);
                        attachment.UserName = username;
                        attachment.UserId = userId;
                        await _logRepository.InsertAsync(attachment, 2);
                        var response = await _context.SaveChangesAsync();
                        await tran.CommitAsync();
                        return await Task.FromResult(1);
                    }
                    return 0;
                }
                catch(Exception ex)
                {
                    await tran.RollbackAsync();
                    throw ex;
                }
            }
        }

        public Attachment Get(int simCardId)
        {
            return _context.Attachments.FirstOrDefault(x => x.SimCardId == simCardId);
        }
        public async Task<Attachment> GetAsync(int simCardId)
        {
            return await _context.Attachments.FirstOrDefaultAsync(x => x.SimCardId == simCardId);
        }
        public async Task<AttachmentResponseShortDto> GetAsync(int assetId, int appId)
        {
            return await _context.Attachments.Where(x => x.AppId == appId && x.AssetId == assetId)
                .Select(x => new AttachmentResponseShortDto
                {
                    SimCardId = x.SimCardId,
                    AppId = appId,
                    AppName = x.App.Name,
                    AssetId = assetId,
                    MobileNumber = x.SimCard.MobileNumber,
                    SimNumber = x.SimCard.SimNumber,
                    IsActive = x.SimCard.IsActive
                }).FirstOrDefaultAsync();
        }
        public async Task<EntityList<AttachmentDto>> GetAsync(int page, int pageSize, string search = "")
        {
            var query = _context.Attachments.OrderBy(x => x.SimCard.MobileNumber)
                                .Where(x => string.IsNullOrEmpty(search) || x.SimCard.MobileNumber.Contains(search))
                                .Select(x => new AttachmentDto
                                {
                                    SimCardId = x.SimCardId,
                                    MobileNumber = x.SimCard.MobileNumber,
                                    AssetId = x.AssetId,
                                    AppId = x.AppId,
                                    AppName = x.App.Name
                                }).AsQueryable();
            int total = await query.CountAsync();
            var data = query.OrderBy(x=> x.MobileNumber)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var result = new EntityList<AttachmentDto>
            {
                Data = data,
                Total = total
            };
            return await Task.FromResult(result);
        }

        public async Task<AttachmentResponseShortDto> GetAsync(string search)
        {
            return await _context.SimCards.GroupJoin(_context.Attachments, s => s.Id, a => a.SimCardId, (s, a) => new { s, a })
                  .SelectMany(temp => temp.a.DefaultIfEmpty(), (sim, att) => new { sim = sim.s, att })
                  .Where(x => x.sim.MobileNumber == search)
                  .Select(x => new AttachmentResponseShortDto
                  {
                      SimCardId = x.sim.Id,
                      MobileNumber = x.sim.MobileNumber,
                      SimNumber = x.sim.SimNumber,
                      AssetId = x.att == null ? 0 : x.att.AssetId,
                      AppId = x.att == null ? 0 : x.att.AppId,
                      AppName = x.att == null ? null : x.att.App.Name,
                      IsActive = x.sim.IsActive
                  }).FirstOrDefaultAsync();
        }

        public Attachment Insert(Attachment entity)
        {
            using(var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Attachments.Add(entity);
                    _logRepository.Insert(entity);
                    _context.SaveChanges();
                    tran.Commit();
                    return entity;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        public Attachment Insert(int simCardId, int assetId, int appId, int userId, string username)
        {
            var entity = new Attachment
            {
                SimCardId = simCardId,
                AssetId = assetId,
                AppId = appId,
                UserName = username,
                UserId = userId
            };
            return Insert(entity);
        }
        public async Task<Attachment> InsertAsync(Attachment entity)
        {
            using (var tran = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var sim = await _context.SimCards.FirstOrDefaultAsync(x => x.Id == entity.SimCardId);
                    sim.IsActive = 1;
                    sim.UpdateBy = entity.UserId;
                    sim.UpdateByUser = entity.UserName;
                    sim.UpdateTime = DateTime.Now;
                    _context.Entry(sim).State = EntityState.Modified;
                    await _context.AddAsync(entity);
                    await _logRepository.InsertAsync(entity);
                    await _context.SaveChangesAsync();
                    await tran.CommitAsync();
                    return await Task.FromResult(entity);
                }
                catch(Exception ex)
                {
                    await tran.RollbackAsync();
                    throw ex;
                }
            }
        }
        public async Task<Attachment> InsertAsync(int simCardId, int assetId, int appId, int userId, string username)
        {
            var entity = new Attachment
            {
                SimCardId = simCardId,
                AssetId = assetId,
                AppId = appId,
                UserName = username,
                UserId = userId
            };
            return await InsertAsync(entity);
        }
        public Attachment Update(Attachment updatedEntity, Attachment existingEntity = null)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    if (existingEntity == null)
                        existingEntity = _context.Attachments.FirstOrDefault(x => x.SimCardId == updatedEntity.SimCardId);
                    existingEntity.UserName = updatedEntity.UserName;
                    existingEntity.UserId = updatedEntity.UserId;
                    updatedEntity.AttachmentTime = DateTime.Now;
                    _logRepository.Insert(existingEntity, 2);
                    _context.Entry(updatedEntity).State = EntityState.Modified;
                    _logRepository.Insert(updatedEntity);
                    tran.Commit();
                    return updatedEntity;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        public Attachment Update(int simCardId, int assetId, int appId, int userId, string username, Attachment existingEntity = null)
        {
            if (existingEntity == null)
                existingEntity = _context.Attachments.FirstOrDefault(x => x.SimCardId == simCardId);
            var entity = existingEntity;
            entity.AssetId = assetId;
            entity.AppId = appId;
            entity.UserName = username;
            entity.UserId = userId;
            return Update(entity, existingEntity);
        }
    }
}
