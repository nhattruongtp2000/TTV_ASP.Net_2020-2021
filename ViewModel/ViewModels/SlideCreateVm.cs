using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class SlideCreateVm
    {
        public int Id { get; set; }

        public string SlideName { get; set; }

        public string Alias { get; set; }

        public bool IsShow { get; set; }

        public IFormFile FromFile { get; set; }
    }
}
