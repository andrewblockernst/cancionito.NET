using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryService : ICloudinaryService{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(Cloudinary cloudinary) {
        _cloudinary = cloudinary;
    }

    public string AddToCloudinary(string imageUrl) {
        var uploadParams = new ImageUploadParams {
            File = new FileDescription(imageUrl) // Subir la imagen desde la URL proporcionada
        };

        var uploadResult = _cloudinary.Upload(uploadParams);

        // Retornar la nueva URL de la imagen alojada en Cloudinary
        return uploadResult?.SecureUrl?.ToString();
    }
    
    public string DeleteFromCloudinary(string imageUrl) {
    // Extraemos el public_id desde la URL usando nuestra función personalizada
    var publicId = ExtractPublicIdFromUrl(imageUrl);

    var deleteParams = new DeletionParams(publicId);

    // Ejecutamos la eliminación de la imagen de forma sincrónica
    var deleteResult = _cloudinary.Destroy(deleteParams);

    if (deleteResult.StatusCode == System.Net.HttpStatusCode.OK) {
        return "Imagen eliminada exitosamente de Cloudinary.";
    }

    throw new Exception("Error al eliminar la imagen de Cloudinary.");
    }

    public string UploadImage(string? url)
    {
        throw new NotImplementedException();
    }

    private string ExtractPublicIdFromUrl(string imageUrl) {
        // Parseamos la URL para obtener el public_id
        var uri = new Uri(imageUrl);
        var path = uri.AbsolutePath; // Obtenemos el path de la URL

        // Suponiendo que el `public_id` está entre el último '/' y antes de la extensión
        var lastSlashIndex = path.LastIndexOf('/');
        var publicIdWithExtension = path.Substring(lastSlashIndex + 1);

        // Removemos la extensión del archivo (.jpg, .png, etc.)
        var lastDotIndex = publicIdWithExtension.LastIndexOf('.');
        var publicId = publicIdWithExtension.Substring(0, lastDotIndex);

        return publicId;
    }

}
