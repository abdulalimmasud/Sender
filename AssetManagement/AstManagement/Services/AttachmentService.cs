using AstManagement.AssetDB;
using AstManagement.AssetDB.Repositories;
using AstManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.Services
{
    public interface IAttachmentService
    {
        Task<int> DeleteAsync(int simCardId, int userId, string username);
        Task<AttachmentResponseShortDto> GetAsync(string search);
        Task<AttachmentResponseShortDto> GetAsync(int assetId, int appId);
        Task<EntityList<AttachmentDto>> GetAsync(int page, int pageSize, string search = "");
        Task<ResponseResult> InsertAsync(AttachmentInsertionDto dto);
        Task<Attachment> InsertOrUpdateAsync(AttachmentInsertionDto dto);
    }
    public class AttachmentService: IAttachmentService
    {
        private readonly IAttachmentRepository _repository;
        public AttachmentService(IAttachmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> DeleteAsync(int simCardId, int userId, string username)
        {
            return await _repository.DeleteAsync(simCardId, userId, username);
        }

        public async Task<EntityList<AttachmentDto>> GetAsync(int page, int pageSize, string search = "")
        {
            return await _repository.GetAsync(page, pageSize, search);
        }

        public async Task<AttachmentResponseShortDto> GetAsync(int assetId, int appId)
        {
            var data = new AttachmentResponseShortDto();
            var response = await _repository.GetAsync(assetId, appId);
            if(response != null)
            {
                data = response;
            }
            return await Task.FromResult(data);
        }

        public async Task<AttachmentResponseShortDto> GetAsync(string search)
        {
            if(search.Length > 11)
            {
                search = search.Substring(2, search.Length - 2);
            }
            return await _repository.GetAsync(search);
        }

        public async Task<ResponseResult> InsertAsync(AttachmentInsertionDto dto)
        {
            var exist = await _repository.GetAsync(dto.SimCardId);
            if (exist == null)
            {
                var deviceExist = await _repository.GetAsync(dto.AssetId, dto.AppId);
                if(deviceExist == null)
                {
                    return new ResponseResult
                    {
                        StatusCode = 200,
                        Message = await _repository.InsertAsync(dto.SimCardId, dto.AssetId, dto.AppId, dto.UserId, dto.Username)
                    };
                }
                return new ResponseResult
                {
                    StatusCode = 409,
                    Message = "Device have a sim. Please detuched first."
                };
            }
            return new ResponseResult
            {
                StatusCode = 409,
                Message = "Simcard already attached with a device."
            };
        }

        public async Task<Attachment> InsertOrUpdateAsync(AttachmentInsertionDto dto)
        {
            var exist = await _repository.GetAsync(dto.SimCardId);
            if(exist == null)
            {
                return await _repository.InsertAsync(dto.SimCardId, dto.AssetId, dto.AppId, dto.UserId, dto.Username);
            }
            else
            {
                return _repository.Update(dto.SimCardId, dto.AssetId, dto.AppId, dto.UserId, dto.Username, exist);
            }
        }
    }
}
