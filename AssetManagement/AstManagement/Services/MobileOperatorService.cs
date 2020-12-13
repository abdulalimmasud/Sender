using AstManagement.AssetDB;
using AstManagement.AssetDB.Repositories;
using AstManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.Services
{
    public interface IMobileOperatorService
    {
        Task<IEnumerable<MobileOperator>> GetAsync();
        Task<MobileOperator> GetAsync(int id);
        Task<MobileOperator> InsertAsync(Entity dto);
        Task<MobileOperator> UpdateAsync(MobileOperator model);
        Task<int> DeleteAsync(int id);
    }
    public class MobileOperatorService:IMobileOperatorService
    {
        private readonly IMobileOperatorRepository _repository;
        public MobileOperatorService(IMobileOperatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> DeleteAsync(int id)
        {
            if(!await _repository.IsUsed(id))
            {
                return await _repository.DeleteAsync(id);
            }
            return 0;
        }

        public async Task<IEnumerable<MobileOperator>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<MobileOperator> GetAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<MobileOperator> InsertAsync(Entity dto)
        {
            var exist = await _repository.GetAsync(dto.Name);
            if(exist == null)
            {
                return await _repository.InsertAsync(dto.Name);
            }
            return null;
        }

        public async Task<MobileOperator> UpdateAsync(MobileOperator model)
        {
            var exist = await _repository.GetAsync(model.Id, model.Name);
            if (exist == null)
            {
                return await _repository.UpdateAsync(model.Id, model.Name);
            }
            return null;
        }
    }
}
