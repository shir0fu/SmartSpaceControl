using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartSpaceControl.Models.Models;

namespace SmartSpaceControl.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<Measure> Measures { get; set; }
        public virtual DbSet<SensorType> SensorTypes { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<Area> Areas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}