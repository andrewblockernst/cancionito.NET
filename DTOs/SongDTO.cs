using System.ComponentModel.DataAnnotations;

public class SongDTO {
    [Required(ErrorMessage = "El campo Titulo es requerido.")]
    public string? Title { get; set; }

}

//DTO QUE DEVUELVE UNA LISTA DE CANCIONES CON ID Y TITULO EN UNA RESPUESTA JSON
public class SongResponse {
    public string Message { get; set; }
    public List<SongDetailDTO> Songs { get; set; }
}

//ATRIBUTOS DE LA CANCION QUE SE DEVUELVEN EN LA RESPUESTA JSON
public class SongDetailDTO {
    public int Id { get; set; }
    public string Title { get; set; }
}
