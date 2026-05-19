using DAL.EF.Tables;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class AabaasBdContext : DbContext
    {
        public AabaasBdContext(DbContextOptions<AabaasBdContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<HotPackage> HotPackages { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Pending");
                entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Hotel).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<HotPackage>(entity =>
            {
                entity.HasKey(e => e.PackageId);

                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.Hotel).WithMany(p => p.HotPackages)
                    .HasForeignKey(d => d.HotelId);
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.HasKey(e => e.HotelId);

                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Name).HasMaxLength(150);
                entity.Property(e => e.PricePerNight).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.RoomType).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.UserPassword).HasMaxLength(256);
            });
        }
    }
}
