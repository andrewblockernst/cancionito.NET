using Microsoft.EntityFrameworkCore;
public class ImageDbService : IImageService {
    private readonly CancionitoContext _context;
    private readonly ICloudinaryService _cloudinaryService;
    public ImageDbService(CancionitoContext context, ICloudinaryService cloudinaryService) {
        _context = context;
        _cloudinaryService = cloudinaryService;
    }
    public async Task<Image> Create(ImageDTO img) {
    // Verificar si la canción existe en la base de datos
    var song = _context.Songs.Find(img.SongId);
    if (song == null) {
        throw new Exception("Invalid SongId, song does not exist.");
    }

    // Subir la imagen a Cloudinary
    var new_url = await _cloudinaryService.UploadImageAsync(img.Url); //

    // Verifica si la URL no es nula antes de guardar
    if (string.IsNullOrEmpty(new_url)) {
        throw new Exception("La URL de Cloudinary es nula o vacía.");
    }

    // Crear la nueva imagen
    var NewImage = new Image(){
        InternalId = img.InternalId,
        SongId = img.SongId,
        Url = new_url 
    };
    
    // Guardar la imagen en la base de datos
    _context.Images.Add(NewImage);
    await _context.SaveChangesAsync();

    return NewImage;
    }
    public async Task<string> Delete(int idInternal, int idSong) {
        Image? img = _context.Images.FirstOrDefault(x => x.InternalId == idInternal && x.SongId == idSong);
        if (img is null) return "Image not found";

        try {
            var response = await _cloudinaryService.DeleteFromCloudinaryAsync(img.Url);
            _context.Images.Remove(img);
            await _context.SaveChangesAsync();
            return response;
        } catch (Exception ex) {
            // Manejo de errores y logging si es necesario
            throw new Exception("Error al eliminar la imagen: " + ex.Message);
    }
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