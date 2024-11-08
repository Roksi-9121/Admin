using BusinessLogic.Concrete;
using DAL.Interface;
using DTO;
using Moq;

namespace TestProject
{
    public class Tests
    {
        [TestFixture]
        public class UserManagerTests
        {
            private Mock<IUserDAL> _mockUserDal;
            private UserManager _userManager;

            [SetUp]
            public void SetUp()
            {
                
                _mockUserDal = new Mock<IUserDAL>();
                _userManager = new UserManager(_mockUserDal.Object);
            }

            [Test]
            public void CreateUser_ShouldReturnUser_WhenUserIsCreatedSuccessfully()
            {
                
                var newUser = new User { User_name = "TestUser", Email = "test@example.com", Is_active = true, Is_admin = false };
                _mockUserDal.Setup(dal => dal.CreateUser(newUser)).Returns(newUser);

                
                var result = _userManager.CreateUser(newUser);

                
                Assert.IsNotNull(result);
                Assert.AreEqual("TestUser", result.User_name);
            }

            [Test]
            public void ReadUsers_ShouldReturnListOfUsers()
            {
                
                var users = new List<User>
            {
                new User { User_name = "User1" },
                new User { User_name = "User2" }
            };
                _mockUserDal.Setup(dal => dal.ReadUsers()).Returns(users);

                
                var result = _userManager.ReadUsers();

                
                Assert.AreEqual(2, result.Count);
            }

            [Test]
            public void UpdateUserStatus_ShouldReturnTrue_WhenStatusIsUpdated()
            {
                
                _mockUserDal.Setup(dal => dal.UpdateUserStatus("TestUser", true)).Returns(true);

                
                var result = _userManager.UpdateUserStatus("TestUser", true);

                
                Assert.IsTrue(result);
            }

            [Test]
            public void RemoveUser_ShouldReturnTrue_WhenUserIsRemoved()
            {
                
                _mockUserDal.Setup(dal => dal.RemoveUser("TestUser")).Returns(true);

                
                var result = _userManager.RemoveUser("TestUser");

                
                Assert.IsTrue(result);
            }
        }
    }
}