using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase {
    private readonly IImageService _imageService;
    private readonly ICloudinaryService _cloudinaryService;
    public ImageController(IImageService imageService, ICloudinaryService cloudinaryService) {
      _imageService = imageService;
      _cloudinaryService = cloudinaryService;
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

    [HttpGet("{idSong}/{idInternal}")]
    public ActionResult<Image> GetById(int idSong, int idInternal) {
      var Image = _imageService.GetById(idSong, idInternal);
      if ( Image is null ) return NotFound();
      return Ok(Image);
    }

    [HttpPost]
    public ActionResult<Image> NewImage(ImageDTO img) {
      _cloudinaryService.UploadImage(img.Url);
      Image _img = _imageService.Create(img);
      return CreatedAtAction(nameof(GetById), new { idSong = _img.SongId, idInternal = _img.InternalId}, _img);
    }

    [HttpPut("{idSong}/{idInternal}")]
    public ActionResult<Image> Update(int idSong, int idInternal, ImageDTO img) {
      try {
        Image _img = _imageService.Update(idInternal, idSong, img);
        if ( _img is null ) return NotFound(new {Message = $"No se pudo actualizar el Image con los ID: {idSong} y {idInternal}"});
        return CreatedAtAction(nameof(GetById), new { idSong = _img.SongId, idInternal = _img.InternalId}, _img);;
      }
      catch (System.Exception e) {
        Console.WriteLine(e.Message);
        return Problem(detail: e.Message, statusCode: 500);
      }
    }

    [HttpDelete]
    public ActionResult Delete(int InternalId, int SongId) {
      bool deleted = _imageService.Delete(InternalId, SongId);
      if (deleted) return NoContent();
      return NotFound();
    }
}