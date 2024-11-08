using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Interface;
using DTO;

namespace DAL.Concrete
{
    public class UserDAL : IUserDAL
    {
        private readonly string _connectionString;

        public UserDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Знаходить користувача за email та паролем
        public User FindUserByEmailAndPassword(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Email = @Email";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = new User
                            {
                                User_id = (int)reader["User_id"],
                                User_name = reader["User_name"].ToString(),
                                Is_active = (bool)reader["Is_active"],
                                Is_admin = (bool)reader["Is_admin"],
                                Password = reader["Password_hash"].ToString()
                            };
                            if (PasswordHelper.VerifyPassword(password, user.Password))
                            {
                                return user;
                            }
                        }
                    }
                }
            }
            return null;
        }

        //Create User
        public User CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (User_name, Email, Password_hash, Is_active, Is_admin) VALUES (@UserName, @Email, @PasswordHash, @IsActive, @IsAdmin)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.User_name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@PasswordHash", user.Password);
                    command.Parameters.AddWithValue("@IsActive", user.Is_active);
                    command.Parameters.AddWithValue("@IsAdmin", user.Is_admin);

                    var newUserId = Convert.ToInt32(command.ExecuteScalar());
                    user.User_id = newUserId;

                    return user;
                }
            }
        }


        // Отримує всіх користувачів
        public List<User> ReadUsers()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            User_id = (int)reader["User_id"],
                            User_name = reader["User_name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Is_active = (bool)reader["Is_active"],
                            Is_admin = (bool)reader["Is_admin"]
                        });
                    }
                }
            }
            return users;
        }

        // Оновлення статусу користувача (активувати/блокувати)
        public bool UpdateUserStatus(string userName, bool isActive)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Is_active = @IsActive WHERE User_name = @UserName";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@UserName", userName);

                    int rowsAffected = command.ExecuteNonQuery(); 

                    
                    return rowsAffected > 0;
                }
            }
        }

        // Пошук користувачів за ім'ям
        public List<User> SearchUsers(string name)
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE User_name LIKE @Name";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", "%" + name + "%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                User_id = (int)reader["User_id"],
                                User_name = reader["User_name"].ToString(),
                                Is_active = (bool)reader["Is_active"],
                                Is_admin = (bool)reader["Is_admin"]
                            });
                        }
                    }
                }
            }
            return users;
        }

        // Сортування користувачів за іменем
        public List<User> SortUsersByName()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users ORDER BY User_name";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            User_id = (int)reader["User_id"],
                            User_name = reader["User_name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Is_active = (bool)reader["Is_active"],
                            Is_admin = (bool)reader["Is_admin"]
                        });
                    }
                }
            }
            return users;
        }

        //Remove User
        public bool RemoveUser(string user_name)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Users WHERE User_name = @user_name";
                command.Parameters.AddWithValue("@user_name", user_name);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                
                return rowsAffected > 0;
            }
        }
    }
}
