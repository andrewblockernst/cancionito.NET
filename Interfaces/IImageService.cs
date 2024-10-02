public interface IImageService {
  public IEnumerable<Image> GetAll();
  public Image? GetById(int id);
  public Image Create(Image img);
  public bool Delete(int id);
  public Image Update(int idInternal, int idSong, Image img);
}