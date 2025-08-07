
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

    }
}
