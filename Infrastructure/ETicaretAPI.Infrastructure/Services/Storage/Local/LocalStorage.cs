using ETicaretAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        readonly IWebHostEnvironment webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName) =>
            File.Delete($"{path}\\{fileName}");

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directoryInfo = new(path);
            return directoryInfo.GetFiles().Select(x => x.Name).ToList();

        }

        public bool HasFile(string path, string fileName)
            => File.Exists($"{path}\\{fileName}");

        async Task<bool> CopyFileAsync(string path, IFormFile formFile)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<(string fileName, string pathOrContainer)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            List<(string fileName, string path)> datas = new();
            foreach (var file in files)
            {
                string newFileName = await FileRenameAsync(path, file.Name, HasFile);

                await CopyFileAsync($"{uploadPath}\\{newFileName}", file);
                datas.Add((newFileName, $"{path}\\{newFileName}"));
            }
            return datas;
        }
    }
}
