public interface IImageService {
  public IEnumerable<Image> GetAll();
  public Image? GetById(int idSong, int idInternal);
  public Task<Image> Create(ImageDTO img);
  public Task<string> Delete(int idInternal, int idSong);
  public Image Update(int idInternal, int idSong, ImageDTO img);
  
}