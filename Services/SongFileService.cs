using System.Text.Json;

public class SongFileService : ISongsService {
    private readonly string _filePath = "Data/Songs.json";
    private readonly IFileStorageService _fileStorageService;

    public SongFileService(IFileStorageService fileStorageService) {
      _fileStorageService = fileStorageService;
    }
    public Song Create(Song s) {
        // Leer el contenido del archivo JSON
        var json = _fileStorageService.Read(_filePath);
        // Deserializar el JSON en una lista de songs
        var songs = JsonSerializer.Deserialize<List<Song>>(json) ?? new List<Song>();
        // Agregar el nuevo autor a la lista
        songs.Add(s);
        // Serializar la lista actualizada de vuelta a JSON
        json = JsonSerializer.Serialize(songs);
        // Escribir el JSON actualizado en el archivo
        _fileStorageService.Write(_filePath, json);
        return s;
    }

    public void Delete(int id) {
        // Leer el contenido del archivo JSON
        var json = _fileStorageService.Read(_filePath);
        // Deserializar el JSON en una lista de songs
        var songs = JsonSerializer.Deserialize<List<Song>>(json) ?? new List<Song>();
        // Buscar el autor por id
        var song = songs.Find(autor => autor.Id == id);

        // Si el autor existe, eliminarlo de la lista
        if (song is not null) {
            songs.Remove(song);
            // Serializar la lista actualizada de vuelta a JSON
            json = JsonSerializer.Serialize(songs);
            // Escribir el JSON actualizado en el archivo
            _fileStorageService.Write(_filePath, json);
        }
    }
    public IEnumerable<Song> GetAll() {
        //Leo el contenido del archivo
        var json = _fileStorageService.Read(_filePath);
        //Deserializo el Json en una lista de songs si es nulo retorna una lista vacia
        return JsonSerializer.Deserialize<List<Song>>(json) ?? new List<Song>();
    }
    public Song? GetById(int id) {
        //Leo el contenido del archivo
        var json = _fileStorageService.Read(_filePath);
        //Deserializo el Json en una lista de songs
        List<Song>? songs = JsonSerializer.Deserialize<List<Song>>(json);
        if(songs is null) return null;
        //Busco el autor por Id y devuelvo el autor encontrado
        return songs.Find(s => s.Id == id);  
    }

    public IEnumerable<Image> GetImages(int id) {
        throw new NotImplementedException();
    }
    public Song? Update(int id, Song song) {
         // Leer el contenido del archivo JSON
        var json = _fileStorageService.Read(_filePath);
        // Deserializar el JSON en una lista de songs
        var songs = JsonSerializer.Deserialize<List<Song>>(json) ?? new List<Song>();
        // Buscar el índice del autor por id
        var songIndex = songs.FindIndex(s => s.Id == id);

        // Si el autor existe, reemplazarlo en la lista
        if (songIndex >= 0) {
            //reeplazo el autor de la lista por el autor recibido por parametro con los nuevos datos
            songs[songIndex] = song;
            // Serializar la lista actualizada de vuelta a JSON
            json = JsonSerializer.Serialize(songs);
            // Escribir el JSON actualizado en el archivo
            _fileStorageService.Write(_filePath, json);
            return song;
        }
        // Retornar null si el autor no fue encontrado
        return null;
    }
}