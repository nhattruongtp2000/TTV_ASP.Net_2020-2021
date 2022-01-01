using Data.Data;
using Data.DB;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ViewModel;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Iden2Context _iden2Context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductRepository(Iden2Context iden2Context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _iden2Context = iden2Context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Create(ProductCreateVm request)
        {
            var c = await UpLoadFile(request.PhotoReview);

            var product = new Product()
            {
                DateAccept=DateTime.Now,
                IdBrand = request.IdBrand,
                ProductName = request.ProductName,
                UseVoucher = request.UseVoucher,
                IdCategory = request.IdCategory,
                IsFree = false,
                Price = request.Price,
                PriceExport=request.PriceExport,
                PhotoReview = c,
                Content = request.Content,
                Description = request.Description,
                Alias = request.Alias,
                Keyword = request.Keyword,
                Quantity = request.Quantity,
                Title = request.Title,
                IsGift = request.IsGift,
                IdProductGiveTo = request.IdProductGiveTo,
                IsShow=request.IsShow,
                IsStandout=request.IsStandout

            };
            _iden2Context.Products.Add(product);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Delete(int IdProduct)
        {
            var product = await _iden2Context.Products.FirstOrDefaultAsync(x => x.IdProduct == IdProduct);
            _iden2Context.Products.Remove(product);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Edit(int IdProduct, ProductVm request)
        {
            var product = await _iden2Context.Products.FirstOrDefaultAsync(x => x.IdProduct == IdProduct);

            product.IdBrand = request.IdBrand;
            product.ProductName = request.ProductName;
            product.UseVoucher = request.UseVoucher;
            product.PriceExport = request.PriceExport;
            product.Price = request.Price;
            product.Alias = request.Alias;
            product.Quantity = request.Quantity;
            product.IdCategory = request.IdCategory;
            product.Description = request.Description;

            await _iden2Context.SaveChangesAsync();
            return product.IdBrand;

        }

        public async Task<List<ProductVm>> GetAll()
        {
            var product = from p in _iden2Context.Products
                           join pt in _iden2Context.Brands on p.IdBrand equals pt.IdBrand
                           join ptt in _iden2Context.Categories on p.IdCategory equals ptt.IdCategory
                           select new { p, pt, ptt };


            var a = await product.Select(x => new ProductVm()
            {
                IdProduct = x.p.IdProduct,
                ProductName = x.p.ProductName,
                Price = x.p.Price,
                PriceExport = x.p.PriceExport,
                IsShow=x.p.IsShow,
                IsStandout=x.p.IsStandout,
                PhotoReview = x.p.PhotoReview,
                Description = x.p.Description,
                NameBrand = x.pt.BrandName,
                NameCategory = x.ptt.NameCategory

            }).ToListAsync();
            return a;
        }

        public async Task<IPagedList<ProductVm>> GetProductPerCategory(int IdCategory, int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 9;


            var cate2 = from p in _iden2Context.Products
                        join pt in _iden2Context.Categories on p.IdCategory equals pt.IdCategory
                        select new { p, pt };


            var product = cate2.Where(x => x.p.IdCategory == IdCategory || x.pt.ParentId == IdCategory);




            var a = await product.Select(x => new ProductVm()
            {
                IdProduct = x.p.IdProduct,
                DateAccept = x.p.DateAccept,
                IdBrand = x.p.IdBrand,
                ProductName = x.p.ProductName,
                UseVoucher = x.p.UseVoucher,
                PhotoReview = x.p.PhotoReview,
                IdCategory = x.p.IdCategory,
                IsFree = x.p.IsFree,
                Price = x.p.Price,
                PriceExport = x.p.PriceExport,
                Alias = x.p.Alias
            }).ToPagedListAsync(pageNumber, pageSize);
            return a;
        }

        public async Task<IPagedList<ProductVm>> GetAll2(int? page)
        {
            var products = _iden2Context.Products;

            var pageNumber = page ?? 1;
            int pageSize = 9;

            var a = await products.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                PhotoReview = x.PhotoReview,
                IdCategory = x.IdCategory,
                IsFree = x.IsFree,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias,
                Quantity = x.Quantity,
                IsShow=x.IsShow,
                IsStandout=x.IsStandout
            }).ToPagedListAsync(pageNumber, pageSize);
            return a;
        }


        public async Task<ProductVm> GetProduct(int IdProduct)
        {

            var product = await(from p in _iden2Context.Products
                          join pt in _iden2Context.Brands on p.IdBrand equals pt.IdBrand
                          join ptt in _iden2Context.Categories on p.IdCategory equals ptt.IdCategory
                          select new { p, pt, ptt }).Where(x=>x.p.IdProduct==IdProduct).FirstOrDefaultAsync();

            var a = new ProductVm()
            {
                IdProduct = product.p.IdProduct,
                DateAccept = product.p.DateAccept,
                IdBrand = product.p.IdBrand,
                ProductName = product.p.ProductName,
                UseVoucher = product.p.UseVoucher,
                IdCategory = product.p.IdCategory,
                Price = product.p.Price,
                PriceExport = product.p.PriceExport,
                Alias = product.p.Alias,
                PhotoReview=product.p.PhotoReview,
                Quantity=product.p.Quantity,    
                Description=product.p.Description,
                NameBrand=product.pt.BrandName,
                NameCategory=product.ptt.NameCategory
                

            };
            return a;
        }



        public async Task<IPagedList<ProductVm>> Search(string key, int? page)
        {
            var products = _iden2Context.Products.Where(x => x.ProductName.Contains(key) && x.IsShow==true);
            var pageNumber = page ?? 1;
            int pageSize = 9;

            var a = await products.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                PhotoReview = x.PhotoReview,
                IdCategory = x.IdCategory,
                IsFree = x.IsFree,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias
            }).ToPagedListAsync(pageNumber, pageSize);

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

        public async Task<int> AddImages(int IdProduct, List<IFormFile> request)
        {

            if (request == null || request.Count == 0)
            {
                return 0;
            }

            foreach (var file in request)
            {
                var path = Path.Combine("wwwroot", "Images", file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var b = new ProductPhoto()
                {
                    IdProduct = IdProduct,
                    IFromFile = path.Substring(8)
                };
                _iden2Context.ProductPhotos.Add(b);
            }
            await _iden2Context.SaveChangesAsync();
            return 1;
        }

        public async Task<ProductDetailsVm> GetProductDetail(string Alias)
        {
            var k = _iden2Context.Products.Where(x => x.Alias == Alias).FirstOrDefault();
            int IdProduct = 0;
            if (k == null)
            {
                IdProduct = _iden2Context.Products.Where(x => x.IdProduct == int.Parse(Alias)).FirstOrDefault().IdProduct;
            }
            else
            {
                IdProduct = k.IdProduct;
            }

            var photos = _iden2Context.ProductPhotos.Where(x => x.IdProduct == IdProduct);
            List<string> linkPhotos = await photos.Select(x => x.IFromFile).ToListAsync();

            var product = await _iden2Context.Products.FirstOrDefaultAsync(x => x.IdProduct == IdProduct);
            var commemt = _iden2Context.Comments.Where(x => x.Alias == Alias);
            var commentvm = await commemt.Select(x => new CommentVm()
            {
                Content = x.Content,
                IdProduct = x.IdProduct,
                UserName = x.UserName,
                Review=x.Review
                
            }).ToListAsync();



            var relateProduct = await RelatedProduct(product.IdCategory, IdProduct);

            var MaybeLikes = await MaybeLike(product.IdBrand, IdProduct);

            var a = new ProductDetailsVm()
            {
                IdProduct = product.IdProduct,
                DateAccept = product.DateAccept,
                IdBrand = product.IdBrand,
                ProductName = product.ProductName,
                UseVoucher = product.UseVoucher,
                IdCategory = product.IdCategory,
                PhotoReview = product.PhotoReview,
                ListPhotos = linkPhotos,
                RelatedProducts = relateProduct,
                Price = product.Price,
                PriceExport = product.PriceExport,
                Comments = commentvm,
                Description = product.Description,
                MaybeLike = MaybeLikes,
                Alias = product.Alias
            };
            return a;
        }

        public async Task<IPagedList<ProductVm>> Filters(int pricemin, int pricemax, int? page)
        {
            var a = _iden2Context.Products.Where(x => x.Price >= pricemin && x.Price <= pricemax);

            var pageNumber = page ?? 1;
            int pageSize = 9;

            var b = await a.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                PhotoReview = x.PhotoReview,
                IdCategory = x.IdCategory,
                PriceExport = x.PriceExport,
                IsFree = x.IsFree,
                Price = x.Price,
                Alias = x.Alias
            }).ToPagedListAsync(pageNumber, pageSize);
            return b;

        }

        public async Task<int> AddComment(string Alias, string Content,int Review)
        {
            var User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string UserName = User.UserName;
            var comment = new Comment()
            {
                Status = false,
                Content = Content,
                DatePost = DateTime.Now,
                Alias = Alias,
                UserName = UserName,
                Review=Review
                
            };
            _iden2Context.Comments.Add(comment);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<IPagedList<ProductVm>> GetProductPerBrand(int IdBrand, int? page)
        {
            var x = _iden2Context.Products.Where(x => x.IdBrand == IdBrand);

            var pageNumber = page ?? 1;
            int pageSize = 9;

            var a = await x.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                PhotoReview = x.PhotoReview,
                IdBrand = x.IdBrand,
                IdCategory = x.IdCategory,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias
            }).ToPagedListAsync(pageNumber, pageSize);
            return a;
        }

        public async Task<List<ProductVm>> RelatedProduct(int IdCategory, int IdProduct)
        {
            var x = _iden2Context.Products.Where(x => x.IdCategory == IdCategory && x.IdProduct != IdProduct);
            var a = await x.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                PhotoReview = x.PhotoReview,
                IdBrand = x.IdBrand,
                IdCategory = x.IdCategory,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias
            }).ToListAsync();
            return a;
        }

        public async Task<List<ProductVm>> MaybeLike(int IdBrand, int IdProduct)
        {
            var x = _iden2Context.Products.Where(x => x.IdBrand == IdBrand && x.IdProduct != IdProduct);
            var a = await x.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                PhotoReview = x.PhotoReview,
                IdBrand = x.IdBrand,
                IdCategory = x.IdCategory,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias
            }).ToListAsync();
            return a;
        }

        public async Task<IPagedList<ProductVm>> GetProductPerMutilpleBrandWithCategory(int IdCategory, int pricemin, int pricemax, int IdBrand1, int IdBrand2, int IdBrand3, int IdBrand4, int IdBrand5, int IdBrand6, int? page)
        {
           
            var pro = _iden2Context.Products.Where(x => x.IdCategory == IdCategory && x.IsShow==true);

            if (IdCategory == 0)
            {
                pro = _iden2Context.Products.Where(x=>x.IsShow==true);
            }
                
            if (IdBrand1 != 0 || IdBrand2 != 0 || IdBrand3 != 0 || IdBrand4 != 0 || IdBrand5 != 0 || IdBrand6 != 0)
            {
                pro = pro.Where(x => x.IdBrand == IdBrand1 || x.IdBrand == IdBrand2 || x.IdBrand == IdBrand3 || x.IdBrand == IdBrand4 || x.IdBrand == IdBrand5 || x.IdBrand == IdBrand6);
            }
            if (pricemax != 0)
            {
                pro = pro.Where(x => x.PriceExport >= pricemin && x.PriceExport <= pricemax);
            }
           
            var pageNumber = page ?? 1;
            int pageSize = 9;

            var a = await pro.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                PhotoReview = x.PhotoReview,
                IdBrand = x.IdBrand,
                IdCategory = x.IdCategory,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias
            }).ToPagedListAsync(pageNumber, pageSize);
            return a;
        }








        //public async Task<List<Photos>> UploadFiles(ProductCreateVm request)
        //{
        //    List<Photos> a = new List<Photos>();
        //    if (request.FileToUpload == null || request.FileToUpload.Count == 0)
        //    {
        //        return null;
        //    }

        //    foreach (var file in request.FileToUpload)
        //    {
        //        var path = Path.Combine("wwwroot", "Images", file.FileName);

        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //        var b = new Photos()
        //        {
        //            IdProduct = request.IdProduct,
        //            LinkImageFile = path.Substring(8)
        //        };
        //        a.Add(b);
        //        await _iden2Context.SaveChangesAsync();
        //    }
        //    return a;
        //}

        public ProductVm GetbyId(int IdProduct)
        {
            var x = _iden2Context.Products.FromSqlRaw("Select * from Products Where IdProduct=12").FirstOrDefault();
            ProductVm product = new ProductVm() 
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                PhotoReview = x.PhotoReview,
                IdBrand = x.IdBrand,
                IdCategory = x.IdCategory,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias
            };
            return product;
        }

        public async Task<int> ChangIsShow(int IdProduct)
        {
            var product = await _iden2Context.Products.Where(x => x.IdProduct == IdProduct).FirstOrDefaultAsync();
            if (product.IsShow == true)
            {
                product.IsShow = false;
            }
            else
            {
                product.IsShow = true;
            }
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> ChangeIsStandout(int IdProduct)
        {
            var product = await _iden2Context.Products.Where(x => x.IdProduct == IdProduct).FirstOrDefaultAsync();
            if (product.IsStandout == true)
            {
                product.IsStandout = false;
            }
            else
            {
                product.IsStandout = true;
            }
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<IPagedList<ProductVm>> GetProductPerSubCategory(int IdCategory, int? page)
        {
            var pageNumber = page ?? 1;
            int pageSize = 9;

            var x = _iden2Context.Products.Where(x => x.IdCategory == IdCategory);
            var a = await x.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                PhotoReview = x.PhotoReview,
                IdCategory = x.IdCategory,
                IsFree = x.IsFree,
                Price = x.Price,
                PriceExport = x.PriceExport,
                Alias = x.Alias
            }).ToPagedListAsync(pageNumber, pageSize);
            return a;
        }
    }
}
