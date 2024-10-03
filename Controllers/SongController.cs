using Microsoft.AspNetCore.Mvc;
    
[ApiController]
[Route("api/songs")]
public class SongsController : ControllerBase {

  private readonly ISongService _songsService;

  public SongsController(ISongService songsService) {
    _songsService = songsService;
  }
 [HttpGet]
  public ActionResult<List<Song>> GetAllSongs(){

    return Ok(_songsService.GetAll());
  }
    
  [HttpGet("{idSong}/images")]
  public ActionResult<List<Image>> GetImages(int idSong) {
    var img = _songsService.GetImages(idSong);
    return Ok(img);
  }

  [HttpGet("{id}")]
  public ActionResult<Song> GetById(int id) {
  Song? s = _songsService.GetById(id);
  if(s == null) {
    return NotFound("Canción no encontrada");
  }
    return Ok(s);
  }

  [HttpPost]
  public ActionResult<Song> NewSong(Song s) {
    Song _s = _songsService.Create(s);
    //Devuelvo el resultado de llamar al metodo GetById pasando como parametro el Id del nuevo autor
    return CreatedAtAction(nameof(GetById), new {id = _s.Id}, _s);
  }

  [HttpDelete("{id}")]
  public ActionResult Delete(int id) {
    var s = _songsService.GetById(id);
    if (s == null) { 
      return NotFound("Canción no encontrada");}
    _songsService.Delete(id);
    return NoContent();
  }
  
  [HttpPut("{id}")]
  public ActionResult<Song> UpdateSong(int id, Song newSong) {
    // Asegurarse de que el ID del song en la solicitud coincida con el ID del parámetro
    if (id != newSong.Id) {
      return BadRequest("El ID de la canción no coincide con el ID del parámetro"); // Retorna 400 Bad Request
    }
    var song = _songsService.Update(id, newSong);

    if (song is null) {
      return NotFound(); // Si no se encontró la song, retorna 404 Not Found
    }
     return CreatedAtAction(nameof(GetById), new {id = song.Id}, song); // Retorna el recurso actualizado
  }

}

