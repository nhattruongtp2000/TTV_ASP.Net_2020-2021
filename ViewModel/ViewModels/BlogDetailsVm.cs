using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class BlogDetailsVm
    {
       public List<BlogVm> RelatedBlogs { get; set; }

       public  BlogVm Blog { get; set; }
    }
}
