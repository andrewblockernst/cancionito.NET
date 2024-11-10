//INTERFACE FOR SongService, WHICH IS USED TO MANAGE THE SONGS

public interface ISongService {

  //GET ALL SONGS
  public IEnumerable<Song> GetAll();

  //GET SONG BY ID
  public Song? GetById(int id);

  //CREATE A NEW SONG
  public Song Create(SongDTO s);

  //DELETE A SONG BY ID
  public void Delete(int id);

  //UPDATE A SONG BY ID
  public Song? Update(int id, Song s);

  //GET IMAGES OF A SONG
  public IEnumerable<Image> GetImages(int id);

  //LIST OF SONGS
  public int GetSongCount();
}