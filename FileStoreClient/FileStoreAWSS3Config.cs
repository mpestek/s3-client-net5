using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace S3Client.FileStoreClient
{
    public class FileStoreAWSS3Config
    {
        public const string NAME = "AWSS3";

        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
        public string Bucket { get; set; }
    }
}
