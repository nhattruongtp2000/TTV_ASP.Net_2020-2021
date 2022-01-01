using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Interace
{
    public interface ICategoryRepository
    {
        Task<List<CategoryVm>> GetAllCategory();

        Task<List<CategoryVm>> GetAllCategory2();

        Task<int> ChangeIsShow(int IdCategory);


        // list category in admin
        Task<IPagedList<CategoryVm>> GetAll(int? page);

        Task<int> Edit( CategoryVm request);

        Task<int> Delete(int IdCategory);

        Task<List<CategoryVm>> SearchCategory(string CategoryName);

        Task<CategoryVm> GetOneCategory(int IdCategory);

        Task<int> Create(CategoryVm request);

    }
}
