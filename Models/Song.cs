using System.Text.Json.Serialization;

public class Song {

    //THE ID OF THE SONG (PRIMARY KEY)
    public int Id { get; set; }

    //THE TITLE OF THE SONG 
    public string? Title { get; set; }

    //LIST OF IMAGES THAT BELONG TO THE SONG (NAVIGATION PROPERTY) - NOT INCLUDED IN THE JSON RESPONSE
    [JsonIgnore]
    public virtual List<Image>? Images {get; set;}
}