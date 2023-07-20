using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services.Interface;

namespace Services.Services
{
    public class MediaUploadService : IMediaUploadService
    {
        private readonly IConfiguration _configuration;

        public MediaUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> AddImage(IFormFile? File, string WebFolder, string FileFolder)
        {
            //can pass WebFolder typesafely or using some kind of injection
            var TofilePath = string.Empty;
            if (File != null && File.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                var filePath = Path.Combine(WebFolder, FileFolder, fileName);
                await SaveFile(filePath, File);
                TofilePath = Path.Combine(FileFolder, fileName);
            }
            else
            {
                //default file Location for image not found
                TofilePath = _configuration.GetValue<string>("PreLoadedImage")?? throw new ArgumentNullException();
            }
            return TofilePath;
        }
        public async Task<string> ChangeImage(IFormFile UFile, string WebFolder, string FileFolder, string FilePath)
        {
            if (UFile != null && UFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(FilePath) && FilePath.Contains(FileFolder))
                {
                    var existingFilePath = Path.Combine(WebFolder, FilePath);
                    await DeleteFile(existingFilePath);
                }
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UFile.FileName);
                var FullFilePath = Path.Combine(WebFolder, FileFolder, fileName);
                await SaveFile(FullFilePath, UFile);
                FilePath = Path.Combine(FileFolder, fileName);
            }
            return FilePath;
        }
        private async Task SaveFile(string filePath, IFormFile File)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }
        }
        private async Task DeleteFile(string existingFilePath)
        {

            if (File.Exists(existingFilePath))
            {
                await Task.Run(() => File.Delete(existingFilePath));
            }
        }
    }
}
