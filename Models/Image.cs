public class Image {
    public int? InternalId { get; set; }
    public int? SongId { get; set; }    
    public string? Url { get; set; }
    public Song Song { get; set; }
    public string Id { get; internal set; }
}
