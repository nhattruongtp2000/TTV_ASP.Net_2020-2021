using Data.Data;
using Data.DB;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Repository
{

    public class BlogRepository : IBlogRepository
    {
        private readonly Iden2Context _iden2Context;

        public BlogRepository(Iden2Context iden2Context)
        { 
            _iden2Context = iden2Context;

        }
        public async Task<int> AddBlog(BlogCreateVm request)
        {
            var c =await UpLoadFile(request.Image);

            var blog = new Blog()
            {
                SeoAlias=request.SeoAlias,
                Status=request.Status,
                BlogName=request.BlogName,
                Content=request.Content,
                Description=request.Description,
                Image=c,
                DateCreated=DateTime.Now,
                ParentId=request.ParentId,
                ViewCount=0,
                Author=request.Author
                
            };
            _iden2Context.Blogs.Add(blog);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<BlogDetailsVm> BlogDetails(int BlogId)
        {
            var blog =await _iden2Context.Blogs.Where(x => x.BlogId == BlogId).FirstOrDefaultAsync();
            blog.ViewCount++;
            var relatedblogs = _iden2Context.Blogs.Where(x => x.ParentId == blog.ParentId && x.BlogId != BlogId);

            var a = await relatedblogs.Select(x => new BlogVm()
            {
                BlogName = x.BlogName,
                Content = x.Content,
                Image = x.Image,
                DateCreated = x.DateCreated,
                ParentId = x.ParentId,
                Author = x.Author,
                BlogId=x.BlogId

            }).ToListAsync();

            var blogvm = new BlogVm()
            {
                BlogId=blog.BlogId,
                SeoAlias = blog.SeoAlias,
                Status = blog.Status,
                BlogName = blog.BlogName,
                Content = blog.Content,
                Description = blog.Description,
                Image = blog.Image,
                DateCreated = DateTime.Now,
                ParentId = blog.ParentId,
                ViewCount = 0,
                Author = blog.Author              
            };
            var blogdetails = new BlogDetailsVm()
            {
                Blog = blogvm,
                RelatedBlogs = a

            };
            _iden2Context.SaveChanges();
            return blogdetails;
        }

        public async Task<int> ChangStatus(int BlogId)
        {
            var blog = await _iden2Context.Blogs.Where(x => x.BlogId == BlogId).FirstOrDefaultAsync();
            if (blog.Status == true)
            {
                blog.Status = false;
            }
            else
            {
                blog.Status = true;
            }
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<List<BlogVm>> GetAll()
        {
            var blogg = _iden2Context.Blogs;


            var a = await blogg.Select(x => new BlogVm() {
                BlogId=x.BlogId,
                SeoAlias = x.SeoAlias,
                Status = x.Status,
                BlogName = x.BlogName,
                Content = x.Content,
                Description = x.Description,
                Image = x.Image,
                DateCreated = x.DateCreated,
                ParentId = x.ParentId,
                ViewCount = x.ViewCount,
                Author=x.Author
               
            }).ToListAsync();
            return a;
        }

        public async Task<List<BlogVm>> ShowinIndex()
        {
            var blogg = _iden2Context.Blogs.Where(x=>x.Status==true);


            var a = await blogg.Select(x => new BlogVm()
            {
                BlogId = x.BlogId,
                SeoAlias = x.SeoAlias,
                Status = x.Status,
                BlogName = x.BlogName,
                Content = x.Content,
                Description = x.Description,
                Image = x.Image,
                DateCreated = x.DateCreated,
                ParentId = x.ParentId,
                ViewCount = x.ViewCount,
                Author = x.Author

            }).ToListAsync();
            return a;
        }

        public async Task<string> UpLoadFile(IFormFile fromFile)
        {
            if (fromFile == null || fromFile.Length == 0)
            {
                return null;
            }

            var path = Path.Combine("wwwroot", "Images", fromFile.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await fromFile.CopyToAsync(stream);
            }
            return path.Substring(8);
        }
    }
}
