using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class Photos
    {
        public int IdPhoto { get; set; }

        public int IdProduct { get; set; }

        public string LinkImageFile { get; set; }
    }
}
