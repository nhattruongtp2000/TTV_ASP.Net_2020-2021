using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class PhotoCreate
    {
        public int IdPhoto { get; set; }

        public int IdProduct { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
