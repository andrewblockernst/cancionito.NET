//INTERFACE FOR CloudinaryService, WHICH IS USED TO UPLOAD AND DELETE IMAGES FROM CLOUDINARY (CHECK IN THE PAGE OF... FOR PRESENTATION)

using System.Collections.Generic;

public interface ICloudinaryService
{
    //DELETES IMAGE FROM CLOUDINARY BY URL
    public Task<string> DeleteFromCloudinaryAsync(string Url);

    //UPLOADS IMAGE TO CLOUDINARY BY URL
    string UploadImage(string? imageUrl);

    //UPLOADS IMAGE TO CLOUDINARY BY URL ASYNC
    public Task<string> UploadImageAsync(string imageUrl);
}