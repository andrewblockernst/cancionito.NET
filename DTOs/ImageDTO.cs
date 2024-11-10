using System.ComponentModel.DataAnnotations;

public class ImageDTO
{
    [Required(ErrorMessage = "The internal ID field is required.")]
    public int? InternalId { get; set; }
    [Required(ErrorMessage = "The song ID field is required.")]
    public int? SongId { get; set; }
    [Required(ErrorMessage = "The song URL field is required.")]
    public string? Url { get; set; }
}