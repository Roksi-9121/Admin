using Microsoft.Extensions.Configuration;
using DAL.Concrete;
using DTO;
using System.Collections.Generic;
using DAL;

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("C:\\Users\\Роксоляна\\source\\repos\\Administrator\\Admin\\config.json")
    .Build();

string connectionString = configuration.GetConnectionString("Admin") ?? "";

while (true)
{
    Console.WriteLine("Choose option: \n" +
        "1. Log in\n" +
        "2. SignUp User\n" +
        "0. Exit\n");
    char option = Console.ReadKey().KeyChar;
    Console.WriteLine();

    switch (option)
    {
        case '1':
            if (LogInUser()) return;
            break;
        case '2':
            if (SignUpUser()) return;
            break;
        case '0':
            Console.WriteLine("Exiting the program...");
            return;
        default:
            Console.WriteLine("Incorrect option!");
            break;
    }
}

bool LogInUser()
{
    Console.WriteLine("Enter your Email:");
    string email = Console.ReadLine();

    Console.WriteLine("Enter your password:");
    string password = Console.ReadLine();

    var dal = new UserDAL(connectionString);
    var user = dal.FindUserByEmailAndPassword(email, password);

    if (user != null)
    {
        Console.WriteLine($"Welcome, {user.User_name}!");
        if (user.Is_admin)
        {
            AdminMenu();
        }
        else
        {
            UserMenu();
        }
        return true;
    }
    else
    {
        Console.WriteLine("User not found or incorrect password.");
        return false;
    }
}


bool SignUpUser()
{
    Console.WriteLine("Enter your name:");
    string name = Console.ReadLine();

    Console.WriteLine("Enter your email:");
    string email = Console.ReadLine();

    Console.WriteLine("Enter your password:");
    string password = Console.ReadLine();


    string passwordHash = PasswordHelper.HashPassword(password);


    var newUser = new User
    {
        User_name = name,
        Email = email,
        Password = passwordHash,
        Is_active = true,
        Is_admin = true
    };


    var dal = new UserDAL(connectionString);
    dal.CreateUser(newUser);

    Console.WriteLine("User registered successfully!");


    AdminMenu();

    return true;
}

void AdminMenu()
{
    while (true)
    {
        Console.WriteLine("Admin Menu:\n" +
            "1. View all users\n" +
            "2. Activate user account\n" +
            "3. Block user account\n" +
            "4. Search users\n" +
            "5. Sort users\n" +
            "0. Log out\n");

        char choice = Console.ReadKey().KeyChar;
        Console.WriteLine();

        switch (choice)
        {
            case '1':
                DisplayUsers(new UserDAL(connectionString).ReadUsers());
                break;
            case '2':
                Console.WriteLine("Enter User Name to activate:");
                string activateName = Console.ReadLine();
                new UserDAL(connectionString).UpdateUserStatus(activateName, true);
                Console.WriteLine("User activated.");
                break;
            case '3':
                Console.WriteLine("Enter User ID to block:");
                string blockName = Console.ReadLine();
                new UserDAL(connectionString).UpdateUserStatus(blockName, false);
                Console.WriteLine("User blocked.");
                break;
            case '4':
                Console.WriteLine("Enter name to search:");
                string name = Console.ReadLine();
                DisplayUsers(new UserDAL(connectionString).SearchUsers(name));
                break;
            case '5':
                DisplayUsers(new UserDAL(connectionString).SortUsersByName());
                break;
            case '0':
                Console.WriteLine("Logging out...");
                return;
            default:
                Console.WriteLine("Invalid option. Try again.");
                break;
        }
    }
}

void UserMenu()
{
    Console.WriteLine("User Menu:\n" +
                      "You have limited access.\n" +
                      "Press any key to log out...");
    Console.ReadKey();
}

void DisplayUsers(List<User> users)
{
    foreach (var item in users)
    {
        Console.WriteLine($"{item.User_id}, {item.User_name}, Active: {item.Is_active}, Admin: {item.Is_admin}");
    }
}



