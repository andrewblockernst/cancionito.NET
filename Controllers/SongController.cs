using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin,bot")] //ALLOW ACCESS TO USERS WITH THE ROLE "admin" OR "bot" IN DIFFERENT ACTIONS
[ApiController]
[Route("api/songs")]
public class SongsController : ControllerBase {

  private readonly ISongService _songsService;

  //CONSTRUCTOR THAT INYECTS THE CONTENXT OF THE SongService
  public SongsController(ISongService songsService) {
    _songsService = songsService;
  }

  //GET ALL SONGS
  [HttpGet]
  public ActionResult<List<Song>> GetAllSongs(){

    return Ok(_songsService.GetAll());
  }
    
  //GET ALL IMAGES OF A SONG
  [HttpGet("{idSong}/images")]
  public ActionResult<List<Image>> GetImages(int idSong) {
    var img = _songsService.GetImages(idSong);
    return Ok(img);
  }

  //GET SONG BY ID
  [HttpGet("{id}")]
  public ActionResult<Song> GetById(int id) {
  Song? s = _songsService.GetById(id);
  if(s == null) {
    return NotFound("Song not found.");
  }
    return Ok(s);
  }

  //GET SONG COUNT AND LIST
  [HttpGet("count")]
  public IActionResult GetSongCountAndList() {
      int count = _songsService.GetSongCount();
      string message = $"There are {count} songs uploaded to 'cancionitodb' (database).";
      
      //SONGS LIST WITH ID AND TITLE
      var songList = _songsService.GetAll()
          .Select(s => new SongDetailDTO { Id = s.Id, Title = s.Title })
          .ToList();

      //RETURNS THE OBJECT WITH THE MESSAGE AND THE LIST OF SONGS
      var response = new SongResponse {
          Message = message,
          Songs = songList
      };

      return Ok(response);
  }

  //CREATE A NEW SONG
  [HttpPost]
  public ActionResult<Song> NewSong(SongDTO s) {

      //CHECK IF THE MODEL IS VALID
      if (!ModelState.IsValid) {
          return BadRequest(ModelState); 
      }
      try {
          Song newSong = _songsService.Create(s);
          return CreatedAtAction(nameof(GetById), new { id = newSong.Id }, newSong);
      } catch (Exception ex) {
          return StatusCode(500, "Error creating song:" + ex.Message);
      }
  }

  //DELETE A SONG BY ID
  [HttpDelete("{id}")]
  public ActionResult Delete(int id) {
    var s = _songsService.GetById(id);
    if (s == null) { 
      return NotFound("Song not found.");}
    _songsService.Delete(id);
    return NoContent();
  }

  //UPDATE A SONG BY ID  
  [HttpPut("{id}")]
  public ActionResult<Song> UpdateSong(int id, Song newSong) {
    // MAKE SURE THE SONG IN THE REQUEST MATCHES THE PARAMETER ID
    if (id != newSong.Id) {
      return BadRequest("The song ID does not match the parameter ID."); //...400 BAD REQUEST
    }
    var song = _songsService.Update(id, newSong);

    if (song is null) {
      return NotFound(); //IF THE SONG IS NOT FOUND, ...404 NOT FOUND
    }
     return CreatedAtAction(nameof(GetById), new {id = song.Id}, song); //...201 (UPDATED)
  }
}

