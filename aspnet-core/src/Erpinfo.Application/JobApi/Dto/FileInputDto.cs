using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erpinfo.JobApi.Dto
{
    public class FileInputDto
    {
        public IFormFile File { get; set; }
    }
}
