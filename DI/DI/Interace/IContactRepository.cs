using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Interace
{
    public interface IContactRepository
    {
        Task<ContactVm> GetContact();

        Task<int> Feedback(string UserName, string PhoneNumber, string Email, string Content);

        Task<int> SendEmailPromotion(string Email);

        string SendOrderReceived(string IdOrder, string EmailShip, string NameShip, string AddressShip, decimal Total, string TypePayment);

        Task<string> SendOrderDeliveried(string IdOrder);

        Task<string> SendOrderCompleted(string IdOrder);
    }
}
