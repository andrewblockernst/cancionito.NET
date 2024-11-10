using System.ComponentModel.DataAnnotations;

public class SongDTO {
    [Required(ErrorMessage = "The Title field is required.")]
    public string? Title { get; set; }

}

//DTO THAT RETURNS A LIST OF SONGS WITH ID AND TITLE IN A JSON RESPONSE
public class SongResponse {
    public string Message { get; set; }
    public List<SongDetailDTO> Songs { get; set; }
}

//SONG ATTRIBUTES RETURNED IN THE JSON RESPONSE
public class SongDetailDTO {
    public int Id { get; set; }
    public string Title { get; set; }
}
