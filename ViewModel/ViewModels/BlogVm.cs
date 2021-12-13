using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class BlogVm
    {
        public int BlogId { get; set; }
        public string BlogName { get; set; }

        public string Image { set; get; }

        public string Description { set; get; }

        public string Author { get; set; }
        public int? ViewCount { get; set; }
        public int? ParentId { get; set; }
        public DateTime DateCreated { get; set; }

        public string Content { set; get; }

        public bool Status { get; set; }

        public string SeoAlias { set; get; }

    }
}
