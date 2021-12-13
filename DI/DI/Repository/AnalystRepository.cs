using Data.Data;
using Data.DB;
using Data.Enums;
using Data.Utilities.Constants;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Repository
{
    public class AnalystRepository : IAnalystRepository
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Iden2Context _iden2Context;
        public AnalystRepository(Iden2Context iden2Context,IHttpContextAccessor httpContextAccessor)
        {
            _iden2Context = iden2Context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<List<OrdersVm>> GetALlProcessOrdersPerDay(DateTime date)
        {
            var get = _iden2Context.Orders.Where(x=>x.Status== Status.Process && x.OrderDay.DayOfYear==date.DayOfYear);
            var c = await get.Select(x => new OrdersVm()
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                UserName = x.IdUser,
                AddressShip = "",
                NameShip = "",
                EmailShip = "",
                NoticeShip = "",
                NumberShip = "",
                VoucherCode = "",
                OrderDay = x.OrderDay,
                TotalPice = x.TotalPice
            }).ToListAsync();
            return c;
        }

        public async Task<List<OrdersVm>> GetALlProcessOrdersPerMonthDetails(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);
            var orderpermonth = _iden2Context.Orders.Where(x => x.OrderDay.Month == month2 && x.OrderDay.Year == year2 &&x.Status== Status.Process);

            var c = await orderpermonth.Select(x => new OrdersVm()
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                UserName = x.IdUser,
                AddressShip="",
                NameShip="",
                EmailShip="",
                NoticeShip="",
                NumberShip="",
                VoucherCode="",
                OrderDay = x.OrderDay,
                TotalPice = x.TotalPice
            }).ToListAsync();

            return c;
        }

        public async Task<List<OrdersVm>> GetOrdersPerDay(DateTime date)
        {
            var get = _iden2Context.Orders.Where(x => x.OrderDay.DayOfYear == date.DayOfYear);
            var x = await get.Select(x => new OrdersVm() 
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                UserName = x.IdUser,
                AddressShip = "",
                NameShip = "",
                EmailShip = "",
                NoticeShip = "",
                NumberShip = "",
                VoucherCode = "",
                OrderDay = x.OrderDay,
                TotalPice = x.TotalPice
            }).ToListAsync();
            return x;
        }



        public async Task<List<SlideVm>> GetSlide()
        {
            var x = _iden2Context.Slides.Where(x=>x.IsShow==true);
            var xx = await x.Select(x => new SlideVm() 
            { 
            SlideName=x.SlideName,
            FromFile=x.FromFile,
            Id=x.Id,
            Alias=x.Alias
            }).ToListAsync();
            return xx;
        }



        public async Task<List<QuantityProducts>> GetTotalQuantityProducts()
        {
            var orderDetails = _iden2Context.OrderDetails;
            var product = _iden2Context.Products;

            var k = from p in orderDetails
                   join  pt in product on p.IdProduct equals pt.IdProduct
                    select new { p, pt };

            var x = await k.Select(x=>new QuantityProducts()
            {
            IdProduct=x.pt.IdProduct,
            TotalQuantity=x.p.Quality*x.p.Price,
            IFromFile=x.pt.PhotoReview,
            Price=x.p.Price,
            Description=x.pt.Description,
            ProductName=x.pt.ProductName
            }).ToListAsync();

            List<int> Id = await product.Select(x => x.IdProduct).ToListAsync();

            List<QuantityProducts> a = new List<QuantityProducts>();

            for(int i = 0; i < Id.Count(); i++)
            {
                var c = new QuantityProducts()
                {
                    IdProduct = Id[i],
                    TotalQuantity = 0
                };
                a.Add(c);
            }
            for(int i = 0; i < a.Count(); i++)
            {
                for(int j = 0; j < x.Count(); j++)
                {
                    if (a[i].IdProduct == x[j].IdProduct)
                    {
                        a[i].TotalQuantity += x[j].TotalQuantity;
                        a[i].IFromFile = x[j].IFromFile;
                        a[i].Price = x[j].Price;
                        a[i].Description = x[j].Description;
                        a[i].ProductName = x[j].ProductName;
                    }
                }
            }
            var aa = a.OrderByDescending(x => x.TotalQuantity).ToList();
            return aa;
        }

        public async Task<List<ToTalRevenue>> GetRevenueMonth(string month, string year)
        {
            int month2=int.Parse(month);
            int year2 = int.Parse(year);

            var order = _iden2Context.Orders.Where(x => x.OrderDay.Month == month2 && x.OrderDay.Year == year2);

            var ordercount = order.GroupBy(x => x.OrderDay.Day)
                .Select(x => new
                {
                    key = x.Key,
                    countx = x.Count(),
                    sum = x.Sum(order => order.TotalPice)
                });

            var renevue = from p in order
                          join pt in ordercount on p.OrderDay.Day equals pt.key
                          select new { p, pt };

            var x =await ordercount.Select(x => new ToTalRevenue() 
            {
            Day=x.key,
            TotalOrder=x.countx,
            TotalPrice=x.sum
            }).ToListAsync();

            var xx = new List<ToTalRevenue>();

            if (x.Count() < 31)
            {
                int j = 0;
                for(int i=1;i<=31;i++)
                {
                    if (x[j].Day == i )
                    {
                        var newmodel = new ToTalRevenue()
                        {
                            Day = x[j].Day,
                            TotalOrder = x[j].TotalOrder,
                            TotalPrice = x[j].TotalPrice
                        };
                        j++;
                        xx.Add(newmodel);                      
                    }
                    else
                    {
                        var newmodel = new ToTalRevenue()
                        {
                            Day = i,
                            TotalOrder = 0,
                            TotalPrice = 0
                        };
                        xx.Add(newmodel);
                    }
                }
            }
           
            return xx;
        }

        public async Task<List<QuantityProducts>> GetTotalQuantityProductsPerDay(string day, string month, string year)
        {
            int day2 = int.Parse(day);
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);

            var order = from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        select new { p, pt };

            var order2 = order.Where(x => x.p.OrderDay.Month == month2 &&x.p.OrderDay.Day==day2 && x.p.OrderDay.Year == year2);

            var standout = order2
                .GroupBy(x => x.pt.IdProduct)
                .Select(x => new
                {
                    key = x.Key,
                    Countx = x.Count()
                })
                .OrderByDescending(x => x.Countx);



            var products = from p in standout
                           join pt in _iden2Context.Products on p.key equals pt.IdProduct
                           select new { p, pt };

            var x = await products.Select(x => new QuantityProducts()
            {
                IdProduct = x.pt.IdProduct,
                TotalQuantity = x.p.Countx,
                ProductName = x.pt.ProductName,
                Price = x.pt.Price,
                Description = "",
                IFromFile = ""
            }).ToListAsync();

            if (x.Count() < 10)
            {
                while (x.Count() <= 9)
                {
                    QuantityProducts qua = new QuantityProducts()
                    {
                        IdProduct = 0,
                        TotalQuantity = 0,
                        ProductName = "",
                        Price = 0,
                        Description = "",
                        IFromFile = ""
                    };
                    x.Add(qua);
                }
            }
            return x;
        }

        public async Task<List<QuantityProducts>> GetTotalQuantityProductsPerMonth(string month, string year)
            {
                int month2 = int.Parse(month);
                int year2 = int.Parse(year);

            var order = from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        select new { p, pt };

            var order2 = order.Where(x => x.p.OrderDay.Month == month2 && x.p.OrderDay.Year == year2);

            var standout = order2
                .GroupBy(x => x.pt.IdProduct)
                .Select(x => new
                {
                    key = x.Key,
                    Countx = x.Count()
                })
                .OrderByDescending(x => x.Countx);

    

            var products = from p in standout
                           join pt in _iden2Context.Products on p.key equals pt.IdProduct
                           select new { p, pt };

            var x = await products.Select(x => new QuantityProducts()
            {
                IdProduct=x.pt.IdProduct,
                TotalQuantity=x.p.Countx,
                ProductName=x.pt.ProductName,
                Price=x.pt.Price,
                Description="",
                IFromFile=""
            }).ToListAsync();

            if (x.Count() < 10)
            {
                while (x.Count() <= 9)
                {
                    QuantityProducts qua = new QuantityProducts()
                    {
                        IdProduct = 0,
                        TotalQuantity = 0,
                        ProductName = "",
                        Price = 0,
                        Description = "",
                        IFromFile = ""
                    };
                    x.Add(qua);
                }
            }
            return x;
        }

        public async Task<List<QuantityProducts>> GetTotalQuantityProductsPerYear(string year)
        {
            int year2 = int.Parse(year);

            var order = from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        select new { p, pt };

            var order2 = order.Where(x => x.p.OrderDay.Year == year2);

            var standout = order2
                .GroupBy(x => x.pt.IdProduct)
                .Select(x => new
                {
                    key = x.Key,
                    Countx = x.Count()
                })
                .OrderByDescending(x => x.Countx);



            var products = from p in standout
                           join pt in _iden2Context.Products on p.key equals pt.IdProduct
                           select new { p, pt };

            var x = await products.Select(x => new QuantityProducts()
            {
                IdProduct = x.pt.IdProduct,
                TotalQuantity = x.p.Countx,
                ProductName = x.pt.ProductName,
                Price = x.pt.Price,
                Description = "",
                IFromFile = ""
            }).ToListAsync();

            if (x.Count() < 10)
            {
                while (x.Count() <= 9)
                {
                    QuantityProducts qua = new QuantityProducts()
                    {
                        IdProduct = 0,
                        TotalQuantity = 0,
                        ProductName = "",
                        Price = 0,
                        Description = "",
                        IFromFile = ""
                    };
                    x.Add(qua);
                }
            }
            return x;
        }

        public async Task<List<ProductVm>> TopNew()
        {
            var products = _iden2Context.Products.Where(x => x.IsShow == true);


            var a = await products.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                PhotoReview = x.PhotoReview,
                Price = x.Price,
                Alias=x.Alias,
            }).OrderByDescending(x => x.IdProduct).ToListAsync();
            return a;
        }

        public async Task<List<ProductVm>> TopSell()
        {
          
            var top2 = _iden2Context.OrderDetails
                .GroupBy(x => x.IdProduct)
                .Select(x => new
                {
                    key = x.Key,
                    Countx = x.Count()
                }
                ).OrderByDescending(x => x.Countx);

            var products = from p in top2
                           join pt in _iden2Context.Products on p.key equals pt.IdProduct 
                           select new { p, pt };

            var productvm = await products.Select(x => new ProductVm()
            {
                IdProduct = x.pt.IdProduct,
                DateAccept = x.pt.DateAccept,
                IdBrand = x.pt.IdBrand,
                ProductName = x.pt.ProductName,
                UseVoucher = x.pt.UseVoucher,
                PhotoReview = x.pt.PhotoReview,
                IdCategory = x.pt.IdCategory,
                IsFree = x.pt.IsFree,
                Price = x.pt.Price,
                Alias = x.pt.Alias,
            }).ToListAsync();
            return productvm;
        }

        public async Task<List<ProductVm>> TopStandOut()
        {
            var products = _iden2Context.Products.Where(x => x.IsStandout == true);


            var a = await products.Select(x => new ProductVm()
            {
                IdProduct = x.IdProduct,
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
                PhotoReview = x.PhotoReview,
                Price = x.Price,
                Alias = x.Alias,
            }).OrderByDescending(x => x.IdProduct).ToListAsync();
            return a;
        }

        public async Task<List<OrdersVm>> GetALlDeliveredOrdersPerDay(DateTime date)
        {
            var get = _iden2Context.Orders.Where(x => x.Status == Status.Delivering && x.OrderDay.DayOfYear == date.DayOfYear);
            var c = await get.Select(x => new OrdersVm()
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                UserName = x.IdUser,
                AddressShip = "",
                NameShip = "",
                EmailShip = "",
                NoticeShip = "",
                NumberShip = "",
                VoucherCode = "",
                OrderDay = x.OrderDay,
                TotalPice = x.TotalPice
            }).ToListAsync();
            return c;
        }

        public string GetAccess()
        {

            var session = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return session;
        }

        public async Task<int> IPAccess()
        {
            var date = DateTime.Now;

            var Ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var x = await _iden2Context.IpAccesses.Where(x => x.IPAddress == Ip && x.DateAccess.DayOfYear==date.DayOfYear   ).FirstOrDefaultAsync();

            if (x == null)
            {
                IpAccess IPtb = new IpAccess()
                {
                    IPAddress = Ip,
                    CountAcess = 1,
                    DateAccess = date
                };
                _iden2Context.IpAccesses.Add(IPtb);
            }
            else
            {
                x.CountAcess = x.CountAcess + 1;
            }

            return await _iden2Context.SaveChangesAsync();
        }

        public  int IPAccessDay()
        {
            var x = _iden2Context.IpAccesses.Where(x => x.DateAccess == DateTime.Now);
            var xx = x.Count();
            return xx;
        }

        public int TotalAccess()
        {
            var x = _iden2Context.IpAccesses.Where(x => x.DateAccess == DateTime.Now);
            var xx = x.Sum(x => x.CountAcess);
            return xx;
        }

        //public TotalnameIp ListNameIp()
        //{
        //    var ipaccess = _iden2Context.IpAccesses;


        //    var ipname2 = ipaccess.GroupBy(x => x.IPAddress)
        //       .Select(x => new
        //       {
        //           key = x.Key,
        //           sum = x.Sum(ipaccess => ipaccess.CountAcess)
        //       });

        //    var x = ipname2.Select(x => new NameIp() 
        //    { 
        //    Name=x.key,
        //    SoLuong=x.sum         
        //    }).ToList(); 

        //    var k = new TotalnameIp();
        //    k.StatusIP = x;
        //    k.Total = x.Sum(x => x.SoLuong);
        //    return k;

        //}
    }
}



