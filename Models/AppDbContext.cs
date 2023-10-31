using Microsoft.EntityFrameworkCore;

namespace project.Models;

public class AppDbContext : DbContext {

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }


    public DbSet<Player> Players {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Wishlist> Wishlists {get; set;}
}