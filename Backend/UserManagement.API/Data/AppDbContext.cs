using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UserManagement.API.Models;

namespace UserManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UserGroup composite key
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            // GroupPermission composite key
            modelBuilder.Entity<GroupPermission>()
                .HasKey(gp => new { gp.GroupId, gp.PermissionId });

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Group)
                .WithMany(g => g.GroupPermissions)
                .HasForeignKey(gp => gp.GroupId);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Permission)
                .WithMany(p => p.GroupPermissions)
                .HasForeignKey(gp => gp.PermissionId);
        }
    }

}
