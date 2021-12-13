using Data.Data;
using Data.DB;
using DI.DI.Interace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly Iden2Context _iden2Context;
        public BrandRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }

        public async  Task<int> Create(BrandVm request)
        {
            var x = new Brand()
            {
                BrandName = request.BrandName,
            };

            _iden2Context.Brands.Add(x);
            return await _iden2Context.SaveChangesAsync();
        }

        public async  Task<int> Delete(int IdBrand)
        {
            var brand = await _iden2Context.Brands.FirstOrDefaultAsync(x => x.IdBrand == IdBrand);
            _iden2Context.Brands.Remove(brand);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Edit(BrandVm request)
        {
            var brand = await _iden2Context.Brands.Where(x => x.IdBrand == request.IdBrand).FirstOrDefaultAsync();

            brand.BrandName = request.BrandName;

            return await _iden2Context.SaveChangesAsync();
        }

        public async  Task<IPagedList<BrandVm>> GetAll(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 9;

            var a = await _iden2Context.Brands.Select(x => new BrandVm()
            {
               IdBrand=x.IdBrand,
               BrandName=x.BrandName

            }).ToPagedListAsync(pageNumber, pageSize);

            return a;
        }

        public async Task<List<BrandVm>> GetAllBrand()
        {

            var query = from c in _iden2Context.Brands
                        select new { c };
            return await query.Select(x => new BrandVm()
            {
                IdBrand = x.c.IdBrand,
                BrandName = x.c.BrandName

            }).ToListAsync();
        }

        public async Task<BrandVm> GetOneBrand(int IdBrand)
        {
            var x = await _iden2Context.Brands.Where(x => x.IdBrand == IdBrand).FirstOrDefaultAsync();
            var xx = new BrandVm()
            {
                BrandName=x.BrandName,
                IdBrand=x.IdBrand
            };
            return xx;
        }

        public async Task<List<BrandVm>> SearchBrand(string BrandName)
        {
            var brand = _iden2Context.Brands.Where(x => x.BrandName.Contains(BrandName));

            var vm = await brand.Select(x => new BrandVm()
            {
                IdBrand=x.IdBrand,
                BrandName=x.BrandName
            }).ToListAsync();
            return vm;
        }
    }
}
