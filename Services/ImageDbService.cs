using Microsoft.EntityFrameworkCore;
public class ImageDbService : IImageService {
    private readonly CancionitoContext _context;
    public ImageDbService(CancionitoContext context) {
        _context = context;
    }
    public Image Create(Image img) {
        Image NewImage = new Image(img.InternalId, img.SongId, img.Url); //REVISAR CON EL MODELO DE IMAGEN (CONSTRUCTOR, ETC)
        _context.Add(NewImage);
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
        return _context.Images.Include(el => el.Song);
    }
    public Image? GetById(int id) {
        return _context.Images.Find(id);
    }
    public Image Update(int idInternal, int idSong, Image img) {
        var imageUpdate = _context.Images.FirstOrDefault(x => x.InternalId == idInternal && x.SongId == idSong);
        imageUpdate.InternalId = img.InternalId;
        imageUpdate.SongId = img.SongId;
        imageUpdate.Url = img.Url;
        
        _context.Entry(imageUpdate).State = EntityState.Modified;
        _context.SaveChanges();
        return imageUpdate;
    }
}