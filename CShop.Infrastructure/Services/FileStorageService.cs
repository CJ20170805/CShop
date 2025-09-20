using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CShop.Application.Interfaces;
using CShop.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infrastructure.Services
{
    public class FileStorageService: IFileStorageService
    {
       private readonly BlobServiceClient _blobService;
       private readonly StorageOptions _options;

        public FileStorageService(IOptions<StorageOptions> options)
        {
            _options = options.Value;
            _blobService = new BlobServiceClient(_options.ConnectionString);
        }
        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string containerKey)
        {
            _options.Containers.TryGetValue(containerKey, out var containerName);
                if(string.IsNullOrEmpty(containerName))
                    throw new ArgumentException($"Container with key '{containerKey}' not found.");

            var containerClient = _blobService.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
            
            return blobClient.Uri.ToString();
        }
    }
}
