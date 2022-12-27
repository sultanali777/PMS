using PMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Status> tbl_Status { get; set; }
        public DbSet<AddressAreas> tbl_Areas { get; set; }
        public DbSet<Building> tbl_Building { get; set; }
        public DbSet<Customer> tbl_Customer { get; set; }
        public DbSet<ExpenseDetails> tbl_ExpenseDetails { get; set; }
        public DbSet<Governorates> tbl_Governorates { get; set; }
        public DbSet<Property> tbl_Property { get; set; }
        public DbSet<PropertyType> tbl_PropertyType { get; set; }
        public DbSet<RentalDetails> tbl_RentalsDetails { get; set; }
        public DbSet<Vendor> tbl_Vendor { get; set; }
        public DbSet<VendorType> tbl_VendorType { get; set; }
        public DbSet<Country> tbl_Country { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }

}
