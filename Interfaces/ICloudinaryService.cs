using System.Collections.Generic;

public interface ICloudinaryService
{
    string AddToCloudinary(string imageUrl);
    void DeleteFromCloudinary(string imageUrl);
    void UploadImage(string? url); // METODO AGREGADO DEL UploadImage de ImageController.cs... Si no, no me dejaba compilar.
}
