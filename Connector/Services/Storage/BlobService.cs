using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace Sample.Services.Storage
{
    public class BlobService : IBlobService
    {
        public async Task Upload(Uri endpoint, string content)
        {
            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await Upload(endpoint, stream);
        }

        public async Task Upload(Uri endpoint, Stream content)
        {
            var blob = new BlobClient(endpoint);
            await blob.UploadAsync(content);
        }

        public async Task<Stream> Download(Uri endpoint)
        {
            var blob = new BlobClient(endpoint);
            return await blob.OpenReadAsync();
        }
    }
}