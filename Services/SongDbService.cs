using Microsoft.EntityFrameworkCore;

public class SongDbService : ISongService {
  private readonly CancionitoContext _context;
  public SongDbService(CancionitoContext context) {
    _context = context;
  }
    public Song Create(SongDTO s) {
    try {
        //NUEVO OBJETO SONG Y PASA AL DTO POR EL TITULO
        var newSong = new Song {
            Title = s.Title
        };

        //AGREGA AL CONTEXT Y GUARDA LOS CAMBIOS
        _context.Songs.Add(newSong);
        _context.SaveChanges();
        
        //DEVUELVE LA CANCION DE MANERA AUTOINCREMENTAL
        return newSong;
    } catch (Exception ex) {
        throw new Exception("Error al guardar la canción en la base de datos: " + ex.Message);
    }
    }

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
    public IEnumerable<Song> GetAll() {
        return _context.Songs; 
    }
    public Song? GetById(int id) {
        return _context.Songs.Find(id);
    }
    public Song? Update(int id, Song s) {
        _context.Entry(s).State = EntityState.Modified;
        _context.SaveChanges();
        return s;
    }
    public IEnumerable<Image> GetImages(int id) {
        Song s = _context.Songs
        .Include(song => song.Images)  // Incluye las imágenes relacionadas
        .FirstOrDefault(x => x.Id == id);
        return s?.Images ??  new List<Image>();
    }
    public int GetSongCount() {
    return _context.Songs.Count();
    }
}
