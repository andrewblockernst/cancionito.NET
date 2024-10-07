using System.Text.Json.Serialization;
public class Song {
    public int Id { get; set; }
    public string? Title { get; set; }

    [JsonIgnore]
    public virtual List<Image>? Images {get; set;}

}