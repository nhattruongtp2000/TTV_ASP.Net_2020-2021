using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Interace
{
    public interface IVoucherRepository
    {
        Task<int> CreateNewVoucher(VoucherVm x);

        Task<List<VoucherVm>> GetAllVoucher();

        Task<int> Edit(VoucherVm request);

        Task<int> Delete(int IdVoucher);

        Task<List<VoucherVm>> SearchVoucher(int IdVoucher);

        Task<VoucherVm> GetOneVoucher(int IdVoucher);
    }
}
