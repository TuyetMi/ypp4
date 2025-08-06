using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace MsListsApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<WorkspaceMember> WorkspaceMembers { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<RecentList> RecentLists { get; set; }
        public DbSet<ListPermission> ListPermissions { get; set; }
        public DbSet<ListMemberPermission> ListMemberPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* ----------------------------------------------------------------- */
            // WorkspaceMember
            // Unique constraint: 1 account chỉ join 1 workspace 1 lần
            modelBuilder.Entity<WorkspaceMember>()
                .HasIndex(wm => new { wm.WorkspaceId, wm.AccountId })
                .IsUnique();

            // Thiết lập mối quan hệ
            modelBuilder.Entity<WorkspaceMember>()
                .HasOne(wm => wm.Workspace)
                .WithMany()
                .HasForeignKey(wm => wm.WorkspaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkspaceMember>()
                .HasOne(wm => wm.Account)
                .WithMany()
                .HasForeignKey(wm => wm.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            /* ----------------------------------------------------------------- */
            // RecentList
            modelBuilder.Entity<RecentList>()
                .HasIndex(r => new { r.AccountId, r.ListId })
                .IsUnique();

            modelBuilder.Entity<RecentList>()
                .HasOne<Account>()
                .WithMany()
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecentList>()
                .HasOne<List>()
                .WithMany()
                .HasForeignKey(r => r.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            /* ----------------------------------------------------------------- */
            modelBuilder.Entity<ListMemberPermission>()
                .HasOne(p => p.Account)
                .WithMany()
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ListMemberPermission>()
                .HasOne(p => p.GrantedByAccount)
                .WithMany()
                .HasForeignKey(p => p.GrantedByAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ListMemberPermission>()
                .HasOne(p => p.List)
                .WithMany()
                .HasForeignKey(p => p.ListId);

            modelBuilder.Entity<ListMemberPermission>()
                .HasOne(p => p.HighestPermission)
                .WithMany(p => p.ListMemberPermissions)
                .HasForeignKey(p => p.HighestPermissionId);
        }
    }
}
