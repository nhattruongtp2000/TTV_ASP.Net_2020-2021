using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        public string BlogName { get; set; }

        public string Image { set; get; }
        [MaxLength]
        public string Description { set; get; }

        public string Author { get; set; }
        public int? ParentId { get; set; }

        public int? ViewCount { set; get; }

        public string Content { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public bool Status { get; set; }

        public string SeoAlias { set; get; }

    }
}
