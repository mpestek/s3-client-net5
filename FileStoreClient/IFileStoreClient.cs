using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3Client.FileStoreClient
{
    public interface IFileStoreClient
    {
        public string GetPresignedUrlForGet(string fileKey);
        public string GetPresignedUrlForUpload(string fileKey);
        public Task UploadFileAsync(string fileKey, byte[] file);
        public Task<byte[]> DownloadFileAsync(string fileKey);
        public Task DownloadAndSaveFileAsync(string fileKey, string path);
    }
}
