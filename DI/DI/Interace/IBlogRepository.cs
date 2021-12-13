using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Interace
{
    public interface IBlogRepository
    {
        Task<int> AddBlog(BlogCreateVm request);

        Task<List<BlogVm>> GetAll();

        Task<BlogDetailsVm> BlogDetails(int BlogId);

        Task<int> ChangStatus(int BlogId);

        Task<List<BlogVm>> ShowinIndex();
    }
}
