using Microsoft.AspNetCore.Http;

namespace ETicaretAPI.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainer)>> UploadAsync(string pathOrContainer, IFormFileCollection files);
        Task DeleteAsync(string pathOrContainer, string fileName);
        List<string> GetFiles(string path);
        bool HasFile(string pathOrContainer, string fileName);

    }
}
