using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin,bot")] // Permite acceso a usuarios con el rol "admin" o "bot"
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

  [HttpGet("count")]
  public IActionResult GetSongCountAndList() {
      int count = _songsService.GetSongCount();
      string message = $"Hay {count} canciones subidas a 'cancionitodb' (base de datos).";
      
      //LISTA DE CANCIONES CON ID Y TITULO
      var songList = _songsService.GetAll()
          .Select(s => new SongDetailDTO { Id = s.Id, Title = s.Title })
          .ToList();

      //OBJETO DE RESPUESTA
      var response = new SongResponse {
          Message = message,
          Songs = songList
      };

      return Ok(response);
  }


  [HttpPost]
  public ActionResult<Song> NewSong(SongDTO s) {
      //VALIDA MODELO DE DATOS
      if (!ModelState.IsValid) {
          return BadRequest(ModelState); 
      }
      try {
          Song newSong = _songsService.Create(s);
          return CreatedAtAction(nameof(GetById), new { id = newSong.Id }, newSong);
      } catch (Exception ex) {
          return StatusCode(500, "Error al crear la canción: " + ex.Message);
      }
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

