using Microsoft.EntityFrameworkCore;

public class CancionitoContext:DbContext {
  public DbSet<Song> Songs {get; set;}
  public DbSet<Image> Images {get; set;}
    public CancionitoContext(DbContextOptions<CancionitoContext> options) : base(options) {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Song>(entity => {
          entity.Property(t => t.Title).IsRequired().HasMaxLength(100);
        }
        );
        modelBuilder.Entity<Image>(entity =>
        {
          entity.Property(i => i.InternalId).IsRequired();
          entity.Property(s => s.SongId).IsRequired();
          entity.Property(u => u.Url).IsRequired();

           entity.HasOne(s => s.Song)
           .WithMany(i => i.Images)
           .HasForeignKey(s => s.SongId).IsRequired();
        }       
        );

    }

} 