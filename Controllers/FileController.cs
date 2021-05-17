using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3;
using Amazon;
using S3Client.FileStoreClient;

namespace S3Client.Controllers
{
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileStoreClient _fileStoreClient;

        public FileController(IFileStoreClient fileStoreClient)
        {
            _fileStoreClient = fileStoreClient;
        }

        [HttpGet("healthcheck")]
        public string GetHealthStatus()
        {
            return "Healthy";
        }
        
        [HttpGet("get-file")]
        public async Task GetFileUrl()
        {
            await _fileStoreClient.DownloadAndSaveFileAsync("pdf-test.pdf", "./test.pdf");
        }
    } 
}
