using Microsoft.EntityFrameworkCore;

//DATABASE CONTEXT FOR THE SONGS AND THEIR IMAGES
public class CancionitoContext:DbContext {
  public DbSet<Song> Songs {get; set;} //GET AND SET OF SONGS
  public DbSet<Image> Images {get; set;} //GET AND SET OF IMAGES

    //CALLING THE BASE CONSTRUCTOR OF THE DBCONTEXT 
    public CancionitoContext(DbContextOptions<CancionitoContext> options) : base(options) {
    }

    //CREATION OF THE TABLES AND THEIR RELATIONSHIPS (ENTITIES)
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Song>(entity => {
        entity.ToTable("Songs"); //MAKES SURE THAT THE TABLE IS CALLED SONGS 
        entity.Property(t => t.Title).IsRequired().HasMaxLength(100);
        });

        //CREATION OF THE IMAGES TABLE AND ITS RELATIONSHIP WITH THE SONGS TABLE (ONMODEL OF THE COMPOSITE KEY AND THEIR RELATIONSHIPS) 
        modelBuilder.Entity<Image>(entity => {
        entity.HasKey(image => new { image.SongId, image.InternalId });
        entity.Property(i => i.InternalId)
            .HasColumnName("id_internal")
            .IsRequired();

        entity.Property(s => s.SongId)
            .HasColumnName("id_song")
            .IsRequired();

        entity.Property(u => u.Url).IsRequired();

        entity.HasOne(s => s.Song)
            .WithMany(i => i.Images)
            .HasForeignKey(s => s.SongId)
            .IsRequired();
        });      

    }

} 