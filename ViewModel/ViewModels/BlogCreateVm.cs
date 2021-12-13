using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class BlogCreateVm
    {
        public string BlogName { get; set; }

        public IFormFile Image { set; get; }

        public string Author { get; set; }
        public string Description { set; get; }

        public int? ParentId { get; set; }

        public string Content { set; get; }

        public bool Status { get; set; }

        public string SeoAlias { set; get; }
    }
}
