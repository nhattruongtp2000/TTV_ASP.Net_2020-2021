using Data.Data;
using Data.DB;
using DI.DI.Interace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Repository
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly Iden2Context _iden2Context;

        public VoucherRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }


        public async Task<int> CreateNewVoucher(VoucherVm x)
        {
            var voucher = new Voucher()
            {
                VoucherCode = x.VoucherCode,
                VoucherName = x.VoucherName,
                Status = x.Status,
                ApplyForAll = x.ApplyForAll,
                DiscountAmount = x.DiscountAmount,
                DiscountPercent = x.DiscountPercent,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                TypeVoucher = x.TypeVoucher,
                Quantity=x.Quantity
            };
             _iden2Context.Add(voucher);
            return await _iden2Context.SaveChangesAsync();

        }

        public async Task<int> Delete(int IdVoucher)
        {
            var voucher = await _iden2Context.Vouchers.FirstOrDefaultAsync(x => x.Id == IdVoucher);
            _iden2Context.Vouchers.Remove(voucher);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<int> Edit(VoucherVm request)
        {
            var voucher = await _iden2Context.Vouchers.Where(x => x.Id == request.IdVoucher).FirstOrDefaultAsync();

            voucher.VoucherCode = request.VoucherCode;
            voucher.VoucherName = request.VoucherName;
            voucher.Status = request.Status;
            voucher.ApplyForAll = request.ApplyForAll;
            voucher.DiscountAmount = request.DiscountAmount;
            voucher.DiscountPercent = request.DiscountPercent;
            voucher.FromDate = request.FromDate;
            voucher.ToDate = request.ToDate;
            voucher.TypeVoucher = request.TypeVoucher;
            voucher.Quantity = request.Quantity;

            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<List<VoucherVm>> GetAllVoucher()
        {
            var vouchers = _iden2Context.Vouchers;
            var vouchervm = await vouchers.Select(x => new VoucherVm() 
            {
                IdVoucher=x.Id,
            VoucherCode=x.VoucherCode,
            VoucherName=x.VoucherName,
            Status=x.Status,
            ApplyForAll=x.ApplyForAll,
            DiscountAmount=x.DiscountAmount,
            DiscountPercent=x.DiscountPercent,
            FromDate=x.FromDate,
            ToDate=x.ToDate,
            TypeVoucher=x.TypeVoucher,
                Quantity = x.Quantity
            }).ToListAsync();

            return vouchervm;
        }

        public async Task<VoucherVm> GetOneVoucher(int IdVoucher)
        {
            var vouchers =await _iden2Context.Vouchers.FindAsync(IdVoucher);
            var vouchervm = new VoucherVm()
            {
                IdVoucher=vouchers.Id,
                VoucherCode = vouchers.VoucherCode,
                VoucherName = vouchers.VoucherName,
                Status = vouchers.Status,
                ApplyForAll = vouchers.ApplyForAll,
                DiscountAmount = vouchers.DiscountAmount,
                DiscountPercent = vouchers.DiscountPercent,
                FromDate = vouchers.FromDate,
                ToDate = vouchers.ToDate,
                TypeVoucher = vouchers.TypeVoucher,
                Quantity = vouchers.Quantity
            };

            return vouchervm;
        }

        public Task<List<VoucherVm>> SearchVoucher(int IdVoucher)
        {
            throw new NotImplementedException();
        }
    }
}
