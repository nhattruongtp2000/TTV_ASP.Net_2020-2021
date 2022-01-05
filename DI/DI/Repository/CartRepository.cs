using BraintreeHttp;
using Data.Data;
using Data.DB;
using Data.Utilities.Constants;
using DI.DI.Interace;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PayPal.Core;
using PayPal.v1.Payments;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;


namespace DI.DI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly Iden2Context _iden2Context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IVoucherRepository _IvoucherRepository;
        private readonly IAnalystRepository _analystRepository;
        private readonly string _clientId;
        private readonly string _secretKey;
        private readonly IConfiguration _configuration;
        public CartRepository(Iden2Context iden2Context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , IConfiguration config, IVoucherRepository IvoucherRepository, IAnalystRepository analystRepository, IConfiguration configuration)
        {
            _iden2Context = iden2Context;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _clientId = "AV-BEqQ4nyfYnUsK6_tkEim9gvX_wWaldPQETeF8DqUg6StRBlikt07ap6efAfcUd477BY77DmZ-ZNMN";
            _secretKey = /*config["PaypalSettings:SecretKey"];         */  "ENt8I6wQejmZJpmoe10v1Ah-q8G16mBsCjpVcgQsvIHbhLmGDXiC9K-hDJFFqQbVhi9m427R3QUNqI27";
            _IvoucherRepository = IvoucherRepository;
            _analystRepository = analystRepository;
            _configuration = configuration;
        }

        public double TyGiaUSD = 23300;//store in Database
        public void AddtoCart(int IdProduct)
        {
            var product = _iden2Context.Products.Where(x => x.IdProduct == IdProduct).FirstOrDefault();
            Product gift = new Product(); //san pham de tang
            ProductVm giftVm = new ProductVm(); //View model
            if (product.UseVoucher == true) //nếu có đính kém quà tặng
            {
                gift = _iden2Context.Products.Where(k => k.IdProduct == product.IdProductGiveTo).FirstOrDefault();
                giftVm = new ProductVm()
                {
                    DateAccept = gift.DateAccept,
                    IdBrand = gift.IdBrand,
                    IdCategory = gift.IdCategory,
                    IdProduct = gift.IdProduct,
                    PhotoReview = gift.PhotoReview,
                    Price = gift.PriceExport,
                    ProductName = gift.ProductName,
                    UseVoucher = gift.UseVoucher,
                    isGift=gift.IsGift,
                };
            }
            ProductVm product2 = new ProductVm()
            {
                DateAccept = product.DateAccept,
                IdBrand = product.IdBrand,
                IdCategory = product.IdCategory,
                IdProduct = product.IdProduct,
                PhotoReview = product.PhotoReview,
                Price = product.PriceExport,
                ProductName = product.ProductName,
                UseVoucher = product.UseVoucher,
                isGift=product.IsGift,
                IdProductGiveTo = product.IdProductGiveTo
            };
            var cart = GetCartItems();
            var cartItems = cart.Find(x => x.Product.IdProduct == IdProduct);
            if (cartItems != null)
            {
                cartItems.Quantity++;
            }
            else
            {
                cart.Add(new CartItem() { Quantity = 1, Product = product2 });
            }
            if (product.UseVoucher == true)
            {
                cart.Add(new CartItem() { Quantity = 1, Product = giftVm });
            }
            SaveCart(cart);


        }

        public void ClearCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove(SystemConstants.AppSettings.CARTKEY);
        }


        public void RemoveCart(int IdProduct)
        {
            var cart = GetCartItems();
            var cartItem = cart.Find(x => x.Product.IdProduct == IdProduct);
   
            if (cartItem.Product.IdProductGiveTo != 0)
            {
                var CartItemGift = cart.Find(x => x.Product.IdProduct == cartItem.Product.IdProductGiveTo);
                cart.Remove(CartItemGift);
            }
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }
            SaveCart(cart);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        public void SaveCart(List<CartItem> ls)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(SystemConstants.AppSettings.CARTKEY, jsoncart);
        }

        public void UpdateCart(int IdProduct, int Quantity)
        {
            var carts = GetCartItems();
            var cartItems = carts.Find(x => x.Product.IdProduct == IdProduct);
            if (cartItems != null)
            {
                cartItems.Quantity = Quantity;
            }
            SaveCart(carts);
        }

        public List<CartItem> GetCartItems()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string jsoncart = session.GetString(SystemConstants.AppSettings.CARTKEY);
            if (jsoncart != null)
            {
                var a = JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);

                return a;
            }
            return new List<CartItem>();
        }

        public async Task<List<ProductVm>> GetAll()
        {
            var query = await _iden2Context.Products.Select(x => new ProductVm()
            {
                DateAccept = x.DateAccept,
                IdBrand = x.IdBrand,
                IdCategory = x.IdCategory,
                IdProduct = x.IdProduct,
                PhotoReview = x.PhotoReview,
                Price = x.PriceExport,
                ProductName = x.ProductName,
                UseVoucher = x.UseVoucher,
            }).ToListAsync();
            return query;
        }



        public async Task<string> Purchase(string IdOrder, string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, string voucherCode)
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string Id = user.Id;
            //var freeProduct = await _iden2Context.Products.Where(x => x.IsFree==true).FirstOrDefaultAsync();

            var vouchers = _iden2Context.Vouchers;


            foreach (var item in vouchers)
            {
                if (voucherCode != null && voucherCode == item.VoucherCode && item.Quantity > 0)
                {
                    item.Quantity = item.Quantity - 1;
                }
            }
            decimal pricevou = 0;
            var voucher2=new Voucher();
            if (voucherCode != null)
            {
                voucher2 = vouchers.Where(x => x.VoucherCode == voucherCode).FirstOrDefault();
            }
            if (voucher2.Id !=0)
            {
                pricevou = voucher2.DiscountAmount.Value;
            }

            var b = GetCartItems();
            foreach (var item in b)
            {
                var orderdetails = new OrderDetails()
                {
                    IdOrder = IdOrder,
                    IdProduct = item.Product.IdProduct,
                    StatusDetails = Data.Enums.Status.Process,
                    Price = item.Product.Price * item.Quantity,
                    Quality = item.Quantity
                };
                _iden2Context.OrderDetails.Add(orderdetails);
            }
            var a = new Data.Data.Order()
            {
                IdOrder = IdOrder,
                IdUser = Id,
                Status = Data.Enums.Status.Process,
                OrderDay = DateTime.Now,
                TotalPice = b.Sum(x=>x.Product.Price*x.Quantity)- pricevou,
                NameShip = NameShip,
                EmailShip = EmailShip,
                NumberShip = NumberShip,
                NoticeShip = NoticeShip,
                AddressShip = AddressShip,
                VoucherCode = voucherCode,
                PaymentType = "Payment on delivery"
            };

            _iden2Context.Orders.Add(a);
            ClearCart();
            await _iden2Context.SaveChangesAsync();
            return IdOrder;
        }



        //public async Task<List<OrderDetailsVm>> Checkout(string IdUser)
        //{
        //    var a = from p in _iden2Context.Orders
        //            join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
        //            join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct
        //            select new { p, pt,ptt };
        //    var order = a.Where(x => x.p.IdUser == IdUser);
        //    var checkout = await order.Select(x => new OrderDetailsVm()
        //    {
        //        IdOrder = x.p.IdOrder,
        //        StatusDetails = x.pt.StatusDetails,
        //        IdProduct = x.pt.IdProduct,
        //        Price = x.pt.Price,
        //        Quality = x.pt.Quality,
        //        DateOrder = x.p.OrderDay,
        //        PhotoReview = x.ptt.PhotoReview
        //    }).ToListAsync();
        //    return checkout;
        //}

        public async Task<string> PayPal(double total)
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);
            var Carts = GetCartItems();
            total = Math.Round(total / TyGiaUSD, 2);

            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };

            foreach (var item in Carts)
            {
                itemList.Items.Add(new Item()
                {
                    Name = item.Product.ProductName,
                    Currency = "USD",
                    Price = Math.Round(item.Product.PriceExport, 2).ToString(),
                    Quantity = item.Quantity.ToString(),
                    Sku = "sku",
                    Tax = "0"
                });
            }

            //payment

            Random generator = new Random();
            string paypalOrderId = generator.Next(0, 1000000).ToString("D6");
            var payment = new Payment()
            {
                Intent = "sale",

                Transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        Amount=new Amount()
                        {
                            Total=total.ToString(),
                            Currency="USD",
                            Details=new AmountDetails
                            {
                                Tax="0",
                                Shipping="0",
                                Subtotal=total.ToString()
                            }
                        },
                        ItemList=itemList,
                        Description="descrip",
                        InvoiceNumber=paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = "http://ustoram.somee.com/Cart/CheckoutFail",
                    ReturnUrl = "http://ustoram.somee.com/Cart/CheckoutSuccess"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }
                return paypalRedirectUrl;
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return "/Cart/CheckoutFail";

            }
        }

        public async Task<string> CheckoutSuccess(decimal total)
        {
            Random generator = new Random();
            string IdOrder = generator.Next(0, 1000000).ToString("D6");


            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string Id = user.Id;
            var freeProduct = await _iden2Context.Products.Where(x => x.IsFree == true).FirstOrDefaultAsync();

            var b = GetCartItems();
            foreach (var item in b)
            {
                var orderdetails = new OrderDetails()
                {
                    IdOrder = IdOrder,
                    IdProduct = item.Product.IdProduct,
                    StatusDetails = Data.Enums.Status.Process,
                    Price = item.Product.PriceExport * item.Quantity,
                    Quality = item.Quantity
                };
                _iden2Context.OrderDetails.Add(orderdetails);
            }
            var a = new Data.Data.Order()
            {
                IdOrder = IdOrder,
                IdUser = Id,
                Status = Data.Enums.Status.Process,
                OrderDay = DateTime.Now,
                TotalPice = total,
                PaymentType = "PayPal Payment"
            };

            _iden2Context.Orders.Add(a);
            ClearCart();
            await _iden2Context.SaveChangesAsync();
            return IdOrder;
        }

        public Task<string> CheckoutFail()
        {
            throw new NotImplementedException();
        }
        private static readonly ILog log =
         LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string VNpay(string OrderCategory, decimal Amount, string txtOrderDesc, string cboBankCode, string EmailShip, string NameShip, string AddressShip, string NumberShip, string NoticeShip, string voucherCode)
        {
            string vnp_Returnurl = _configuration["VNPay:vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration["VNPay:vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = _configuration["VNPay:vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"]; //Chuoi bi mat

            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            Random generator = new Random();
            /// không cần thiết
            string IdOrder = generator.Next(0, 1000000).ToString("D6");
            order.OrderId = IdOrder; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = Amount; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            order.OrderDesc = txtOrderDesc; ;// noi dung thanh toan
            order.CreatedDate = DateTime.Now; //loai ngna hang
            ///////
            //Build URL for VNPAY
            VNPayLibrary vnpay = new VNPayLibrary();

            vnpay.AddRequestData("vnp_Version", VNPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount*100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (cboBankCode != null && !string.IsNullOrEmpty(cboBankCode)) // loai thanh toan
            {
                vnpay.AddRequestData("vnp_BankCode", cboBankCode);
            }
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", _analystRepository.GetAccess());

            vnpay.AddRequestData("vnp_Locale", "vn");


            vnpay.AddRequestData("vnp_OrderInfo", EmailShip +","+NameShip+","+AddressShip+","+NumberShip+","+NoticeShip+","+voucherCode);
            vnpay.AddRequestData("vnp_OrderType", OrderCategory); //default value: other //loai thanh toan
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version


            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return paymentUrl;
        }

        public async Task<VNPayReturnVm> VNPayReturn( string voucherCode)
        {
           
            string vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"]; //Chuoi bi mat
            var vnpayData = _httpContextAccessor.HttpContext.Request.Query;
            VNPayLibrary vnpay = new VNPayLibrary();

            foreach (var s in vnpayData)
            {
                //get all querystring data
                if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(s.Key, s.Value);
                }
            }

            var orderId = vnpay.GetResponseData("vnp_TxnRef");
            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
            String vnp_SecureHash = _httpContextAccessor.HttpContext.Request.Query["vnp_SecureHash"].ToString();
            String TerminalID = _httpContextAccessor.HttpContext.Request.Query["vnp_TmnCode"].ToString();
            String bankCode = _httpContextAccessor.HttpContext.Request.Query["vnp_BankCode"].ToString();

            string info = _httpContextAccessor.HttpContext.Request.Query["vnp_OrderInfo"].ToString();
            string[] arrListStr = info.Split(new char[] { ',' });
            


            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
            VNPayReturnVm VNreturn = new VNPayReturnVm();
            if (checkSignature)
            {
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    VNreturn.IdOrder = orderId;
                    VNreturn.TerminalID = TerminalID;
                    VNreturn.OrderId = orderId.ToString();
                    VNreturn.VnPayAmount = vnp_Amount.ToString();
                    VNreturn.BankCode = bankCode;
                    VNreturn.VnPayTranId = vnpayTranId.ToString();
                }
                else
                {
                    VNreturn=null;
                }    
            }
            else
            {
                VNreturn = null;
            }


            //database
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string Id = user.Id;
            //var freeProduct = await _iden2Context.Products.Where(x => x.IsFree==true).FirstOrDefaultAsync();

            var vouchers = _iden2Context.Vouchers;


            foreach (var item in vouchers)
            {
                if (voucherCode != null && voucherCode == item.VoucherCode && item.Quantity > 0)
                {
                    item.Quantity = item.Quantity - 1;
                }
            }


            var b = GetCartItems();
            foreach (var item in b)
            {
                var orderdetails = new OrderDetails()
                {
                    IdOrder = orderId,
                    IdProduct = item.Product.IdProduct,
                    StatusDetails = Data.Enums.Status.Process,
                    Price = item.Product.PriceExport * item.Quantity,
                    Quality = item.Quantity
                };
                _iden2Context.OrderDetails.Add(orderdetails);
            }
            var a = new Data.Data.Order()
            {
                IdOrder = orderId,
                IdUser = Id,
                Status = Data.Enums.Status.Process,
                OrderDay = DateTime.Now,
                TotalPice = vnp_Amount,
                NameShip = arrListStr[2],
                EmailShip = arrListStr[0],
                NumberShip = arrListStr[3],
                NoticeShip = arrListStr[4],
                AddressShip = arrListStr[1],
                VoucherCode = voucherCode,
                PaymentType = "VN PAY"
            };

            _iden2Context.Orders.Add(a);
            await _iden2Context.SaveChangesAsync();
            ClearCart();
            

            return VNreturn;
        }
    
    }
}
