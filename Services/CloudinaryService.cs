using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryService : ICloudinaryService {
    private readonly Cloudinary _cloudinary;

    //CONSTRUCTOR TO INITIALIZE THE CloudinaryService
    public CloudinaryService() {
        _cloudinary = new Cloudinary();
    }

    //METHOD TO UPLOAD IMAGE TO CLOUDINARY BY URL ASYNC
    public async Task<string> UploadImageAsync(string imageUrl) {
        using (var handler = new HttpClientHandler()) {
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
                var response = await client.GetAsync(imageUrl);

                //DOWNLOAD AND SAVE THE IMAGE IN A TEMPORARY FILE OR MEMORY TO UPLOAD IT TO Cloudinary
                if (response.IsSuccessStatusCode) {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    //SAVE THE IMAGE IN A TEMPORARY FILE OR MEMORY
                    var tempFilePath = Path.GetTempFileName();
                    await File.WriteAllBytesAsync(tempFilePath, imageBytes);

                    //UPLOAD THE DOWNLOADED FILE TO Cloudinary
                    var uploadParams = new ImageUploadParams() {
                        File = new FileDescription(tempFilePath),
                        Folder = "images",
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    
                    //DELETE THE TEMPORARY FILE AFTER UPLOADING IT TO Cloudinary
                    File.Delete(tempFilePath);

                    if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK) {
                        return uploadResult.SecureUrl.ToString();
                    }
                    else {
                        throw new Exception("Error uploading image to Cloudinary: " + uploadResult.Error.Message);
                    }
                }
                else {
                    throw new Exception("Error downloading image from the provided URL.");
                }
            }
        }
    }
    
    //METHOD TO DELETE IMAGE FROM CLOUDINARY BY URL ASYNC
    public async Task<string> DeleteFromCloudinaryAsync(string imageUrl) {

    //EXTRACT THE public_id FROM THE IMAGE URL USING OUR CUSTOM METHOD (ExtractPublicIdFromUrl) 
    var publicId = "images/" + ExtractPublicIdFromUrl(imageUrl);

    var deleteParams = new DeletionParams(publicId);

    //IMAGE DELETION IS ASYNCHRONOUS AND RETURNS A RESULT OBJECT
    var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

    if (deleteResult.Result == "ok") {
    return "Image successfully removed from Cloudinary.";
    }

    throw new Exception("Error deleting Cloudinary image.");
    }

    //METHOD TO UPLOAD IMAGE TO CLOUDINARY BY URL (NOT ASYNC) BY LOCAL FILE PATH 
    public string UploadImage(string? imageUrl) {
    var uploadParams = new ImageUploadParams() {
        File = new FileDescription(imageUrl)
    };

    var uploadResult = _cloudinary.Upload(uploadParams);

    return uploadResult.PublicId;
    }

    private string ExtractPublicIdFromUrl(string imageUrl) {
        //THE URL IS CONVERTED TO OBTAIN THE PUBLIC ID OF THE IMAGE
        var uri = new Uri(imageUrl);
        var path = uri.AbsolutePath; //URL PATH

        //ASSUMMING THE 'PUBLIC_ID' IS BETWEEN THE LAST TWO SLASHES ('/') AND BEFORE THE EXTENSION
        var lastSlashIndex = path.LastIndexOf('/');
        var publicIdWithExtension = path.Substring(lastSlashIndex + 1);

        //THE FILE EXTENSION (.jpg, .png, .gif, etc.) IS REMOVED TO GET THE PUBLIC ID
        var lastDotIndex = publicIdWithExtension.LastIndexOf('.');
        var publicId = publicIdWithExtension.Substring(0, lastDotIndex);

        return publicId;
    }

}
