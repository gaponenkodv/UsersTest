using System;
using AppTest.Db.Models;
using AppTest.Db.Seed;
using Microsoft.EntityFrameworkCore;

namespace AppTest.Db
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Seed();
    }
}
