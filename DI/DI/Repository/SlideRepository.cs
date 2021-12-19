using Data.Data;
using Data.DB;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Repository
{
    public class SlideRepository : ISlideRepository
    {
        private readonly Iden2Context _iden2Context;
        public SlideRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }

        public async Task<int> ChangIsShowSlide(int IdSlide)
        {
            var slide = await _iden2Context.Slides.Where(x => x.Id == IdSlide).FirstOrDefaultAsync();
            if (slide.IsShow == true)
            {
                slide.IsShow = false;
            }
            else
            {
                slide.IsShow = true;
            }
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Create(SlideCreateVm request)
        {
            var x = await UpLoadFile(request.FromFile);
            var slide = new Slide()
            {
                SlideName = request.SlideName,
                IsShow = request.IsShow,
                Alias = request.Alias,
                FromFile = x,
                DateUp = DateTime.Now.Date
            };
            _iden2Context.Slides.Add(slide);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Delete(int IdSlide)
        {
            var slide = await _iden2Context.Slides.FirstOrDefaultAsync(x => x.Id == IdSlide);
            _iden2Context.Slides.Remove(slide);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Edit(SlideVm request)
        {
            var slide =await _iden2Context.Slides.FindAsync(request.Id);
            slide.Alias = request.Alias;
            slide.SlideName = request.SlideName;
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<IPagedList<SlideVm>> GetAll(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 9;

            var a = await _iden2Context.Slides.Select(x => new SlideVm()
            {
                SlideName = x.SlideName,
                IsShow = x.IsShow,
                Alias = x.Alias,
                FromFile = x.FromFile,
                DateUp = x.DateUp,
                Id=x.Id

            }).ToPagedListAsync(pageNumber, pageSize);

            return a;
        }

        public async Task<List<SlideVm>> GetAllSlide()
        {
            throw new NotImplementedException();
        }

        public async Task<SlideVm> GetOneSlide(int IdSlide)
        {
            var x =await _iden2Context.Slides.FindAsync(IdSlide);
            var vm = new SlideVm()
            {
                Id=x.Id,
                Alias = x.Alias,
                SlideName = x.SlideName,
                DateUp = x.DateUp,
            };
            return vm;
        }

        public async Task<SlideVm> SearchSlide(int  Id)
        {
            var x =await _iden2Context.Slides.Where(x => x.Id == Id).FirstOrDefaultAsync();
            var slide = new SlideVm()
            {
                SlideName = x.SlideName,
                IsShow = x.IsShow,
                Alias = x.Alias,
                FromFile = x.FromFile,
                DateUp = x.DateUp,
                Id = x.Id
            };
            return slide;
        }

        public async Task<string> UpLoadFile(IFormFile fromFile)
        {          
            if(fromFile==null || fromFile.Length == 0)
            {
                return null;
            }
            var path = Path.Combine("wwwroot", "Images", fromFile.FileName);
            using(var stream = new FileStream(path, FileMode.Create))
            {
                await fromFile.CopyToAsync(stream);
            }
            return path.Substring(8);
        }
    }
}
