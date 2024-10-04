using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryPeronService : ICloudinaryPeronService {
    private readonly Cloudinary _cloudinary;

    public CloudinaryPeronService(Cloudinary cloudinary) {
        _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));;
    }

    public string AddToCloudinary(string imageUrl) {
        if (_cloudinary == null) {
            throw new InvalidOperationException("La instancia de Cloudinary no ha sido inicializada.");
        }

        var uploadParams = new ImageUploadParams {
            File = new FileDescription(imageUrl), // Subir la imagen desde la URL proporcionada
            PublicId = "arroz chaufa",
            QualityAnalysis = false,
            Colors = false,        
            Categorization = "google_tagging"
        };

        var uploadResult = _cloudinary.Upload(uploadParams);
        // Retornar la nueva URL de la imagen alojada en Cloudinary
        return uploadResult?.SecureUrl?.ToString() ?? throw new InvalidOperationException("El resultado de la carga de imagen es nulo.");;
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
    public string UploadImage(string? imageUrl) {
    var uploadParams = new ImageUploadParams()
    {
        File = new FileDescription(imageUrl)
    };

    var uploadResult = _cloudinary.Upload(uploadParams);

    return uploadResult.PublicId;
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
