using CarRentalSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarRentalSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for Cars, Customers, and Rentals
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Rental> Rentals { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Suppress unnecessary warnings
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Car entity for PricePerDay precision
            builder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasPrecision(18, 2);

            // Seed Admin Role
            const string ADMIN_ROLE_ID = "1";
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            // Seed Default Admin User
            const string ADMIN_USER_ID = "1";
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_USER_ID,
                UserName = "admin@carrental.com",
                NormalizedUserName = "ADMIN@CARRENTAL.COM",
                Email = "admin@carrental.com",
                NormalizedEmail = "ADMIN@CARRENTAL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin@123"),
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = "System",
                LastName = "Administrator",
                Role = "Admin"
            });

            // Assign Admin Role to Default Admin User
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_USER_ID
            });

            // Configure ApplicationUser fields
            builder.Entity<ApplicationUser>()
                .Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .Property(u => u.Role)
                .HasMaxLength(50)
                .IsRequired();

            // Configure Rentals relationships
            builder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Rental>()
                .HasOne(r => r.Car)
                .WithMany()
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
