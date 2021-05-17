using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace S3Client.FileStoreClient
{
    public static class Extensions
    {
        public static void RegisterFileStoreClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileStoreAWSS3Config>(
                configuration.GetSection(FileStoreAWSS3Config.NAME));
            services.AddTransient<IFileStoreClient, FileStoreClientAWSS3>();
        }
    }
}
