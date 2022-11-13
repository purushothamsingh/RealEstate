using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using RealEstateAPI.Repositories.PhotoRepo;

namespace RealEstateAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;

        public PhotoService(IConfiguration config)
        {
            Account account = new Account(
                 "drn3bc1nj",
            "141665214458799",
            "tNXJvyvIqXYfqHNiQgAA5Uc8QdU");  
            
            cloudinary = new Cloudinary(account);
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await cloudinary.DestroyAsync(deleteParams);

            return result;
        }

        public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo)
        {
            var uploadResult = new ImageUploadResult();
            if(photo.Length > 0)
            {
                using var stream = photo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(photo.FileName, stream),
                    Transformation = new Transformation()
                    .Height(500).Width(800)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
    }
}
