using System.Collections.Generic;

public interface ICloudinaryService {

    public Task<string> UploadImageAsync(string imageUrl);
    //string AddToCloudinary(string imageUrl);
    public Task<string> DeleteFromCloudinary(string imageUrl);
    string UploadImage(string? imageUrl); // METODO AGREGADO DEL UploadImage de ImageController.cs... Si no, no me dejaba compilar.
}
