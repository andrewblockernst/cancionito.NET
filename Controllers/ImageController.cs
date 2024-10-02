using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase {
    private readonly IImageService _imageService;
    private readonly ISongService _songsService;
    public ImageController(IImageService imageService, ISongService songService) {
      _imageService = imageService;
      _songsService = songService;
    }

    [HttpGet]
    public ActionResult<List<Image>> GetAllImages() {
      try {
        return Ok(_imageService.GetAll());
      }
      catch (System.Exception e) {
        Console.WriteLine(e.Message);
        return Problem(detail: e.Message, statusCode: 500);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Image> GetById(int id) {
      Image? Image = _imageService.GetById(id);
      if ( Image is null ) return NotFound();
      return Ok(Image);
    }

    [HttpPost]
    public ActionResult<Image> NewImage(Image img) {
      Image Image = _imageService.Create(img);
      return img;
    }

    [HttpPut("{SongId}/{InternalId}")]
    public ActionResult<Image> Update(int idSong, int idInternal, Image img) {
      try {
        Image Image = _imageService.Update(idInternal, idSong, img);
        if ( Image is null ) return NotFound(new {Message = $"No se pudo actualizar el Image con los ID: {idSong} y {idInternal}"});
        return img;
      }
      catch (System.Exception e) {
        Console.WriteLine(e.Message);
        return img;
      }
    }

    [HttpDelete]
    public ActionResult Delete(int id) {
      bool deleted = _imageService.Delete(id);
      if (deleted) return NoContent();
      return NotFound();
    }
}