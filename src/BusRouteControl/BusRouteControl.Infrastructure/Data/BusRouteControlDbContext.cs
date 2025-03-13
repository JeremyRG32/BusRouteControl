using BusRouteControl.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Data;

public partial class BusRouteControlDbContext : DbContext
{
    public BusRouteControlDbContext()
    {
    }

    public BusRouteControlDbContext(DbContextOptions<BusRouteControlDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Routes> Routes { get; set; }

    public virtual DbSet<Schedules> Schedules { get; set; }

    public virtual DbSet<Tickets> Tickets { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("StrConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Routes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Routes__3214EC07D09BDA28");

            entity.Property(e => e.Destination).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Origin).HasMaxLength(100);
        });

        modelBuilder.Entity<Schedules>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC075FB0C618");

            entity.Property(e => e.DepartureTime).HasColumnType("datetime");

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("FK__Schedules__Route__4F7CD00D");
        });

        modelBuilder.Entity<Tickets>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tickets__3214EC07CA46B105");

            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Reserved");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Tickets__Schedul__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Tickets__UserId__5535A963");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07355C7797");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053436B4DFAE").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
