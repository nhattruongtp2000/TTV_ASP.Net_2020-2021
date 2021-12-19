using ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ViewModel;
using X.PagedList;
using ClosedXML.Excel;

namespace DI.DI.Interace
{
    public interface IProductRepository
    {
        Task<int> AddImages(int IdProduct, List<IFormFile> request);

        Task<int> Create(ProductCreateVm productVm);

        Task<List<ProductVm>> GetAll();

        Task<IPagedList<ProductVm>> GetAll2(int? page);

        Task<IPagedList<ProductVm>> Search(string key, int? page);

        Task<ProductVm> GetProduct(int IdProduct);

        Task<ProductDetailsVm> GetProductDetail(string Alias);

        Task<int> Edit(int IdProduct, ProductVm request);

        Task<int> Delete(int IdProduct);

        Task<IPagedList<ProductVm>> GetProductPerCategory(int IdCategory, int? page);

        Task<IPagedList<ProductVm>> GetProductPerSubCategory(int IdCategory, int? page);

        Task<IPagedList<ProductVm>> GetProductPerBrand(int IdBrand, int? page);

        Task<string> UpLoadFile(IFormFile fromFile);

        Task<IPagedList<ProductVm>> Filters(int pricemin, int pricemax, int? page);

        Task<int> AddComment(string Alias, string Content,int Review);

        Task<List<ProductVm>> RelatedProduct(int IdCategory, int IdProduct);

        Task<List<ProductVm>> MaybeLike(int IdBrand, int IdProduct);

        Task<IPagedList<ProductVm>> GetProductPerMutilpleBrandWithCategory(int IdCategory, int pricemin, int pricemax, int IdBrand1, int IdBrand2, int IdBrand3, int IdBrand4, int IdBrand5, int IdBrand6, int? page);

        Task<int> ChangIsShow(int IdProduct);

        Task<int> ChangeIsStandout(int IdProduct);
        //ProductVm GetbyId(int IdProduct);  
    }
}
