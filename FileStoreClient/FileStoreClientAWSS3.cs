using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using S3Client.FileStoreClient;

namespace S3Client.FileStoreClient
{
    public class FileStoreClientAWSS3 : IFileStoreClient, IDisposable
    {
        private const int GET_PRESIGNED_URL_EXPIRATION_IN_MIN = 10;
        private const int UPLOAD_PRESIGNED_URL_EXPIRATION_IN_MIN = 10;

        private readonly AmazonS3Client _amazonS3Client;
        private readonly string _bucketName;

        public FileStoreClientAWSS3(IOptions<FileStoreAWSS3Config> fileStoreAWSConfigOptions)
        {
            var fileStoreAWSConfig = fileStoreAWSConfigOptions.Value;

            _amazonS3Client = new AmazonS3Client(
                fileStoreAWSConfig.AccessKey,
                fileStoreAWSConfig.SecretKey,
                RegionEndpoint.GetBySystemName(fileStoreAWSConfig.Region)
            );

            _bucketName = fileStoreAWSConfig.Bucket;
        }

        public string GetPresignedUrlForGet(string fileKey)
        {
            return this._amazonS3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = this._bucketName,
                Key = fileKey,
                Expires = DateTime.Now.AddMinutes(GET_PRESIGNED_URL_EXPIRATION_IN_MIN),
                Verb = HttpVerb.GET
            });
        }

        public string GetPresignedUrlForUpload(string fileKey)
        {
            return this._amazonS3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = this._bucketName,
                Key = fileKey,
                Expires = DateTime.Now.AddMinutes(UPLOAD_PRESIGNED_URL_EXPIRATION_IN_MIN),
                Verb = HttpVerb.PUT
            });
        }

        public async Task UploadFileAsync(string fileKey, byte[] file)
        {
            await _amazonS3Client.PutObjectAsync(new PutObjectRequest
            {
                InputStream = new MemoryStream(file),
                BucketName = _bucketName,
                Key = fileKey
            });
        }

        public async Task<byte[]> DownloadFileAsync(string fileKey)
        {
            var response = await _amazonS3Client.GetObjectAsync(_bucketName, fileKey);
            
            using (var responseStream = response.ResponseStream)
            using (var memStream = new MemoryStream())
            {
                responseStream.CopyTo(memStream);
                
                return memStream.ToArray();
            }
        }

        public async Task DownloadAndSaveFileAsync(string fileKey, string path)
        {
            var bytes = await DownloadFileAsync(fileKey);

            await File.WriteAllBytesAsync(path, bytes);
        }

        public void Dispose()
        {
            this._amazonS3Client.Dispose();
        }
    }
}
