using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AstManagement.AssetDB.Repositories
{
    public interface IAppRepository
    {
        IEnumerable<App> Get();
        Task<IEnumerable<App>> GetAsync();
        Task<App> GetAsync(int id);
        Task<App> GetAsync(string name);
        Task<App> GetAsync(int id, string name);
        Task<App> InsertAsync(string name);
        Task<App> UpdateAsync(App entity);
    }
    public class AppRepository: IAppRepository
    {
        private readonly EyeAssetDbContext _context;
        public AppRepository(EyeAssetDbContext context)
        {
            _context = context;
        }
        public IEnumerable<App> Get()
        {
            return _context.Apps;
        }
        public async Task<IEnumerable<App>> GetAsync()
        {
            return await _context.Apps.ToListAsync();
        }

        public async Task<App> GetAsync(int id)
        {
            return await _context.Apps.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<App> GetAsync(string name)
        {
            return await _context.Apps.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<App> GetAsync(int id, string name)
        {
            return await _context.Apps.FirstOrDefaultAsync(x => x.Id != id && x.Name == name);
        }
        
        public async Task<App> InsertAsync(string name)
        {
            int maxId = await _context.Apps.MaxAsync(x => x.Id);
            var entity = new App
            {
                Id = maxId + 1,
                Name = name
            };
            _context.Apps.Add(entity);
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public async Task<App> UpdateAsync(App entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }
    }
}
