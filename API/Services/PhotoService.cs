using System.Security.AccessControl;
using System.Threading.Tasks;
using API.Helpers;                                                          // CloudinarySettings
using API.Interfaces;                                                                    // IPhotoService
using CloudinaryDotNet;                                         // Cloudinary
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;                     // IOptions

namespace API.Services
{
    public class PhotoService : IPhotoService                           // within PhotoService we will need access to Cloudinary so we need to implement a method constructor here
    {
        private readonly Cloudinary _cloudinary;                    // here we are providing details of our API keys
        public PhotoService(IOptions<CloudinarySettings> config)      // we use IOptions interface to get our configurations when we create a class to store our configurations //CloudinarySettings is what we called our API class
        {
            var acc = new Account               // we are creating a Cloudinary account here // takes config options inside parenthesis // ordering important here
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);          // acc is the variable we created above
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)          // needs to be an async method
        {
            var uploadResult = new ImageUploadResult();         // the variable to store our upload

            if (file.Length > 0)        // Length property available on a file object   // we check to see if we have somethign in our file paramater here
            {                                   // below we get our file as a stream of data
                using var stream = file.OpenReadStream();         // disposable with using .. this is the logic to upload our file to cloudinary // we use a stream here // OpenReadStream() not asynchronous method so we don't use await here
                var uploadParams = new ImageUploadParams    
                {                                                                                   // this is the logic to upload our file to cloudinary
                    File = new FileDescription(file.FileName, stream),  // specify file name, add file name and stream of file
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face") // specify what transform settings are req. // crops to sqare and focuses on face
                };  // next, we upload our result to cloudinary
                uploadResult = await _cloudinary.UploadAsync(uploadParams);         // async method implemented here
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
           var deleteParams = new DeletionParams(publicId);
           var result = await _cloudinary.DestroyAsync(deleteParams);

           return result;
        }
    }
}