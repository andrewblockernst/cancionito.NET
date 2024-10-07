using System.Collections.Generic;

public interface ICloudinaryService
{
    public Task<string> DeleteFromCloudinaryAsync(string Url);
    string UploadImage(string? imageUrl); // METODO AGREGADO DEL UploadImage de ImageController.cs... Si no, no me dejaba compilar.
    public Task<string> UploadImageAsync(string imageUrl);
}