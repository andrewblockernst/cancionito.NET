using System.Collections.Generic;

public interface ICloudinaryPeronService
{
    string AddToCloudinary(string imageUrl);
    string DeleteFromCloudinary(string imageUrl);
    string UploadImage(string? imageUrl); // METODO AGREGADO DEL UploadImage de ImageController.cs... Si no, no me dejaba compilar.
}