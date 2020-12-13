using AstManagement.AssetDB;
using AstManagement.AssetDB.Repositories;
using AstManagement.Util;
using AstManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.Services
{
    public interface IUserService
    {
        Task<User> GetAsync(string username);
        Task<User> Register(UserRegistrationDto dto);
    }
    public class UserService:IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<User> GetAsync(string username)
        {
            return await _repository.GetAsync(username);
        }
        public async Task<User> Register(UserRegistrationDto dto)
        {
            var existing = await _repository.GetAsync(dto.Username);
            if (existing == null)
            {
                byte[] hash, salt;
                SecurityManagement.EncryptPassword(dto.Password, out hash, out salt);
                return await _repository.InsertAsync(dto.Name, dto.Username, hash, salt, dto.Email);
            }
            return null;
        }
    }
}
