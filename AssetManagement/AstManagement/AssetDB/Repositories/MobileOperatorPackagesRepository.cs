using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using AstManagement.ViewModel;

namespace AstManagement.AssetDB.Repositories
{
    public interface IMobileOperatorPackagesRepository
    {
        Task<IEnumerable<OperatorPackageResponseDto>> GetAsync();
        IEnumerable<MobileOperatorPackage> GetByOperator(int operatorId);
        Task<IEnumerable<MobileOperatorPackage>> GetByOperatorAsync(int operatorId);
        MobileOperatorPackage GetByOperator(int operatorId, string packagename);
        Task<MobileOperatorPackage> GetByOperatorAsync(int operatorId, string packagename);
        Task<MobileOperatorPackage> GetByOperatorAsync(int id, int operatorId, string packagename);
        MobileOperatorPackage Insert(MobileOperatorPackage entity);
        MobileOperatorPackage Insert(string name, int operatorId, string description = null);
        Task<MobileOperatorPackage> InsertAsync(MobileOperatorPackage entity);
        Task<MobileOperatorPackage> InsertAsync(string name, int operatorId, string description = null);
        Task<MobileOperatorPackage> UpdateAsync(int id, string name, string description, int operatorId);
        Task<MobileOperatorPackage> UpdateAsync(MobileOperatorPackage entity);
        Task<int> DeleteAsync(int id);
        Task<int> UsesCountAsync(int id);
    }
    public class MobileOperatorPackagesRepository: IMobileOperatorPackagesRepository
    {
        private readonly EyeAssetDbContext _context;
        public MobileOperatorPackagesRepository(EyeAssetDbContext context)
        {
            _context = context;
        }
        public IEnumerable<MobileOperatorPackage> GetByOperator(int operatorId)
        {
            return _context.MobileOperatorPackages.Where(x => x.OperatorId == operatorId && x.IsActive == 1 && x.IsDeleted == 0);
        }
        public async Task<IEnumerable<MobileOperatorPackage>> GetByOperatorAsync(int operatorId)
        {
            return await _context.MobileOperatorPackages
                .Where(x => x.OperatorId == operatorId && x.IsActive == 1 && x.IsDeleted == 0)
                .Select(x=> new MobileOperatorPackage
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreationTime = x.CreationTime,
                    IsActive = x.IsActive,
                    OperatorId = x.OperatorId,
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync();
        }
        public MobileOperatorPackage GetByOperator(int operatorId, string packagename)
        {
            return _context.MobileOperatorPackages
                .Where(x => x.OperatorId == operatorId && x.Name == packagename && x.IsActive == 1 && x.IsDeleted == 0)
                .Select(x => new MobileOperatorPackage
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreationTime = x.CreationTime,
                    IsActive = x.IsActive,
                    OperatorId = x.OperatorId,
                    IsDeleted = x.IsDeleted
                }).FirstOrDefault();
        }
        public async Task<MobileOperatorPackage> GetByOperatorAsync(int operatorId, string packagename)
        {
            return await _context.MobileOperatorPackages
                .Where(x => x.OperatorId == operatorId && x.Name == packagename && x.IsActive == 1 && x.IsDeleted == 0)
                .Select(x => new MobileOperatorPackage
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreationTime = x.CreationTime,
                    IsActive = x.IsActive,
                    OperatorId = x.OperatorId,
                    IsDeleted = x.IsDeleted
                }).FirstOrDefaultAsync();
        }
        public async Task<MobileOperatorPackage> GetByOperatorAsync(int id, int operatorId, string packagename)
        {
            return await _context.MobileOperatorPackages
                .Where(x => x.Id != id && x.OperatorId == operatorId && x.Name == packagename && x.IsActive == 1 && x.IsDeleted == 0)
                .Select(x => new MobileOperatorPackage
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    CreationTime = x.CreationTime,
                    IsActive = x.IsActive,
                    OperatorId = x.OperatorId,
                    IsDeleted = x.IsDeleted
                }).FirstOrDefaultAsync();
        }
        public MobileOperatorPackage Insert(MobileOperatorPackage entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public MobileOperatorPackage Insert(string name, int operatorId, string description = null)
        {
            var entity = new MobileOperatorPackage
            {
                Name = name,
                OperatorId = operatorId,
                Description = description
            };
            return Insert(entity);
        }
        public async Task<MobileOperatorPackage> InsertAsync(MobileOperatorPackage entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }
        public async Task<MobileOperatorPackage> InsertAsync(string name, int operatorId, string description = null)
        {
            var entity = new MobileOperatorPackage
            {
                Name = name,
                OperatorId = operatorId,
                Description = description
            };
            return await InsertAsync(entity);
        }
        public async Task<MobileOperatorPackage> UpdateAsync(MobileOperatorPackage entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _context.MobileOperatorPackages.FirstOrDefaultAsync(x => x.Id == id);
            entity.IsDeleted = 1;
            _context.Entry(entity).State = EntityState.Modified;
            //_context.MobileOperatorPackages.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UsesCountAsync(int id)
        {
            return await _context.SimCards.CountAsync(x => x.OperatorPackageId == id);
        }

        public async Task<MobileOperatorPackage> UpdateAsync(int id, string name, string description, int operatorId)
        {
            var entity = await _context.MobileOperatorPackages.FirstOrDefaultAsync(x => x.Id == id);
            entity.Name = name;
            entity.Description = description;
            entity.OperatorId = operatorId;
            return await UpdateAsync(entity);
        }

        public async Task<IEnumerable<OperatorPackageResponseDto>> GetAsync()
        {
            var response = _context.MobileOperatorPackages.Where(x=> x.IsDeleted == 0).Select(x => new OperatorPackageResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                OperatorId = x.OperatorId,
                Operator = x.Operator.Name,
                IsActive = x.IsActive,
                IsDeleted = x.IsDeleted,
                CreationTime = x.CreationTime
            }).AsEnumerable();
            return await Task.FromResult(response);
        }
    }
}
