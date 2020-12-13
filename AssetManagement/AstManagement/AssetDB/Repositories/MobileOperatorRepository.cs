using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.AssetDB.Repositories
{
    public interface IMobileOperatorRepository
    {
        IEnumerable<MobileOperator> Get();
        Task<IEnumerable<MobileOperator>> GetAsync();
        Task<MobileOperator> GetAsync(int id);
        Task<MobileOperator> GetAsync(string name);
        Task<MobileOperator> GetAsync(int id, string name);
        Task<MobileOperator> InsertAsync(string name);
        Task<MobileOperator> UpdateAsync(int id, string name);
        Task<MobileOperator> UpdateAsync(MobileOperator model);
        Task<int> DeleteAsync(int id);
        Task<bool> IsUsed(int id);
    }
    public class MobileOperatorRepository: IMobileOperatorRepository
    {
        private readonly EyeAssetDbContext _context;
        public MobileOperatorRepository(EyeAssetDbContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var opt = await _context.MobileOperators.FirstOrDefaultAsync(x=> x.Id == id);
            opt.IsDeleted = 1;
            _context.Entry(opt).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public IEnumerable<MobileOperator> Get()
        {
            return _context.MobileOperators;
        }
        public async Task<IEnumerable<MobileOperator>> GetAsync()
        {
            return await _context.MobileOperators.Where(x=> x.IsDeleted == 0).ToListAsync();
        }
        public async Task<MobileOperator> GetAsync(int id)
        {
            return await _context.MobileOperators.FirstOrDefaultAsync(x=> x.Id == id && x.IsDeleted == 0);
        }
        public async Task<MobileOperator> GetAsync(string name)
        {
            return await _context.MobileOperators.FirstOrDefaultAsync(x => x.Name == name && x.IsDeleted == 0);
        }
        public async Task<MobileOperator> GetAsync(int id, string name)
        {
            return await _context.MobileOperators.FirstOrDefaultAsync(x => x.Id != id && x.Name == name);
        }
        public async Task<MobileOperator> InsertAsync(string name)
        {
            int maxId = await _context.MobileOperators.MaxAsync(x => x.Id);
            var entity = new MobileOperator
            {
                Id = maxId + 1,
                Name = name
            };
            _context.MobileOperators.Add(entity);
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task<bool> IsUsed(int id)
        {
            return await _context.MobileOperatorPackages.AnyAsync(x => x.OperatorId == id);
        }

        public async Task<MobileOperator> UpdateAsync(MobileOperator model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await Task.FromResult(model);
        }

        public async Task<MobileOperator> UpdateAsync(int id, string name)
        {
            var entity = await _context.MobileOperators.FirstOrDefaultAsync(x => x.Id == id);
            entity.Name = name;
            return await UpdateAsync(entity);
        }
    }
}
