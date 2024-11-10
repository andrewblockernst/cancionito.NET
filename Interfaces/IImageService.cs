//INTERFACE FOR ImageService, WHICH IS USED TO MANAGE THE IMAGES OF THE SONGS

public interface IImageService {

  //GET ALL IMAGES
  public IEnumerable<Image> GetAll();

  //GET IMAGE BY ID
  public Image? GetById(int idSong, int idInternal);

  //CREATE A NEW IMAGE
  public Task<Image> Create(ImageDTO img);

  //DELETE AN IMAGE BY ID AND INTERNAL ID
  public Task<string> Delete(int idInternal, int idSong);

  //UPDATE AN IMAGE BY ID AND INTERNAL ID
  public Image Update(int idInternal, int idSong, ImageDTO img);
  
}