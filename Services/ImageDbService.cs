using Microsoft.EntityFrameworkCore;
public class ImageDbService : IImageService {
    private readonly CancionitoContext _context;
    private readonly ICloudinaryService _cloudinaryService;
    public ImageDbService(CancionitoContext context, ICloudinaryService cloudinaryService) {
        _context = context;
        _cloudinaryService = cloudinaryService;
    }

    //CREATE AND SAVE A NEW IMAGE IN THE DATABASE AFTER UPLOADING IT TO Cloudinary
    public async Task<Image> Create(ImageDTO img) {
    //CHECK IF THE SONG EXISTS IN THE DATABASE
    var song = _context.Songs.Find(img.SongId);
    if (song == null) {
        throw new Exception("Invalid SongId, song does not exist.");
    }

    //UPLOAD THE IMAGE TO Cloudinary
    var new_url = await _cloudinaryService.UploadImageAsync(img.Url); 

    //VERIFIES IF THE URL IS NULL OR EMPTY
    if (string.IsNullOrEmpty(new_url)) {
        throw new Exception("Cloudinary URL is null or empty.");
    }

    //CREATE A NEW IMAGE OBJECT
    var NewImage = new Image(){
        InternalId = img.InternalId,
        SongId = img.SongId,
        Url = new_url 
    };
    
    //SAVE THE IMAGE IN THE DATABASE
    _context.Images.Add(NewImage);
    await _context.SaveChangesAsync();

    return NewImage;
    }

    //DELETE AN IMAGE BY ID AND INTERNAL ID FROM DATABASE AFTER DELETING IT FROM Cloudinary
    public async Task<string> Delete(int idInternal, int idSong) {
        Image? img = _context.Images.FirstOrDefault(x => x.InternalId == idInternal && x.SongId == idSong);
        if (img is null) return "Image not found";

        try {
            var response = await _cloudinaryService.DeleteFromCloudinaryAsync(img.Url);
            _context.Images.Remove(img);
            await _context.SaveChangesAsync();
            return response;
        } 
        catch (Exception ex) {
            //TRY AND CATCH BLOCK TO HANDLE EXCEPTIONS
            throw new Exception("Error deleting image: " + ex.Message);
    }
    }

    //GET ALL IMAGES FROM THE DATABASE 
    public IEnumerable<Image> GetAll() {
        return _context.Images;
    }

    //GET IMAGE BY ID AND INTERNAL ID FROM THE DATABASE 
    public Image? GetById(int idSong, int idInternal) {
        return _context.Images
        .SingleOrDefault(x => x.SongId == idSong && x.InternalId == idInternal);
    }

    //UPDATE AN IMAGE BY ID AND INTERNAL ID IN THE DATABASE 
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