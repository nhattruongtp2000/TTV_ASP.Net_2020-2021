using ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.DI.Interace
{
    public interface IAnalystRepository
    {

        string GetAccess();

        //
        Task<List<QuantityProducts>> GetTotalQuantityProductsPerMonth(string month, string year);

        Task<List<QuantityProducts>> GetTotalQuantityProductsPerDay(string day,string month, string year);

        Task<List<QuantityProducts>> GetTotalQuantityProductsPerYear(string year);

        Task<List<QuantityProducts>> GetTotalQuantityProducts();
        //
        

        Task<List<OrdersVm>> GetOrdersPerDay(DateTime date);

        Task<List<OrdersVm>> GetALlProcessOrdersPerDay(DateTime date);

        Task<List<OrdersVm>> GetALlDeliveredOrdersPerDay(DateTime date);

        Task<List<OrdersVm>> GetALlProcessOrdersPerMonthDetails(string month, string year);


        //

        Task<List<SlideVm>> GetSlide();

        Task<List<ProductVm>> TopSell();

        Task<List<ProductVm>> TopNew();

        Task<List<ProductVm>> TopStandOut();
        //

        Task<List<ToTalRevenue>> GetRevenueMonth(string month, string year);

        //

        Task<int> IPAccess();

        int IPAccessDay();

        int TotalAccess();

        


    }
}
