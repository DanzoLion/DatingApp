using System.Threading.Tasks;
using CloudinaryDotNet.Actions;         //ImageUploadResult
using Microsoft.AspNetCore.Http;        // IFormFile        // represents a file sent with HttpRequest

namespace API.Interfaces
{
    public interface IPhotoService
    {
        
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);          
        Task<DeletionResult> DeletePhotoAsync(string publicId);     // each file uploaded to cloudinary is given a publicId // used in order to delete the image  // we already created PublicId property in Photo.cs

    }
}