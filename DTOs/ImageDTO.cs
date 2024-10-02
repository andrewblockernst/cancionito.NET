using System.ComponentModel.DataAnnotations;

public class ImageDTO
{
    [Required(ErrorMessage = "El campo de Id interna es requerido.")]
    public int? InternalId { get; set; }
    [Required(ErrorMessage = "El campo de Id de la canción es requerido.")]
    public int? SongId { get; set; }
    [Required(ErrorMessage = "El campo de la url de la canción es requerido.")]
    public string? Url { get; set; }
}