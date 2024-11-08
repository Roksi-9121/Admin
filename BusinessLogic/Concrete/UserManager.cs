using BusinessLogic.Interface;
using DAL.Interface;
using DTO;

namespace BusinessLogic.Concrete
{
    public class UserManager : IUserManager
    {
        private readonly IUserDAL _userDal;

        public UserManager(IUserDAL userDal)
        {
            _userDal = userDal;
        }

        public User FindUserByEmailAndPassword(string email, string password)
        {
            return _userDal.FindUserByEmailAndPassword(email, password);
        }
        public User CreateUser(User user)
        {
            return _userDal.CreateUser(user);
        }

        public List<User> ReadUsers()
        {
            return _userDal.ReadUsers();
        }

        public List<User> SortUsersByName()
        {
            return _userDal.SortUsersByName();
        }
        public List<User> SearchUsers(string name)
        {
            return _userDal.SearchUsers(name);
        }
        public bool UpdateUserStatus(string userName, bool isActive)
        {
            return _userDal.UpdateUserStatus(userName, isActive);
        }

        public bool RemoveUser(string userName)
        {
            return _userDal.RemoveUser(userName);
        }
    }
}
