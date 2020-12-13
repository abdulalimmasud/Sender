using AstManagement.AssetDB.Repositories;
using AstManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.Services
{
    public interface IAttachmentLogService
    {
        Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int simCardId);
        Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int assetId, int appId);
    }
    public class AttachmentLogService: IAttachmentLogService
    {
        private readonly IAttachmentLogRepository _repository;
        public AttachmentLogService(IAttachmentLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int simCardId)
        {
            return await _repository.GetAsync(simCardId);
        }

        public async Task<IEnumerable<AttachmentHistoryDto>> GetAsync(int assetId, int appId)
        {
            return await _repository.GetAsync(assetId, appId);
        }
    }
}
