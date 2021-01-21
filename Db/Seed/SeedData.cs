using System;
using System.Collections.Generic;
using System.Linq;
using AppTest.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace AppTest.Db.Seed
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var roles = new List<Role>
            {
                new Role{ Id = 1, Name = "Admin"},
                new Role{ Id = 2, Name = "Redactor"},
                new Role{ Id = 3, Name = "Customer"}
            };

            modelBuilder.Entity<Role>().HasData(roles);

            string pass = "admin";

            var user = new User
            {
                Id = 1,
                Name = "Name",
                Login = "admin",
                Email = "admin@admin.ru",
            };

            CreatePasswordHash(pass, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            modelBuilder.Entity<User>().HasData(user);
        }

        static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
