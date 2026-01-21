using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using WebService.Models;

namespace WebService.Services
{
    public class CloudinaryService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new CloudinaryDotNet.Cloudinary(acc);
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "sportelite/product",
                PublicId = $"sportelite/product/{Path.GetFileNameWithoutExtension(fileName)}"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            throw new Exception("Image upload failed: " + uploadResult.Error?.Message);
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            // Get publicId from URL Cloudinary
            // Example: https://res.cloudinary.com/xxx/image/upload/v123456/sportelite/product/SP000001_7.jpg
            // publicId is "sportelite/product/SP000001_7"
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');
            var fileName = segments.Last(); // Example: SP000001_7.jpg
            var publicId = $"sportelite/product/{Path.GetFileNameWithoutExtension(fileName)}";
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            if (result.Result != "ok" && result.Result != "not found")
            {
                throw new Exception("Failed to delete image from Cloudinary: " + result.Error?.Message);
            }
        }
    }
}
