using LogicalModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnesLogical.Interface
{
    public interface IUserService
    {
        Task<bool> NotNullUser(User user);
        Task<User> GetCheckAsync(User checkUser);
        Task<User> FindUser(User user);
    }
}
