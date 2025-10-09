using Microsoft.AspNetCore.Http;

namespace PickleballStore.BLL.Services
{
    public class FileService
    {
        public async Task<string> GenerateFile(IFormFile file, string filePath)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is null or empty", nameof(file));

            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{fileNameWithoutExt}-{Guid.NewGuid()}{extension}";

            var fullPath = Path.Combine(filePath, fileName);

            Directory.CreateDirectory(filePath);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public bool IsImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var contentType = file.ContentType.ToLowerInvariant();
            return contentType.StartsWith("image/");
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(wwwrootPath, "images", "products");
            return await GenerateFile(file, uploadsFolder);
        }

        public void DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var filePath = Path.Combine(wwwrootPath, "images", "products", fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}