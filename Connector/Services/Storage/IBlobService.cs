using System;
using System.IO;
using System.Threading.Tasks;

namespace Sample.Services.Storage
{
    public interface IBlobService
    {
        Task Upload(Uri endpoint, string content);
        Task Upload(Uri endpoint, Stream content);
    }
}