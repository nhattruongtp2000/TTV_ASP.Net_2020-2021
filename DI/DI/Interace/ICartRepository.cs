using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Interace
{
    public interface ICartRepository
    {
        void AddtoCart(int IdProduct);

        void UpdateCart(int IdProduct, int Quantity);

        void RemoveCart(int IdProduct);

        void ClearCart();

        void SaveCart(List<CartItem> ls);

        List<CartItem> GetCartItems();

        Task<List<ProductVm>> GetAll();

        Task<string> Purchase(string IdOrder,string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, decimal total,string voucherCode);

        //Task<List<OrderDetailsVm>> Checkout(string IdUser);

        Task<string> PayPal(double total);

        Task<string> CheckoutSuccess(decimal total);

        Task<string> CheckoutFail();

        string VNpay(string OrderCategory,decimal Amount,string txtOrderDesc,string cboBankCode, string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, string voucherCode);

        Task<VNPayReturnVm> VNPayReturn(  string voucherCode);
        
    }
}
