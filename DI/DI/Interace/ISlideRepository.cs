using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Interace
{
    public interface ISlideRepository
    {
        Task<List<SlideVm>> GetAllSlide();
        // list category in admin
        Task<IPagedList<SlideVm>> GetAll(int? page);

        Task<int> Edit(SlideVm request);

        Task<int> Delete(int IdSlide);

        Task<SlideVm> SearchSlide(int Id);

        Task<SlideVm> GetOneSlide(int IdSlide);

        Task<int> Create(SlideCreateVm request);

        Task<string> UpLoadFile(IFormFile fromFile);

        Task<int> ChangIsShowSlide(int IdSlide);


    }
}
