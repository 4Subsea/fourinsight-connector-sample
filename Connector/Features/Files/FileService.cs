using System;
using System.IO;
using System.Threading.Tasks;
using Sample.Services.Rest;
using Sample.Services.Storage;

namespace Sample.Features.Files
{
    public class FileService : IFileService
    {
        private readonly IRestService _rest;
        private readonly IBlobService _blob;

        public FileService(
            IRestService rest,
            IBlobService blob)
        {
            _rest = rest;
            _blob = blob;
        }

        public async Task<Guid> Upload(Stream timeSeriesContent)
        {
            // Request for uploading a file
            var upload = await _rest.Post<UploadResponse>("/api/files/upload");

            // Upload content
            await _blob.Upload(upload.EndpointUri, timeSeriesContent);

            // Commit the file
            var commit = new { upload.FileId };
            await _rest.Post("/api/files/commit", commit);

            return upload.FileId;
        }

        public async Task WaitForFileProcessed(Guid fileId)
        {
            var state = "";
            var stateMessage = "";
            while (state != "Ready" && state != "Failed")
            {
                var response = await _rest.Get<StatusResponse>($"/api/files/{fileId:N}/status");

                state = response.State;
                stateMessage = response.StateMessage;

                Console.WriteLine($"Status of {fileId}: {state} {stateMessage}...");
                await Task.Delay(1000);
            }
            Console.WriteLine($"File {fileId}: processed with {state} {stateMessage}");
        }


        private class UploadResponse
        {
            public Guid FileId { get; set; }
            public string Endpoint { get; set; }
            public Uri EndpointUri => new Uri(Endpoint);
        }

        private class StatusResponse
        {
            public string State { get; set; }
            public string StateMessage { get; set; }
        }
    }
}