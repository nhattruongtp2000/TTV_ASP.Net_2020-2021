using Data.DB;
using DI.DI.Interace;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using Data.Data;

namespace DI.DI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Iden2Context _iden2Context;

        public CategoryRepository(Iden2Context iden2Context )
        {
            _iden2Context = iden2Context;


        }

        public async Task<int> ChangeIsShow(int IdCategory)
        {
            var cate = await _iden2Context.Categories.Where(x => x.IdCategory == IdCategory).FirstOrDefaultAsync();
            if (cate.IsShow == true)
            {
                cate.IsShow = false;
            }
            else
            {
                cate.IsShow = true;
            }
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Create(CategoryVm request)
        {
            var x = new Category()
            {
                NameCategory = request.NameCategory,
                ParentId = request.ParentId,
                IsShow = false
            };

            _iden2Context.Categories.Add(x);
            return await _iden2Context.SaveChangesAsync();
        }

        public async  Task<int> Delete(int IdCategory)
        {
            var category = await _iden2Context.Categories.FirstOrDefaultAsync(x => x.IdCategory == IdCategory);
            _iden2Context.Categories.Remove(category);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Edit(CategoryVm request)
        {
            var cate =await _iden2Context.Categories.Where(x => x.IdCategory == request.IdCategory).FirstOrDefaultAsync();
            
                cate.NameCategory = request.NameCategory;
                cate.ParentId = request.ParentId;
                
            return await _iden2Context.SaveChangesAsync();
        }

 

        public async Task<IPagedList<CategoryVm>> GetAll(int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 9;

            var a =await _iden2Context.Categories.Select(x => new CategoryVm() { 
            IdCategory=x.IdCategory,
            NameCategory=x.NameCategory,
            ParentId=x.ParentId,
            IsShow=x.IsShow
            
            
            }).ToPagedListAsync(pageNumber,pageSize);

            return a;
        }



        public async Task<List<CategoryVm>> GetAllCategory()
        {

                var query = from c in _iden2Context.Categories
                            select new { c };
                return await query.Select(x => new CategoryVm()
                {
                    IdCategory=x.c.IdCategory,
                    NameCategory=x.c.NameCategory,
                    ParentId=x.c.ParentId
                    

                }).ToListAsync();           
        }

        public async Task<List<CategoryVm>> GetAllCategory2()
        {
            var query = _iden2Context.Categories.Where(x => x.IsShow == true);
            return await query.Select(x => new CategoryVm()
            {
                IdCategory = x.IdCategory,
                NameCategory = x.NameCategory,
                ParentId = x.ParentId


            }).ToListAsync();
        }

        public async Task<CategoryVm> GetOneCategory(int IdCategory)
        {
            var x = await _iden2Context.Categories.Where(x => x.IdCategory == IdCategory).FirstOrDefaultAsync();
            var xx = new CategoryVm() { 
            IdCategory=x.IdCategory,
            NameCategory=x.NameCategory,
            ParentId=x.ParentId
            };
            return xx;
        }

        public async Task<List<CategoryVm>> SearchCategory(string CategoryName)
        {
            var cate =  _iden2Context.Categories.Where(x => x.NameCategory.Contains(CategoryName));

            var vm = await cate.Select(x => new CategoryVm() {
            IdCategory=x.IdCategory,
            NameCategory=x.NameCategory,
            ParentId=x.ParentId
            
            
            }).ToListAsync();
            return vm;
        }
    }
}
