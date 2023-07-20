using Microsoft.AspNetCore.Http;

namespace Services.Interface
{
    public interface IMediaUploadService
    {
        Task<string> AddImage(IFormFile? File, string WebFolder, string FileFolder);
        Task<string> ChangeImage(IFormFile UFile, string WebFolder, string FileFolder, string FilePath);
    }
}