public interface IImageService {
  public IEnumerable<Image> GetAll();
  public Image? GetById(int idSong, int idInternal);
  public Image Create(ImageDTO img);
  public bool Delete(int id);
  public Image Update(int idInternal, int idSong, ImageDTO img);
}