using System.Text.Json;

public class ImageFileService : IImageService {
    private readonly IFileStorageService _fileStorageService;
    private readonly ISongService _songsService;
    private readonly string _filePath = "Data/Images.json";

    public ImageFileService(IFileStorageService fileStorageService, ISongService songsService) {
      _fileStorageService = fileStorageService;
      _songsService = songsService;
    }
    public Image Create(Image img) {
        throw new NotImplementedException();
    }
    public bool Delete(int id) {
      List<Image> images = (List<Image>)GetAll();
      Image? imageToDelete =  images.Find(img => img.Id == id.ToString()); // Busca el libro por id y lo retorna, si no lo encuentra retorna null.
      
      if( imageToDelete is null ) return false;
      
      bool deleted = images.Remove(imageToDelete) ;
      if ( deleted ) {
        var json = JsonSerializer.Serialize(images);
        _fileStorageService.Write(_filePath , json);
      }
      return deleted;
    }
    public IEnumerable<Image> GetAll() {
      var json = _fileStorageService.Read(_filePath);
      return JsonSerializer.Deserialize<List<Image>>(json) ?? new List<Image>();
    }
    public Image? GetById(int id) {      
      List<Image> images = (List<Image>)GetAll();
      return images.Find(img => img.Id == id.ToString()); // Busca el libro por id y lo retorna, si no lo encuentra retorna null. 
    }
    public Boolean Update(int id, Image img) {
      List<Image> images = (List<Image>)GetAll();
      int index = images.FindIndex(img => img.Id == id.ToString()); //Busca el indice del id que se quiere actualizar, si no lo encuentra devuelve -1. 
      //No se encontró el id que se quiere actualizar
      if ( index == -1 ) return false;
      
      images[index] = img;
      _fileStorageService.Write(_filePath, JsonSerializer.Serialize(images));
      return true;
    }
    Image IImageService.Update(int idSong, int idInternal, Image img)
    {
        throw new NotImplementedException();
    }
}