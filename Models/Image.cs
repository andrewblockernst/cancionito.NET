using System.Text.Json.Serialization;

public class Image {
    public int? InternalId { get; set; }
    public int? SongId { get; set; }    
    public string? Url { get; set; }
    [JsonIgnore]
    public Song Song { get; set; }
}
