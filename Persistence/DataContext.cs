using Domain;
using Domain.Maps;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<GridLayoutMaster> GridLayoutMasters {get;set;}
        public DbSet<GridLayoutDetail> GridLayoutDetails { get; set; }
        public DbSet<Rights> Rights { get; set; }
        public DbSet<RightsAllotmentM> RightsAllotmentMs { get; set; }
        public DbSet<RightsAllotmentD> RightsAllotmentDs { get; set; }
        public DbSet<RightsAllotmentHistory> RightsAllotmentHistories { get; set; }
        public DbSet<global> Globals { get; set; }
        public DbSet<LogFile> LogFiles { get; set; }
        public DbSet<UserView> UserViews { get; set; }
        public DbSet<MapCategories> MapCategories { get; set; }
        public DbSet<MapList> MapLists{get;set;}
        public DbSet<MapPosition> MapPositions{get;set;}
    }
}