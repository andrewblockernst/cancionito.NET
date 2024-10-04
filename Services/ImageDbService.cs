using Microsoft.EntityFrameworkCore;
public class ImageDbService : IImageService {
    private readonly CancionitoContext _context;
    public ImageDbService(CancionitoContext context) {
        _context = context;
    }
public Image Create(ImageDTO img) {
    // Verificar si la canción existe en la base de datos
    var song = _context.Songs.Find(img.SongId);
    if (song == null) {
        throw new Exception("Invalid SongId, song does not exist.");
    }
    // Crear la nueva imagen
    var NewImage = new Image(){
        InternalId = img.InternalId,
        SongId = img.SongId,
        Url = img.Url 
    };
    // Guardar la imagen en la base de datos
    _context.Images.Add(NewImage);
    _context.SaveChanges();

    return NewImage;
}
    public bool Delete(int id) {
        Image? img = _context.Images.Find(id);
        if (img is null) return false;
        _context.Images.Remove(img);
        _context.SaveChanges();
        return true;
    }
    public IEnumerable<Image> GetAll() {
        return _context.Images;
    }
    public Image? GetById(int idSong, int idInternal) {
        return _context.Images
        .SingleOrDefault(x => x.SongId == idSong && x.InternalId == idInternal);
    }
    public Image Update(int idInternal, int idSong, ImageDTO img) {
        var imageUpdate = _context.Images.FirstOrDefault(x => x.InternalId == idInternal && x.SongId == idSong);
        imageUpdate.InternalId = img.InternalId;
        imageUpdate.SongId = img.SongId;
        imageUpdate.Url = img.Url;
        
        _context.Entry(imageUpdate).State = EntityState.Modified;
        _context.SaveChanges();
        return imageUpdate;
    }

    
}