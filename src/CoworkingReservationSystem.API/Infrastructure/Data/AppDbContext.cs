using Microsoft.EntityFrameworkCore;

public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Reservation> Reservations { get; set; }
    DbSet<Room> Rooms { get; set; }
    int SaveChanges();
}
public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User { Id = 1, Name = "Admin", Email = "admin@admin.com", Password = "hashed_password" });

        modelBuilder.Entity<Reservation>()
        .HasOne(r => r.User)
        .WithMany(u => u.Reservations)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(r => r.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}