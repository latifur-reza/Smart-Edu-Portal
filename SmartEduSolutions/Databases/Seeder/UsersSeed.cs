using Microsoft.AspNetCore.Identity;
using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases.Seeder
{
    public class UsersSeed
    {
        private readonly IPasswordHasher<Users> _passwordHasher;

        public UsersSeed(IPasswordHasher<Users> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        private List<Users> Users
        {
            get
            {
                return new List<Users>
                {
                    new Users
                    {
                        IdUsers = 1,
                        Username = "Reza1",
                        Email = "reza1@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                     new Users
                    {
                        IdUsers = 2,
                        Username = "Reza2",
                        Email = "reza2@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                      new Users
                    {
                        IdUsers = 3,
                        Username = "Reza3",
                        Email = "reza3@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                    new Users
                    {
                        IdUsers = 4,
                        Username = "Reza4",
                        Email = "reza4@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                     new Users
                    {
                        IdUsers = 5,
                        Username = "Reza5",
                        Email = "reza5@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                    new Users
                    {
                        IdUsers = 6,
                        Username = "Reza6",
                        Email = "reza6@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                      new Users
                    {
                        IdUsers = 7,
                        Username = "Reza7",
                        Email = "reza7@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                      new Users
                    {
                        IdUsers = 8,
                        Username = "Reza8",
                        Email = "reza8@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                      new Users
                    {
                        IdUsers = 9,
                        Username = "Reza9",
                        Email = "reza9@smart-edu.com",

                        CreatedAt = DateTime.Now
                    },
                      new Users
                    {
                        IdUsers = 10,
                        Username = "Reza10",
                        Email = "reza10@smart-edu.com",

                        CreatedAt = DateTime.Now
                    }
                };
            }
        }

        public List<Users> GetUsers()
        {
            var users = new List<Users>();
            foreach (var user in Users)
            {
                user.Password = _passwordHasher.HashPassword(user, "password");
                users.Add(user);
            }
            return users;

        }

    }
}
