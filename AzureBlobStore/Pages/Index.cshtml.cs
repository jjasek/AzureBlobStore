using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AzureBlobStore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private BlobServiceClient _bsc;
        private BlobContainerClient _container;
        public Uri uri;
        public List<string> pictures;

        [BindProperty]
        public IFormFile[] UploadedFile { get; set; }

        public IndexModel(ILogger<IndexModel> logger, BlobServiceClient bsc)
        {
            _logger = logger;
            _bsc = bsc;
            _container = _bsc.GetBlobContainerClient("pictures");
            _container.CreateIfNotExistsAsync();
            uri = _bsc.Uri;
            pictures = getPictureNames();
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            var bh = new BlobHttpHeaders();
            bh.ContentType = "image/jpg";

            foreach (var file in UploadedFile)
            {
                var blobClient = _container.GetBlobClient($"{Guid.NewGuid()}.jpg");
                await blobClient.UploadAsync(file.OpenReadStream());
                await blobClient.SetHttpHeadersAsync(bh);
            }
            pictures = getPictureNames();
        }

        private List<string> getPictureNames()
        {
            return _container.GetBlobs().Select(p => p.Name).ToList();
        }
    }
}
