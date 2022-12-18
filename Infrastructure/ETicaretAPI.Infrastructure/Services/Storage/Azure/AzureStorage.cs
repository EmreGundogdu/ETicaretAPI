using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient blobServiceClient;
        BlobContainerClient blobContainerClient;
        public AzureStorage(IConfiguration configuration)
        {
            blobServiceClient = new(configuration["Storage:Azure"]);
        }
        public async Task DeleteAsync(string containerName, string fileName)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return blobContainerClient.GetBlobs().Select(x => x.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return blobContainerClient.GetBlobs().Any(x => x.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainer)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
            List<(string fileNmae, string pathOrContainer)> datas = new();
            foreach (IFormFile file in files)
            {
                string newFileName = await FileRenameAsync(containerName, file.Name, HasFile);

                BlobClient blobClient = blobContainerClient.GetBlobClient(file.Name);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((newFileName, containerName));
            }
            return datas;
        }
    }
}
