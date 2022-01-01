    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Interace
{
    public interface IOrderRepository
    {
        Task<IPagedList<OrdersVm>> GetAll(int? page);

        Task<OrderDetailsUser> GetDetails(string IdOrder);

        Task<int> ChangeStatusDetails(string IdOrder,string x);

        Task<string> GetStatus(string IdOrder);

        Task<IPagedList<OrdersVm>> OrderHistory(string IdUser, int? page);

        Task<OrdersVm> SearchOrder(string IdOrder,string IdUser);

        Task<List<OrdersVm>> GetAllToExcel();
    }
}
