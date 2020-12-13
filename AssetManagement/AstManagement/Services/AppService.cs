using AstManagement.AssetDB;
using AstManagement.AssetDB.Repositories;
using AstManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.Services
{
    public interface IAppService
    {
        Task<IEnumerable<App>> GetAsync();
        Task<App> GetAsync(int id);
        Task<App> InsertAsync(Entity dto);
        Task<App> UpdateAsync(App entity);
    }
    public class AppService:IAppService
    {
        private readonly IAppRepository _repository;
        public AppService(IAppRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<App>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<App> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<App> InsertAsync(Entity dto)
        {
            var exist = await _repository.GetAsync( dto.Name);
            if (exist == null)
            {
                return await _repository.InsertAsync(dto.Name);
            }
            return null;
        }

        public async Task<App> UpdateAsync(App entity)
        {
            var exist = await _repository.GetAsync(entity.Id, entity.Name);
            if (exist == null)
            {
                return await _repository.UpdateAsync(entity);
            }
            return null;
        }
    }
}
