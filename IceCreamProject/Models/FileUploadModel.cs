using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IceCreamProject.Models
{
    public class FileUploadModel
    {
        public IFormFile File { get; set; }
    }
}
