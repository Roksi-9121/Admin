using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface IUserManager
    {
        User FindUserByEmailAndPassword(string email, string password);
        User CreateUser(User user);
        List<User> ReadUsers();
        List<User> SortUsersByName();
        List<User> SearchUsers(string name);
        bool UpdateUserStatus(string userName, bool isActive);

        bool RemoveUser(string userName);

    }
}
