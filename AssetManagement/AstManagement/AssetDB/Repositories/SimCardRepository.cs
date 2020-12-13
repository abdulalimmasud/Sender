using AstManagement.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.AssetDB.Repositories
{
    public interface ISimCardRepository
    {
        Task<List<SimCardResponseShortDto>> GetInactiveSimAsync(int page, int pageSize, string search);
        Task<SimCard> ActivateAsync(string mobileNumber, int operation);
        Task<SimCardResponseDto> GetAsync(int id);
        Task<SimCardResponseDto> GetAsync(string mobileNumber);
        Task<SimCard> GetAsync(int id, string mobileNumber);
        Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize);
        Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize, int type, string search);
        SimCard Insert(SimCard entity);
        SimCard Insert(string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username, int isActive = 1, int isDeleted = 0);
        Task<SimCard> InsertAsync(SimCard entity);
        Task<SimCard> InsertAsync(string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username, int isActive = 1, int isDeleted = 0);
        List<WrongInput> BulkInsert(List<SimCard> entities);
        SimCard Update(SimCard entity);
        SimCard Update(int id, string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username, int isActive = 1, int isDeleted = 0);
        Task<SimCard> UpdateAsync(SimCard entity);
        Task<SimCard> UpdateAsync(int id, string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username);
        int Delete(SimCard entity);
        int Delete(int id, int userId, string username);
        Task<int> DeleteAsync(SimCard entity);
        Task<int> DeleteAsync(int id, int userId, string username);
        Task<int> UsesCountAsync(int id);
    }
    public class SimCardRepository: ISimCardRepository
    {
        private readonly EyeAssetDbContext _context;
        public SimCardRepository(EyeAssetDbContext context)
        {
            _context = context;
        }

        public async Task<SimCard> ActivateAsync(string mobileNumber, int operation)
        {
            var simcard = await _context.SimCards.FirstOrDefaultAsync(x => x.MobileNumber == mobileNumber);
            simcard.IsActive = operation;
            _context.Entry(simcard).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await Task.FromResult(simcard);
        }
        public async Task<SimCardResponseDto> GetAsync(int id)
        {
            var entity = await _context.SimCards.GroupJoin(_context.Attachments, s=> s.Id, a=> a.SimCardId, (s, a)=> new { s, a})
                            .SelectMany(temp=> temp.a.DefaultIfEmpty(), (sim, att)=> new { sim =sim.s, att})
                            .Where(x => x.sim.Id == id && x.sim.IsActive == 1 && x.sim.IsDeleted == 0)
                            .Select(x => new SimCardResponseDto
                            {
                                Id = x.sim.Id,
                                MobileNumber = x.sim.MobileNumber,
                                SimNumber = x.sim.SimNumber,
                                Creator = x.sim.UserName,
                                Updator = x.sim.UpdateByUser,
                                OperatorId = x.sim.OperatorId,
                                OperatorPackageId = x.sim.OperatorPackageId,
                                Operator = x.sim.Operator.Name,
                                OperatorPackage = x.sim.OperatorPackage.Name,
                                AssetId = x.att == null ? 0 : x.att.AssetId,
                                CreationTime = x.sim.CreationTime,
                                UpdateTime = x.sim.UpdateTime
                            }).FirstOrDefaultAsync();
            return await Task.FromResult(entity);
        }
        public async Task<SimCardResponseDto> GetAsync(string mobileNumber)
        {
            var entity = await _context.SimCards.GroupJoin(_context.Attachments, s => s.Id, a => a.SimCardId, (s, a) => new { s, a })
                            .SelectMany(temp => temp.a.DefaultIfEmpty(), (sim, att) => new { sim = sim.s, att })
                            .Where(x => x.sim.MobileNumber == mobileNumber && x.sim.IsActive == 1 && x.sim.IsDeleted == 0)
                            .Select(x => new SimCardResponseDto
                            {
                                Id = x.sim.Id,
                                MobileNumber = x.sim.MobileNumber,
                                SimNumber = x.sim.SimNumber,
                                Creator = x.sim.UserName,
                                Updator = x.sim.UpdateByUser,
                                OperatorId = x.sim.OperatorId,
                                OperatorPackageId = x.sim.OperatorPackageId,
                                Operator = x.sim.Operator.Name,
                                OperatorPackage = x.sim.OperatorPackage.Name,
                                AssetId = x.att == null ? 0 : x.att.AssetId,
                                CreationTime = x.sim.CreationTime,
                                UpdateTime = x.sim.UpdateTime
                            }).FirstOrDefaultAsync();
            return await Task.FromResult(entity);
        }
        public async Task<SimCard> GetAsync(int id, string mobileNumber)
        {
            var entity = await _context.SimCards.FirstOrDefaultAsync(x => x.Id != id && x.MobileNumber == mobileNumber);
            return await Task.FromResult(entity);
        }
        public async Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize)
        {
            var query = from sim in _context.SimCards
                        join opt in _context.MobileOperators on sim.OperatorId equals opt.Id
                        join pkg in _context.MobileOperatorPackages on sim.OperatorPackageId equals pkg.Id
                        join attachment in _context.Attachments on sim.Id equals attachment.SimCardId into attachments
                        from atcmnt in attachments.DefaultIfEmpty()
                        where atcmnt == null && sim.IsDeleted == 0
                        select new SimCardResponseDto
                            {
                                Id = sim.Id,
                                MobileNumber = sim.MobileNumber,
                                SimNumber = sim.SimNumber,
                                Creator = sim.UserName,
                                Updator = sim.UpdateByUser,
                                OperatorId = sim.OperatorId,
                                OperatorPackageId = sim.OperatorPackageId,
                                Operator = opt.Name,
                                OperatorPackage = pkg.Name
                            };
            int total = await query.CountAsync();
            var data = await query.OrderBy(x => x.MobileNumber).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = new EntityList<SimCardResponseDto>
            {
                Data = data,
                Total = total
            };
            return await Task.FromResult(result);
        }
        public async Task<EntityList<SimCardResponseDto>> GetAsync(int page, int pageSize, int type, string search)
        {
            var query = _context.SimCards.GroupJoin(_context.Attachments, s => s.Id, a => a.SimCardId, (s, a) => new { s, a })
                            .SelectMany(temp => temp.a.DefaultIfEmpty(), (sim, att) => new { sim = sim.s, att })
                            .Where(x => (string.IsNullOrEmpty(search) || ((type == 1 && x.sim.MobileNumber.Contains(search)) || type == 2 && x.sim.SimNumber.Contains(search))) && x.sim.IsDeleted == 0)
                            .Select(x => new SimCardResponseDto
                            {
                                Id = x.sim.Id,
                                MobileNumber = x.sim.MobileNumber,
                                SimNumber = x.sim.SimNumber,
                                Creator = x.sim.UserName,
                                Updator = x.sim.UpdateByUser,
                                OperatorId = x.sim.OperatorId,
                                OperatorPackageId = x.sim.OperatorPackageId,
                                Operator = x.sim.Operator.Name,
                                OperatorPackage = x.sim.OperatorPackage.Name,
                                AssetId = x.att == null ? 0 : x.att.AssetId,
                                CreationTime = x.sim.CreationTime,
                                UpdateTime = x.sim.UpdateTime
                            });
            int total = await query.CountAsync();
            var data = await query.OrderBy(x => x.MobileNumber).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = new EntityList<SimCardResponseDto>
            {
                Data = data,
                Total = total
            };
            return await Task.FromResult(result);
        }
        public SimCard Insert(SimCard entity)
        {
            try
            {
                _context.SimCards.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch(Exception ex)
            {
                _context.Entry(entity).State = EntityState.Detached;
                _context.SaveChanges();
                throw ex;
            }
        }
        public SimCard Insert(string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username, int isActive = 1, int isDeleted = 0)
        {
            var entity = new SimCard
            {
                MobileNumber = mobileNo,
                SimNumber = simNumber,
                OperatorId = operatorId,
                OperatorPackageId = packageId,
                IsActive = isActive,
                IsDeleted = isDeleted,
                UserName = username,
                UserId = userId,
                UpdateTime = DateTime.Now,
                UpdateByUser = username,
                UpdateBy = userId
            };
            return Insert(entity);
        }
        public async Task<SimCard> InsertAsync(SimCard entity)
        {
            await _context.SimCards.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }
        public async Task<SimCard> InsertAsync(string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username, int isActive = 1, int isDeleted = 0)
        {
            var entity = new SimCard
            {
                MobileNumber = mobileNo,
                SimNumber = simNumber,
                OperatorId = operatorId,
                OperatorPackageId = packageId,
                IsActive = isActive,
                IsDeleted = isDeleted,
                UserName = username,
                UserId = userId,
                UpdateTime = DateTime.Now,
                UpdateByUser = username,
                UpdateBy = userId
            };
            return await InsertAsync(entity);
        }
        public SimCard Update(SimCard entity)
        {
            _context.SimCards.Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public SimCard Update(int id, string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username, int isActive = 1, int isDeleted = 0)
        {
            var entity = _context.SimCards.FirstOrDefault(x => x.Id == id);
            entity.MobileNumber = mobileNo;
            entity.SimNumber = simNumber;
            entity.OperatorId = operatorId;
            entity.OperatorPackageId = packageId;
            entity.IsActive = isActive;
            entity.IsDeleted = isDeleted;
            entity.UpdateByUser = username;
            entity.UpdateBy = userId;
            entity.UpdateTime = DateTime.Now;
            return Update(entity);
        }
        public async Task<SimCard> UpdateAsync(SimCard entity)
        {
            _context.SimCards.Update(entity);
            _context.SaveChanges();
            return await Task.FromResult(entity);
        }
        public async Task<SimCard> UpdateAsync(int id, string mobileNo, string simNumber, int operatorId, int packageId, int userId, string username)
        {
            var entity = await _context.SimCards.FirstOrDefaultAsync(x => x.Id == id);
            entity.MobileNumber = mobileNo;
            entity.SimNumber = simNumber;
            entity.OperatorId = operatorId;
            entity.OperatorPackageId = packageId;
            entity.UpdateByUser = username;
            entity.UpdateBy = userId;
            entity.UpdateTime = DateTime.Now;
            return await UpdateAsync(entity);
        }
        public int Delete(SimCard entity)
        {
            entity.IsDeleted = 1;
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChanges();
        }
        public int Delete(int id, int userId, string username)
        {
            var entity = _context.SimCards.FirstOrDefault(x => x.Id == id);
            entity.UpdateTime = DateTime.Now;
            entity.UpdateBy = userId;
            entity.UpdateByUser = username;
            return Delete(entity);
        }
        public async Task<int> DeleteAsync(SimCard entity)
        {
            entity.IsDeleted = 1;
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(int id, int userId, string username)
        {
            var entity = await _context.SimCards.FirstOrDefaultAsync(x => x.Id == id);
            entity.UpdateTime = DateTime.Now;
            entity.UpdateBy = userId;
            entity.UpdateByUser = username;
            return await DeleteAsync(entity);
        }

        public List<WrongInput> BulkInsert(List<SimCard> entities)
        {
            var nonInserted = new List<WrongInput>();
            var mobileNumbers = entities.Select(x => x.MobileNumber).ToArray();
            var simNumbers = entities.Select(x => x.SimNumber).ToArray();
            var existingMobileNumbers = _context.SimCards.Where(x => mobileNumbers.Contains(x.MobileNumber)).Select(x => x.MobileNumber).ToArray();
            var existingSimNumbers = _context.SimCards.Where(x => simNumbers.Contains(x.SimNumber)).Select(x => x.SimNumber).ToArray();
            for (int i = 0; i < entities.Count; i++)
            {
                if (!existingMobileNumbers.Any(x=> x == entities[i].MobileNumber))
                {
                    if (!existingSimNumbers.Any(x => x == entities[i].SimNumber))
                    {
                        try
                        {
                            Insert(entities[i]);
                        }
                        catch (Exception ex)
                        {
                            nonInserted.Add(new WrongInput { Key = entities[i].MobileNumber , Cause = ex.Message});
                        }
                    }
                    else
                    {
                        nonInserted.Add(new WrongInput { Key = entities[i].MobileNumber, Cause = "Sim Number Already Exists." });
                    }
                }
                else
                {
                    nonInserted.Add(new WrongInput { Key = entities[i].MobileNumber, Cause = "Mobile Number Already Exists." });
                }
            }
            return nonInserted;
        }

        public async Task<int> UsesCountAsync(int id)
        {
            return await _context.Attachments.CountAsync(x => x.SimCardId == id);
        }

        public async Task<List<SimCardResponseShortDto>> GetInactiveSimAsync(int page, int pageSize, string search)
        {
            return await _context.SimCards
                .Where(x => x.IsDeleted == 0 && x.IsActive == 0 && (string.IsNullOrEmpty(search) || x.MobileNumber.Contains(search)))
                .Select(x => new SimCardResponseShortDto
                {
                    Id = x.Id,
                    MobileNumber = x.MobileNumber,
                    SimNumber = x.SimNumber
                }).OrderBy(x=> x.MobileNumber).Skip((page - 1)* pageSize).Take(pageSize).ToListAsync();
        }
    }
}
