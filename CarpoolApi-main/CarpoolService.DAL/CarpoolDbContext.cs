using CarPoolService.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace CarPoolService.DAL;
public partial class CarpoolDbContext : DbContext
{
    public CarpoolDbContext()
    {
    }

    public CarpoolDbContext(DbContextOptions<CarpoolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<CarPoolRide> CarPoolRides { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=10.0.0.27;database=CarpoolDB;Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__73951ACDF512D655");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.DropLocationId).HasColumnName("DropLocationID");
            entity.Property(e => e.PassengerId).HasColumnName("PassengerID");
            entity.Property(e => e.PickupLocationId).HasColumnName("PickupLocationID");
            entity.Property(e => e.RideId).HasColumnName("RideID");
        });

        modelBuilder.Entity<CarPoolRide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarPoolR__C5B8C414591FCDD0");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.DepartureCityId).HasColumnName("DepartureCityID");
            entity.Property(e => e.DestinationCityId).HasColumnName("DestinationCityID");
            entity.Property(e => e.DriverId).HasColumnName("DriverID");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cities__F2D21A96D5F628C4");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__1788CCAC9A9A1D86");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534428E5288").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("Id");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
