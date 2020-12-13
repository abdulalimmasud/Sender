using AstManagement.AssetDB;
using AstManagement.AssetDB.Repositories;
using AstManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.Services
{
    public interface IMobileOperatorPackagesService
    {
        Task<IEnumerable<OperatorPackageResponseDto>> GetAsync();
        Task<IEnumerable<MobileOperatorPackage>> GetByOperatorAsync(int operatorId);
        Task<MobileOperatorPackage> GetByOperatorAsync(int operatorId, string packagename);
        Task<MobileOperatorPackage> InsertAsync(OperatorPackageInsertionDto dto);
        Task<MobileOperatorPackage> UpdateAsync(MobileOperatorPackage entity);
        Task<int> DeleteAsync(int id);
    }
    public class MobileOperatorPackagesService:IMobileOperatorPackagesService
    {
        private readonly IMobileOperatorPackagesRepository _repository;
        public MobileOperatorPackagesService(IMobileOperatorPackagesRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> DeleteAsync(int id)
        {
            int usesCount = await _repository.UsesCountAsync(id);
            if(usesCount < 1)
            {
                return await _repository.DeleteAsync(id);
            }
            return 0;
        }

        public async Task<IEnumerable<OperatorPackageResponseDto>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<IEnumerable<MobileOperatorPackage>> GetByOperatorAsync(int operatorId)
        {
            return await _repository.GetByOperatorAsync(operatorId);
        }

        public async Task<MobileOperatorPackage> GetByOperatorAsync(int operatorId, string packagename)
        {
            return await _repository.GetByOperatorAsync(operatorId, packagename);
        }

        public async Task<MobileOperatorPackage> InsertAsync(OperatorPackageInsertionDto dto)
        {
            var exist = await _repository.GetByOperatorAsync(dto.OperatorId, dto.Name);
            if(exist == null)
            {
                return await _repository.InsertAsync(dto.Name, dto.OperatorId, dto.Description);
            }
            return null;
        }

        public async Task<MobileOperatorPackage> UpdateAsync(MobileOperatorPackage entity)
        {
            var exist = await _repository.GetByOperatorAsync(entity.Id, entity.OperatorId, entity.Name);
            if (exist == null)
            {
                return await _repository.UpdateAsync(entity.Id, entity.Name, entity.Description, entity.OperatorId);
            }
            return null;            
        }
    }
}
