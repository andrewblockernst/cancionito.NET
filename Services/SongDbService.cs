using Microsoft.EntityFrameworkCore;

public class SongDbService : ISongService {
  private readonly CancionitoContext _context;
  public SongDbService(CancionitoContext context) {
    _context = context;
  }

    //CREATE A NEW SONG IN THE DATABASE AND RETURN IT IN A JSON RESPONSE
    public Song Create(SongDTO s) {
    try {
        //NEW SONG OBJECT AND GO TO THE DTO FOR THE TITLE
        var newSong = new Song {
            Title = s.Title
        };

        //ADD TO CONTEXT AND SAVE CHANGES
        _context.Songs.Add(newSong);
        _context.SaveChanges();
        
        //RETURN THE SONG IN AN AUTOINCREMENTAL WAY
        return newSong;
    } catch (Exception ex) {
        throw new Exception("Error saving song to database: " + ex.Message);
        }
    }

    //DELETE A SONG BY ID AND REMOVE THE IMAGES RELATED TO IT FROM THE DATABASE 
    public void Delete(int id) {
    Song? s = _context.Songs.Find(id);
    if (s is not null)
    {
        _context.Images
            .Where(x => x.SongId == id)
            .ToList()
            .ForEach(x => _context.Images.Remove(x));
        _context.Songs.Remove(s);
        _context.SaveChanges();
        }
    }

    //GET ALL SONGS FROM THE DATABASE 
    public IEnumerable<Song> GetAll() {
        return _context.Songs; 
    }

    //GET A SONG BY ID FROM THE DATABASE
    public Song? GetById(int id) {
        return _context.Songs.Find(id);
    }

    //UPDATE A SONG BY ID IN THE DATABASE AND RETURN IT IN A JSON RESPONSE 
    public Song? Update(int id, Song s) {
        _context.Entry(s).State = EntityState.Modified;
        _context.SaveChanges();
        return s;
    }

    //GET IMAGES OF A SONG FROM THE DATABASE 
    public IEnumerable<Image> GetImages(int id) {
        Song s = _context.Songs
        .Include(song => song.Images)  //INCLUDES RELATED IMAGES
        .FirstOrDefault(x => x.Id == id);
        return s?.Images ??  new List<Image>();
    }

    //GET THE SONG COUNT FROM THE DATABASE 
    public int GetSongCount() {
    return _context.Songs.Count();
    }
}
