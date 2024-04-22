using BuissnesLogical.Interface;
using Microsoft.EntityFrameworkCore;
using LogicalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnesLogical.Service
{
    public class UserSrvice : IRepository<User>, IUserService
    {
        BankDbContext _bankDbContext;
        public UserSrvice(BankDbContext bankDbContext) 
        {
            _bankDbContext = bankDbContext;
        }

        public async Task CreateAsync(User entity)
        {
            if (entity != null)
            {
                entity.UserTypes = UserType.Клиент;
                await _bankDbContext.Users.AddAsync(entity);
                await _bankDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
              var user=  await _bankDbContext.Users.FindAsync(id);
                _bankDbContext.Users.Remove(user);
                await _bankDbContext.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _bankDbContext.Users.Include(a => a.Applications).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _bankDbContext.Users.Include(a=>a.Applications).FirstOrDefaultAsync(i=>i.UserID == id);
        }

        public async Task<User> GetCheckAsync(User checkUser)
        {
            var user = await _bankDbContext.Users.FirstOrDefaultAsync(u =>
                  u.Name == checkUser.Name);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetListByID(int id)
        {
            return await _bankDbContext.Users.Where(c=>c.UserID==id).Include(a => a.Applications).ToListAsync();
        }

        public async Task<bool> NotNullUser(User user)
        {
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
            {
                return false;
            }
            return true;
        }
        public async Task<User> FindUser(User user)
        {
            var userfind = await _bankDbContext.Users.FirstOrDefaultAsync(c => c.Name == user.Name && c.Password == user.Password);
            return userfind;
        }

        public async Task UpdateAsync(User entity)
        {
            if (entity != null)
            {
                _bankDbContext.Users.Update(entity);
                await _bankDbContext.SaveChangesAsync();
            }
        }
    }
}
