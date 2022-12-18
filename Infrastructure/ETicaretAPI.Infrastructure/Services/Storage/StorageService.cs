using ETicaretAPI.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage storage;

        public StorageService(IStorage storage)
        {
            this.storage = storage;
        }

        public string StorageName => storage.GetType().Name;

        public async Task DeleteAsync(string pathOrContainer, string fileName) => await storage.DeleteAsync(pathOrContainer, fileName);

        public List<string> GetFiles(string path) => storage.GetFiles(path);

        public bool HasFile(string pathOrContainer, string fileName) => storage.HasFile(pathOrContainer, fileName);

        public async Task<List<(string fileName, string pathOrContainer)>> UploadAsync(string pathOrContainer, IFormFileCollection files) => await storage.UploadAsync(pathOrContainer, files);
    }
}
