using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DUBle_Watch.Models.AnimeViewModels
{
    public class UploadImageViewModel
    {
        public Anime Anime { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}

