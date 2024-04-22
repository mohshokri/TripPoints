using Microsoft.EntityFrameworkCore;
using TripPoints.Entities;

namespace TripPoints.Infrastructure
{
    public class TripDbContext : DbContext
    {
        public TripDbContext(DbContextOptions<TripDbContext> options) : base(options)
        {

        }
        public DbSet<Poly> Polies { get; set; }
        public DbSet<CarTrip> CarTrips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Introduce Your Provider Like: SQLServer, MySQL, SQLLight
            //optionsBuilder.UseSqlServer("DefaultConnection");

            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=TripDbContext;Trusted_Connection=True;TrustServerCertificate=True;", x => x.UseNetTopologySuite());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.Entity<Poly>().HasIndex(p => p.Geometry).IsClustered(false);
            modelBuilder.Entity<CarTrip>(p =>
            {
                p.Property(c => c.StartPoint).HasColumnType("geometry");
                p.Property(c => c.EndPoint).HasColumnType("geometry");
            });
        }
    }
}
