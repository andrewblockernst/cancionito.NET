using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase {
    private readonly IImageService _imageService;
    private readonly ISongsService _songsService;
    public ImageController(IImageService imageService, ISongsService songService) {
      _imageService = imageService;
      _songsService = songService;
    }

    [HttpGet]
    public ActionResult<List<Image>> GetAll() {
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
    public ActionResult<Image> NuevoImage(Image img) {
      Image Image = _imageService.Create(img);
      return CreatedAtAction(nameof(GetById), new { id = Image.Id}, Image);
    }

    [HttpPut("{id}")]
    public ActionResult<Image> Update(int id, Image img) {
      try {
        Image Image = _imageService.Update(id, img);
        if ( Image is null ) return NotFound(new {Message = $"No se pudo actualizar el Image con ID: {id}"});
        return CreatedAtAction(nameof(GetById), new { id = Image.Id}, Image);
      }
      catch (System.Exception e) {
        Console.WriteLine(e.Message);
        return Problem(detail: e.Message, statusCode: 500);
      }
    }

    [HttpDelete]
    public ActionResult Delete(int id) {
      bool deleted = _imageService.Delete(id);
      if (deleted) return NoContent();
      return NotFound();
    }
}