using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Interace
{
    public interface IBrandRepository
    {
        Task<List<BrandVm>> GetAllBrand();
        // list category in admin
        Task<IPagedList<BrandVm>> GetAll(int? page);

        Task<int> Edit(BrandVm request);

        Task<int> Delete(int IdBrand);

        Task<List<BrandVm>> SearchBrand(string BrandName);

        Task<BrandVm> GetOneBrand(int IdBrand);

        Task<int> Create(BrandVm request);
    }
}
