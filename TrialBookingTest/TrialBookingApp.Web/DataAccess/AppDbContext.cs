using Microsoft.EntityFrameworkCore;
using TrialBookingApp.Web.Domain.Entities;

namespace TrialBookingApp.Web.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Parent> Parents => Set<Parent>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<TrialClass> TrialClasses => Set<TrialClass>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<PaymentAttempt> PaymentAttempts => Set<PaymentAttempt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }
    }
}
