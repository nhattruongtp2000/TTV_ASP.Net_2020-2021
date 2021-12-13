using Data.DB;
using Data.Enums;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Repository
{
    public class OrderRepository : IOrderRepository
    {

        private readonly Iden2Context _iden2Context;

        public OrderRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }

        public async Task<int> ChangeStatusDetails(string IdOrder,string x)
        {

            var change = await _iden2Context.Orders.FirstOrDefaultAsync((x => x.IdOrder == IdOrder ));
              if(x== "Process")
                {
                    change.Status = Status.Process;
                }
                else if (x == "Complete")
                {
                    change.Status = Status.Complete;
                }
                else
                {
                    change.Status = Status.Delivering;
                }
                return await _iden2Context.SaveChangesAsync();

        }

        

        public async Task<IPagedList<OrdersVm>> GetAll(int? page)
        {
            var orders = from p in _iden2Context.Orders
                         join pt in _iden2Context.Users on p.IdUser equals pt.Id
                         select new { p,pt};

           

            //join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
            //join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct

            var pageNumber = page ?? 1;
            int pageSize = 10;

            var c =await orders.Select(x => new OrdersVm() 
            { 
            IdOrder=x.p.IdOrder,
            UserName=x.pt.UserName,
            EmailShip=x.p.EmailShip,
            AddressShip=x.p.AddressShip,
            NameShip=x.p.NameShip,
            NoticeShip=x.p.NoticeShip,
            NumberShip=x.p.NumberShip,
            Status=x.p.Status,
            OrderDay=x.p.OrderDay,
            TotalPice=x.p.TotalPice,
            VoucherCode=x.p.VoucherCode
            }).ToPagedListAsync(pageNumber,pageSize);
            return c;
        }

        public async Task<List<OrderDetailsVm>> GetDetails(string IdOrder)
        {
            var query = _iden2Context.OrderDetails.Where(x => x.IdOrder == IdOrder);

            var orderdetais = from p in query
                              join pt in _iden2Context.Products on p.IdProduct equals pt.IdProduct
                              join ptt in _iden2Context.Orders on p.IdOrder equals ptt.IdOrder
                              select new { p, pt ,ptt };
            var x = await orderdetais.Select(x => new OrderDetailsVm() 
            {
            IdOrder=x.p.IdOrder,
            ProductName=x.pt.ProductName,
            PhotoReview=x.pt.PhotoReview,
            Price=x.pt.Price,
            Quality=x.p.Quality,
            Status=x.ptt.Status
            }).ToListAsync();
            return x;
        }

        public async Task<string> GetStatus(string IdOrder)
        {
            var x = await _iden2Context.Orders.Where(x => x.IdOrder == IdOrder).FirstOrDefaultAsync();
            return x.Status.ToString();
        }

        public async Task<IPagedList<OrdersVm>> OrderHistory(string IdUser, int? page)
        {
            var user =await _iden2Context.Users.Where(x => x.UserName == IdUser).FirstOrDefaultAsync();

            var k = _iden2Context.Orders.Where(x => x.IdUser == user.Id);

            var orders = from p in k
                         join pt in _iden2Context.Users on p.IdUser equals pt.Id
                         select new { p, pt };
            //join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
            //join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct

            var pageNumber = page ?? 1;
            int pageSize = 10;

            var c = await orders.Select(x => new OrdersVm()
            {
                IdOrder = x.p.IdOrder,
                UserName = x.p.IdUser,
                EmailShip = "",
                AddressShip = "",
                NameShip = "",
                OrderType=x.p.PaymentType,
                NoticeShip = "",
                NumberShip = "",
                Status = x.p.Status,
                OrderDay = x.p.OrderDay,
                TotalPice = x.p.TotalPice,
                VoucherCode = x.p.VoucherCode
            }).ToPagedListAsync(pageNumber, pageSize);
            return c;
        }

        public async Task<OrdersVm> SearchOrder(string IdOrder, string IdUser)
        {
            

            var k = await _iden2Context.Orders.Where(x => x.IdUser == IdUser && x.IdOrder == IdOrder).FirstOrDefaultAsync() ;

            //join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
            //join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct

            var ordervm = new OrdersVm()
            {
                IdOrder = k.IdOrder,
                OrderDay = k.OrderDay,
                TotalPice = k.TotalPice,
                VoucherCode = k.VoucherCode,
                OrderType = k.PaymentType,
                Status = k.Status


            };
            return ordervm;


        }
    }
}
