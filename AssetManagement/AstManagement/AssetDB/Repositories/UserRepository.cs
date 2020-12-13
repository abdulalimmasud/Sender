using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstManagement.AssetDB.Repositories
{
    public interface IUserRepository
    {
        User Get(int id);
        Task<User> GetAsync(int id);
        User Get(string username);
        Task<User> GetAsync(string username);
        User Insert(User entity);
        User Insert(string name, string username, byte[] hash, byte[] salt, string email = null);
        Task<User> InsertAsync(User entity);
        Task<User> InsertAsync(string name, string username, byte[] hash, byte[] salt, string email = null);
    }
    public class UserRepository: IUserRepository
    {
        private readonly EyeAssetDbContext _context;
        public UserRepository(EyeAssetDbContext context)
        {
            _context = context;
        }
        public User Get(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id && x.IsActive == 1 && x.IsDeleted == 0);
        }
        public async Task<User> GetAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == 1 && x.IsDeleted == 0);
        }
        public User Get(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username);
        }
        public async Task<User> GetAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }
        public User Insert(User entity)
        {
            _context.Users.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public User Insert(string name, string username, byte[] hash, byte[] salt, string email = null)
        {
            var entity = new User
            {
                Name = name,
                Username = username,
                PasswordHash = hash,
                PasswordSalt = salt,
                Email = email
            };
            return Insert(entity);
        }
        public async Task<User> InsertAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await Task.FromResult(entity);
        }
        public async Task<User> InsertAsync(string name, string username, byte[] hash, byte[] salt, string email = null)
        {
            var entity = new User
            {
                Name = name,
                Username = username,
                PasswordHash = hash,
                PasswordSalt = salt,
                Email = email
            };
            return await InsertAsync(entity);
        }
    }
}
