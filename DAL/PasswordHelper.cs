using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace DAL
{
    public class PasswordHelper
    {
        // Генерація хешу з використанням PBKDF2
        public static string HashPassword(string password)
        {
            // Генерація випадкової солі
            byte[] salt = GenerateSalt();

            // Хешування пароля з використанням PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(32); // Отримання хешу розміром 32 байти

                // Повернення солі та хешу у вигляді рядка для зберігання
                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }

        // Перевірка введеного пароля із збереженим хешем
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Перевірка наявності символу розділювача
            if (!storedHash.Contains(":"))
            {
                throw new InvalidOperationException("Invalid password hash format: missing separator ':'");
            }

            // Розділення збереженого хешу на сіль і хеш
            var parts = storedHash.Split(':');

            // Перевірка, чи правильно розділено
            if (parts.Length != 2 || string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
            {
                throw new InvalidOperationException("Invalid password hash format: salt or hash is missing.");
            }

            // Далі код залишається тим самим...
            try
            {
                var salt = Convert.FromBase64String(parts[0]);
                var hash = Convert.FromBase64String(parts[1]);

                // Хешування введеного пароля з тією ж сіллю
                using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000))
                {
                    byte[] enteredHash = pbkdf2.GetBytes(32);

                    // Порівняння масивів байтів
                    return enteredHash.SequenceEqual(hash);
                }
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("The stored password hash format is invalid.");
            }
        }


        // Генерація випадкової солі
        private static byte[] GenerateSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
