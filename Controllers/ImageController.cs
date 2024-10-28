using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin,bot")] // Permite acceso a usuarios con el rol "admin" o "bot"
[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase {
    private readonly IImageService _imageService;
    public ImageController(IImageService imageService) {
      _imageService = imageService;
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
    public async Task<ActionResult<Image>> NewImage(ImageDTO img) {
    if (img == null || string.IsNullOrEmpty(img.Url)) {
        return BadRequest("La URL de la imagen no puede estar vacía.");
    }

    // Crear el nuevo registro de imagen con la URL generada por Cloudinary
    var newImage = new ImageDTO(){
        InternalId = img.InternalId,
        SongId = img.SongId,
        Url = img.Url
    };

    // Guardar la imagen en la base de datos
    Image _img = await _imageService.Create(newImage);

    // Retornar la respuesta con la URL de la imagen subida a Cloudinary
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
    public async Task<ActionResult> Delete(int InternalId, int SongId) {
      string deleted = await _imageService.Delete(InternalId, SongId);
      if (deleted == "Image not found") return NotFound();
      return Ok(deleted);
    }
}