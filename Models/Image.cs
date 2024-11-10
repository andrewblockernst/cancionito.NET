using System.Text.Json.Serialization;

public class Image {

    //THE INTERNAL ID OF THE IMAGE (PRIMARY KEY) 
    public int? InternalId { get; set; }

    //THE ID OF THE SONG THAT THE IMAGE BELONGS TO (FOREIGN KEY)
    public int? SongId { get; set; }    
    
    //THE URL OF THE IMAGE LOCATED IN THE SERVER (CLOUDINARY)
    public string? Url { get; set; }

    //THE SONG THAT THE IMAGE BELONGS TO (NAVIGATION PROPERTY) - NOT INCLUDED IN THE JSON RESPONSE
    [JsonIgnore]
    public Song Song { get; set; }
}
