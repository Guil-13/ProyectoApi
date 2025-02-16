namespace ProyectoApi.Servicios
{
    public interface IFileService
    {
        Task Delete(string? path, string container);
        Task<string> Save(string fileName, string container, IFormFile file);
        async Task<string> Replace(string fileName, string? path, string container, IFormFile file)
        {
            await Delete(path, container);
            return await Save(fileName, container, file);
        }
    }
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }
        public Task Delete(string? path, string container)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return Task.CompletedTask;
            }
            var fileName = Path.GetFileName(path);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return Task.CompletedTask;
        }

        public async Task<string> Save(string fileName, string container, IFormFile file)
        {
            //string folder = Path.Combine($@"\\UPV-SERVER1\MiUPV\Documents\{DateTime.Now.Year}\", container);
            string folder = Path.Combine($@"\\Blackguard-Stud\Users\Blackguard\Documents\MiUPV\Documents\{DateTime.Now.Year}\", container);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filePath = Path.Combine(folder, fileName); //nombreArchivo
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var content = ms.ToArray();
                await File.WriteAllBytesAsync(filePath, content);
            }
            return filePath;
        }
    }
}
