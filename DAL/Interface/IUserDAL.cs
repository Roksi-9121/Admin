using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUserDAL
    {
        
        User FindUserByEmailAndPassword(string email, string password);
        User CreateUser(User user);
        List<User> ReadUsers();
        bool UpdateUserStatus(string userName, bool isActive);
        List<User> SearchUsers(string name);
        List<User> SortUsersByName();
        bool RemoveUser(string user_name);
    }
}
