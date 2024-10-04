using System.Collections.Generic;

public interface ICloudinaryService
{
    string AddToCloudinary(string imageUrl);
    string DeleteFromCloudinary(string imageUrl);
    string UploadImage(string? url); // METODO AGREGADO DEL UploadImage de ImageController.cs... Si no, no me dejaba compilar.
}
