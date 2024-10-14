using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryService : ICloudinaryService {
    private readonly Cloudinary _cloudinary;

    public CloudinaryService() {
        _cloudinary = new Cloudinary();
    }

    public async Task<string> UploadImageAsync(string imageUrl) {
        using (var handler = new HttpClientHandler()){
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
                var response = await client.GetAsync(imageUrl);

                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    // Guardar la imagen en un archivo temporal o memoria
                    var tempFilePath = Path.GetTempFileName();
                    await File.WriteAllBytesAsync(tempFilePath, imageBytes);

                    // Subir el archivo descargado a Cloudinary
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(tempFilePath),
                        Folder = "images",
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    
                    // Eliminar el archivo temporal después de la subida
                    File.Delete(tempFilePath);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return uploadResult.SecureUrl.ToString();
                    }
                    else
                    {
                        throw new Exception("Error al subir la imagen a Cloudinary: " + uploadResult.Error.Message);
                    }
                }
                else
                {
                    throw new Exception("Error al descargar la imagen de la URL proporcionada.");
                }
            }
        }
    }
    
    public async Task<string> DeleteFromCloudinaryAsync(string imageUrl) {
    // Extraemos el public_id desde la URL usando nuestra función personalizada
    var publicId = "images/" + ExtractPublicIdFromUrl(imageUrl);

    var deleteParams = new DeletionParams(publicId);

    // Ejecutamos la eliminación de la imagen de forma sincrónica
    var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

    if (deleteResult.Result == "ok") {
    return "Imagen eliminada exitosamente de Cloudinary.";
    }

    throw new Exception("Error al eliminar la imagen de Cloudinary.");
    }
    public string UploadImage(string? imageUrl) {
    var uploadParams = new ImageUploadParams() {
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
