namespace QuoteManager_Pro.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string subDirectory);

    }
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subDirectory)
        {
            //create unique filename
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";

            //create directory if not exists
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads",subDirectory);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            //save file
            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);

            }
            return $"/uploads/{subDirectory}/{fileName}";
        }
    }
}
