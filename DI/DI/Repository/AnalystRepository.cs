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
            var get = _iden2Context.Orders.Where(x=>x.Status== Status.Process && x.OrderDay.Date==date.Date);
            var c = await get.Select(x => new OrdersVm()
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                UserName = x.IdUser,
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
                VoucherCode="",
                OrderDay = x.OrderDay,
                TotalPice = x.TotalPice
            }).ToListAsync();

            return c;
        }

        public async Task<List<OrdersVm>> GetOrdersPerDay(DateTime date)
        {
            var get = _iden2Context.Orders.Where(x => x.OrderDay.Date == date.Date);
            var x = await get.Select(x => new OrdersVm() 
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                UserName = x.IdUser,
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

        public async Task<List<QuantityProducts>> GetTotalQuantityProductsPerDay(DateTime date)
        {


            var order = from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct
                        select new { p, pt, ptt };

            var order2 = order.Where(x => x.p.OrderDay.Date==date.Date);

            var standout = await order2
                .GroupBy(x => x.ptt.ProductName)
                .Select(x => new QuantityProducts
                {
                    ProductName = x.Key,
                    TotalQuantity = x.Count(),
                    Price = x.Sum(x => x.pt.Price)
                })
                .OrderByDescending(x => x.Price).ToListAsync();



            return standout;
        }

        public async Task<List<QuantityProducts>> GetTotalQuantityProductsPerMonth(string month, string year)
            {
                int month2 = int.Parse(month);
                int year2 = int.Parse(year);

            var order = from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct
                        select new { p, pt,ptt};

            var order2 = order.Where(x => x.p.OrderDay.Month == month2 && x.p.OrderDay.Year == year2);

            var standout =await order2
                .GroupBy(x => x.ptt.ProductName)
                .Select(x => new QuantityProducts
                {
                    ProductName = x.Key,
                    TotalQuantity = x.Count(),
                    Price=x.Sum(x=>x.pt.Price)
                })
                .OrderByDescending(x => x.Price).ToListAsync();



            return standout;
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
                TotalQuantity = x.p.Countx,
                ProductName = x.pt.ProductName,
                Price = x.pt.Price,
            }).ToListAsync();

            if (x.Count() < 10)
            {
                while (x.Count() <= 9)
                {
                    QuantityProducts qua = new QuantityProducts()
                    {
                        TotalQuantity = 0,
                        ProductName = "",
                        Price = 0,
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
                PriceExport=x.PriceExport,
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
                PriceExport=x.pt.PriceExport,
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
                PriceExport=x.PriceExport,
                Price = x.Price,
                Alias = x.Alias,
            }).OrderByDescending(x => x.IdProduct).ToListAsync();
            return a;
        }

        public async Task<List<OrdersVm>> GetALlDeliveredOrdersPerDay(DateTime date)
        {
            var get = _iden2Context.Orders.Where(x => x.Status == Status.Delivering && x.OrderDay.Date == date.Date);
            var c = await get.Select(x => new OrdersVm()
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                UserName = x.IdUser,
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
            var x = _iden2Context.IpAccesses.Where(x => x.DateAccess.Date == DateTime.Now.Date);
            var xx = x.Count();
            return xx;
        }

        public int TotalAccess()
        {
            var x = _iden2Context.IpAccesses.Where(x => x.DateAccess == DateTime.Now);
            var xx = x.Sum(x => x.CountAcess);
            return xx;
        }

        public async Task<List<RevenueBrandVm>> RevenuePerBrands(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);


            var orderdetais = (from p in _iden2Context.Products
                              join pt in _iden2Context.OrderDetails on p.IdProduct equals pt.IdProduct
                              join ptt in _iden2Context.Brands on p.IdBrand equals ptt.IdBrand
                              join pttt in _iden2Context.Orders on pt.IdOrder equals pttt.IdOrder
                              select new { p, pt, ptt,pttt }).Where(x => x.pttt.OrderDay.Month == month2 && x.pttt.OrderDay.Year == year2);

            var brandrevenue =await orderdetais.GroupBy(x => x.ptt.BrandName)
                .Select(x => new RevenueBrandVm
                {
                    BrandName=x.Key,
                    TotalRevenue=x.Sum(dd=>dd.pt.Price)
                }).ToListAsync();

            return brandrevenue;

        }

        public async Task<List<QuantityBrandVm>> QuantityPerBrand(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);


            var orderdetais = (from p in _iden2Context.Products
                               join pt in _iden2Context.OrderDetails on p.IdProduct equals pt.IdProduct
                               join ptt in _iden2Context.Brands on p.IdBrand equals ptt.IdBrand
                               join pttt in _iden2Context.Orders on pt.IdOrder equals pttt.IdOrder
                               select new { p, pt, ptt, pttt }).Where(x => x.pttt.OrderDay.Month == month2 && x.pttt.OrderDay.Year == year2);

            var brandquantity = await orderdetais.GroupBy(x => x.ptt.BrandName)
                .Select(x => new QuantityBrandVm
                {
                    BrandName = x.Key,
                    Quantity = x.Sum(dd => dd.pt.Quality)
                }).ToListAsync();

            return brandquantity;
        }

        public async Task<List<RevenueMonthVm>> RevenuePerMonth(string month, string year)
        {

            int month2 = int.Parse(month);
            int year2 = int.Parse(year);

            var order = _iden2Context.Orders.Where(x => x.OrderDay.Month == month2 && x.OrderDay.Year == year2);


            var permonth =await order
                .GroupBy(x => x.OrderDay.Day)
                .Select(x => new RevenueMonthVm
                {
                    Day = x.Key,
                    Revenue = x.Sum(re => re.TotalPice)
                })
                .OrderBy(x => x.Day)
                .ToListAsync();

            var k = new RevenueMonthVm()
            {
                Day = 32,
                Revenue = 0
            };
            permonth.Add(k);



            
            return permonth;
        }

        public int OrdersMonth(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);

            var order = _iden2Context.Orders.Where(x => x.OrderDay.Month == month2 && x.OrderDay.Year == year2);

            return order.Count() ;
        }

        public  int ProductSold(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);
            var sold = (from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        select new { p, pt }).Where(x => x.p.OrderDay.Month == month2 && x.p.OrderDay.Year == year2);

            var sold2 =  sold.Sum(x => x.pt.Quality);

            return sold2;

        }

        public decimal TotalRevenueMonth(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);

            var order = _iden2Context.Orders.Where(x => x.OrderDay.Month == month2 && x.OrderDay.Year == year2);
            var sum = order.Sum(x => x.TotalPice);
            return sum;

        }

        public int OrdersDay(DateTime date)
        {

            var order = _iden2Context.Orders.Where(x => x.OrderDay.Date==date.Date);

            return order.Count();
        }

        public int ProductDay(DateTime date)
        {

            var sold = (from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        select new { p, pt }).Where(x => x.p.OrderDay == date);

            var sold2 = sold.Sum(x => x.pt.Quality);

            return sold2;
        }
        public decimal TotalRevenueDay(DateTime date)
        {           
            var order = _iden2Context.Orders.Where(x => x.OrderDay == date);
            var sum = order.Sum(x => x.TotalPice);
            return sum;
        }

        public List<AnalystAccessVm> AnalystAccessMonth(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);

            var access = _iden2Context.IpAccesses.Where(x => x.DateAccess.Month == month2 && x.DateAccess.Year == year2);
            var analystaccess = access.GroupBy(x => x.DateAccess.Day)
                .Select(x => new AnalystAccessVm() { 
                
                    Day=x.Key,
                    TotalAccess=x.Sum(x=>x.CountAcess)
                }).ToList();

            analystaccess.Add(new AnalystAccessVm() { 
            Day=32,
            TotalAccess=0
            });
            return analystaccess;
        }

        public async Task<List<ProductExportVm>> ProfitMonth(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);

            var order = from p in _iden2Context.Orders
                        join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                        join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct
                        select new { p, pt, ptt };

            var order2 = order.Where(x => x.p.OrderDay.Month == month2 && x.p.OrderDay.Year == year2);

            var standout = order2
                .GroupBy(x => x.ptt.IdProduct)
                .Select(x => new
                {
                    IdProduct = x.Key,
                    TotalQuantity = x.Count(),
                    Revenue = x.Sum(x => x.pt.Price),
                });

            var profit = from p in _iden2Context.Products
                         join pt in standout on p.IdProduct equals pt.IdProduct
                         join ptt in _iden2Context.OrderDetails on p.IdProduct equals ptt.IdProduct
                         select new { p, pt, ptt };

            var profitvm =await profit.Select(x => new ProductExportVm() { 
            IdProduct=x.p.IdProduct,
            IdOrder=x.ptt.IdOrder,
            PriceExport=x.p.PriceExport,
            PriceImport=x.p.Price,
            ProductName=x.p.ProductName,
            Profit=x.p.PriceExport*x.pt.TotalQuantity-x.p.Price*x.pt.TotalQuantity,
            Quantity=x.pt.TotalQuantity,
            Revenue=x.pt.Revenue
            }).ToListAsync();
            


            return profitvm;
        }

        public decimal? TotalPriceVoucher()
        {
            var x = _iden2Context.Orders.Where(x => x.VoucherCode != null);
            var query = from p in _iden2Context.Vouchers
                        join pt in x on p.VoucherCode equals pt.VoucherCode
                        select new { p, pt };

            decimal? total = query.Sum(x => x.p.DiscountAmount);
            return total;
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



