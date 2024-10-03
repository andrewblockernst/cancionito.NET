public interface ISongService {
  public IEnumerable<Song> GetAll();
  public Song? GetById(int id);
  public Song Create(Song s);
  public void Delete(int id);
  public Song? Update(int id, Song s);
  public IEnumerable<Image> GetImages(int id);
}