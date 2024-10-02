
using System.ComponentModel.DataAnnotations;

public class SongDTO {
    [Required(ErrorMessage = "El campo Titulo es requerido.")]
    public string? Title { get; set; }

}