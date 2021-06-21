using Microsoft.EntityFrameworkCore;
using SmartEduSolutions.Databases.SEDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEduSolutions.Databases
{
    public class SEDBContext : DbContext
    {
        private readonly string _connectionString;
        public SEDBContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public SEDBContext(DbContextOptions<SEDBContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Classrooms> Classrooms { get; set; }
        public virtual DbSet<User_has_Classroom> User_has_Classroom { get; set; }
        public virtual DbSet<Assignments> Assignments { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString);
            }
        }
    }
}
