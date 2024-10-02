public interface ICloudinaryService
{
    Task<string> UploadImageAsync(string imagePath, string publicId);
}
