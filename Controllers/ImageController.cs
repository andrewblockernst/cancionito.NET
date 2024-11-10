using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin,bot")] //ALLOW ACCESS TO USERS WITH THE ROLE "admin" OR "bot" IN DIFFERENT ACTIONS
[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase {
    private readonly IImageService _imageService;

    //CONSTRUCTOR THAT INYECTS THE CONTENXT OF THE ImageService
    public ImageController(IImageService imageService) {
      _imageService = imageService;
    }

    //GET ALL IMAGES
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

    //GET IMAGE BY ID
    [HttpGet("{idSong}/{idInternal}")]
    public ActionResult<Image> GetById(int idSong, int idInternal) {
      var Image = _imageService.GetById(idSong, idInternal);
      if ( Image is null ) return NotFound();
      return Ok(Image);
    }

    //CREATE A NEW IMAGE
    [HttpPost]
    public async Task<ActionResult<Image>> NewImage(ImageDTO img) {
    if (img == null || string.IsNullOrEmpty(img.Url)) {
        return BadRequest("The image URL cannot be empty.");
    }

    //GENERATES A NEW REGISTER WITH THE DATA OF THE IMAGE FROM CLOUDINARY
    var newImage = new ImageDTO(){
        InternalId = img.InternalId,
        SongId = img.SongId,
        Url = img.Url
    };

    //CREATE THE IMAGE IN THE DATABASE
    Image _img = await _imageService.Create(newImage);

    //IT RETURNS WITH THE URL OF THE IMAGE CREATED IN THE DATABASE FROM CLOUDINARY
    return CreatedAtAction(nameof(GetById), new { idSong = _img.SongId, idInternal = _img.InternalId}, _img);
    }

    //UPDATE AN IMAGE BY ID AND INTERNAL ID 
    [HttpPut("{idSong}/{idInternal}")]
    public ActionResult<Image> Update(int idSong, int idInternal, ImageDTO img) {

      //VALIDATES THE MODEL OF THE IMAGE
      try {
        Image _img = _imageService.Update(idInternal, idSong, img);
        if ( _img is null ) return NotFound(new {Message = $"Could not update Image with IDs: {idSong} and {idInternal}."});
        return CreatedAtAction(nameof(GetById), new { idSong = _img.SongId, idInternal = _img.InternalId}, _img);;
      }
      
      //IN CASE OF AN ERROR IN THE UPDATE OF THE IMAGE - INTERNAL SERVER ERROR (500)
      catch (System.Exception e) {
        Console.WriteLine(e.Message);
        return Problem(detail: e.Message, statusCode: 500);
      }
    }

    //DELETE AN IMAGE BY ID AND INTERNAL ID
    [HttpDelete]
    public async Task<ActionResult> Delete(int InternalId, int SongId) {
      string deleted = await _imageService.Delete(InternalId, SongId);
      if (deleted == "Image not found.") return NotFound();
      return NoContent();
    }
}