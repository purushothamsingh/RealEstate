using CloudinaryDotNet.Actions;
using RealEstateAPI.Models;

namespace RealEstateAPI.Repositories.PhotoRepo
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
