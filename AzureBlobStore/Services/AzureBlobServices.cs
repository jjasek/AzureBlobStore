using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobStore.Services
{
    public class AzureBlobServices
    {
        public async Task UploadImage(List<string> imagePaths)
        {
            BlobServiceClient bsc = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=tgrstorage;AccountKey=07YNLuRL4Sw2cGHLLnXA1hVuPDOqyg63PJBhb/vqJJQV/q4X5eZZV+KaEjhCXRk/iXw6tCztiryx0lp8qmPfHg==;EndpointSuffix=core.windows.net");
            var container = bsc.GetBlobContainerClient("pictures");
            await container.CreateIfNotExistsAsync();

            var bh = new BlobHttpHeaders();
            bh.ContentType = "image/jpg";

            var blobClient = container.GetBlobClient($"{Guid.NewGuid()}.jpg");
            foreach(var i in imagePaths)
            {
                await blobClient.UploadAsync(i);
            }
            await blobClient.SetHttpHeadersAsync(bh);
        }

        public List<Stream> DownloadImage()
        {
            BlobServiceClient bsc = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=tgrstorage;AccountKey=07YNLuRL4Sw2cGHLLnXA1hVuPDOqyg63PJBhb/vqJJQV/q4X5eZZV+KaEjhCXRk/iXw6tCztiryx0lp8qmPfHg==;EndpointSuffix=core.windows.net");
            var container = bsc.GetBlobContainerClient("pictures");
            List<Stream> blobDownloadInfos = new List<Stream>();
            container.GetBlobs().Select(p => p.Name).ToList().ForEach(async p =>
            {
                BlobDownloadInfo download = await container.GetBlobClient(p).DownloadAsync();
                blobDownloadInfos.Add(download.Content);
            });
            return blobDownloadInfos;
        }
    }
}
