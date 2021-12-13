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
    public class ContactRepository : IContactRepository
    {
        private readonly Iden2Context _iden2Context;
        private readonly IAccountRepository _IaccountRepository;

        public ContactRepository(Iden2Context iden2Context,IAccountRepository IaccountRepository)
        {
            _iden2Context = iden2Context;
            _IaccountRepository = IaccountRepository;
        }

        public async Task<int> Feedback(string UserName, string PhoneNumber, string Email, string Content)
        {
            var a = new Feedback()
            {
                Content = Content,
                Status = true,
                CreatedDate = DateTime.Now,
                Email = Email,
                PhoneNumber = PhoneNumber,
                UserName = UserName
            };
            _iden2Context.Add(a);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<ContactVm> GetContact()
        {
            var x =await _iden2Context.Contacts.Where(x => x.Status == true).FirstOrDefaultAsync();
            var contact = new ContactVm();
            contact.Content = x.Content;
            contact.Status = x.Status;
            return contact;
        }

        public async Task<int> SendEmailPromotion(string Email)
        {
            var a = _iden2Context.EmailPromotions;
            var email = new EmailPromotion()
            {
                Email = Email
            };
            a.Add(email);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<string> SendOrderCompleted(string IdOrder)
        {
            var order = await _iden2Context.Orders.Where(x => x.IdOrder == IdOrder).FirstOrDefaultAsync();
            var user = await _iden2Context.Users.Where(x => x.Id == order.IdUser).FirstOrDefaultAsync();

            var body = @"<html lang="" en"">
                        <head>
                        
                            <title>
                                Upcoming topics
                            </title>
                            <style>
                                .Logo {
                                    text-align: center;
                                    color: orange;
                                    font-size: 40;
                                }
                        
                                .tks {
                                    text-align: center;
                                    font-size: 20px;
                                    color: #4660de;
                                }
                        
                                .ustora2 {
                                    border: 1px solid black;
                                    margin-right: 200px;
                                    margin-left: 200px;
                                    border-radius: 10px;
                                }
                        
                                .dot {
                                    background-color:#f0f0f0;
                                    display:block;
                                    height:20px;
                                }
                        
                                .data {
                                    padding-left: 40px;
                                }
                        
                                label{
                                    color:black;
                                    font-weight:bold;
                                }
                        
                                .bien {
                                    color: blue
                                }
                                .announce{
                                    font-weight:bold;
                                }
                                .note{
                                    margin-left:20px;
                                }
                            </style>
                        </head>
                        <body>
                            <div class="" ustora"">
                                <div class="" ustora2"">
                                    <h1 class="" Logo"">Ustora</h1>
                                    <div class="" tks"">Đơn hàng của bạn đã được giao thành công!</div>
                                    <p>Lazada đã giao cho bạn đầy đủ với các sản phẩm được liệt kê bên dưới theo đơn hàng <label>#"+order.IdOrder + @"</label> của bạn, Lazada hi vọng bạn hài lòng với các sản phẩm này! </p>
                                    <p class=""announce"">Một vài lưu ý nhỏ cho bạn:</p>
                                    <p class=""note"">Hãy kiểm tra kỹ chất lượng sản phẩm bạn nhận được và giữ lại hóa đơn, hộp sản phẩm và phiếu bảo hành (nếu có) để trả hàng hoặc bảo hành khi cần thiết.</p>
                                    <p class=""note"">Trong trường hợp sản phẩm có dấu hiệu hư hỏng/ bể vỡ hoặc không đúng với thông tin như mô tả, bạn hãy <a href=""#"">liên hệ</a> với Lazada bằng cách để lại tin nhắn tại trang Liên hệ trong vòng 24 giờ sau khi nhận sản phẩm để được hỗ trợ nhé. </p>
                                    <div class="" dot""> </div>
                                    <div class="" price"">
                                        <table class="" "">
                                            <thead>
                                                
                                                <tr>
                                                    <th class="" bien"">Tổng tiền</th>
                                                    <td class="" data"">" + order.TotalPice + @"</td>
                                                </tr>
                                                <tr>
                                                    <th class="" bien"">Hình thức thanh toán</th>
                                                    <td class="" data"">" + order.PaymentType + @"</td>
                                                </tr>
                                            </thead>
                                        </table>
                        
                                    </div>
                                </div>
                            </div>
                        </body>
                        </html>";

            _IaccountRepository.SendTo(user.Email, "Your order" + IdOrder + "has been completed", body);
            return IdOrder;
        }

        public async Task<string> SendOrderDeliveried(string IdOrder)
        {
            var order = await _iden2Context.Orders.Where(x => x.IdOrder == IdOrder).FirstOrDefaultAsync();
            var user = await _iden2Context.Users.Where(x => x.Id == order.IdUser).FirstOrDefaultAsync();
            var body = @"<html lang="" en"">
                        <head>
                        
                            <title>
                                Upcoming topics
                            </title>
                            <style>
                                body {
                                    font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif;
                                }
                                .Logo {
                                    text-align: center;
                                    color: orange;
                                    font-size: 40px;
                                }
                        
                                .tks {
                                    font-size: 20px;
                                    color: blue;
                                }
                        
                                .ustora2 {
                                    border: 1px solid black;
                                    margin-right: 200px;
                                    margin-left: 200px;
                                    border-radius: 10px;
                                }
                        
                                .dot {
                                    background-color: #f0f0f0;
                                    display: block;
                                    height:15px;
                                }
                        
                                .data {
                                    padding-left: 40px;
                                }
                        
                                .bien {
                                    color: blue
                                }
                                img {
                                    margin-top: 10px;
                                    margin-bottom: 10px;
                                   max-width:90%;
                                }
                                span{
                                    color:orange;
                                }
                                .first{
                                    color:blue;
                                    font-size:18px;
                        
                                }
                                .introduce {
                                    background-color: #f0f0f0;
                                    font-size:16px;
                                    height:155px;
                                }
                                .nut {
                                    background-color: orange;
                                    height: 40px;
                                    width: 300px;
                                    text-align: center;
                                    justify-content: center;
                                    display: block;
                                    padding-top: 10px;
                                    margin-left: 30px;
                                    color: wheat;
                                    font-weight: bold;
                                    text-decoration: none;
                                }
                                .about{
                                    margin-left:32px;
                                }
                                .tach{
                                    padding-left:200px;
                                }
                        
                            </style>
                        </head>
                        <body>
                            <div class=""ustora"">
                                <div class="" ustora2"">
                                    <h1 class="" Logo"">Ustora</h1>
                                    <div class="" tks"">Một kiện hàng đang được giao đến bạn</div>
                                    <img  src=""http://ustoram.somee.com/Images/unnamed.jpg"" />
                        
                                    <div class=""introduce"">
                                        <p class=""first"">Nhật Trường thân mến,</p>
                                        <p>Một kiện hàng thuộc đơn hàng <span>#"+order.IdOrder+ @"</span> đang được giao đến bạn bởi LELEXPRESS. Bạn sẽ nhận được cuộc gọi từ đơn vị vận chuyển để thông báo thời gian giao hàng cụ thể.</p>
                                        <a href=""@Url.Action(""OrderHistoryDetails"", ""Account"", new { IdOrder = ""071523"" })"" class=""nut"">
                                            XEM CHI TIẾT ĐƠN HÀNG
                                        </a>
                                    </div>
                                    <div>
                                        <p class=""first"">Bước tiếp theo</p>
                                        <div class=""about"">
                                            <p>Bạn vui lòng chuẩn bị sẵn số tiền mặt tương ứng để thuận tiện cho việc thanh toán.</p>
                                            <p>Kiểm tra kỹ thông tin đơn hàng trước khi thanh toán, bạn chỉ có thể kiểm tra ngoại quan kiện hàng (thông tin mua hàng, tình trạng kiện hàng, ..) và bạn chỉ có thể mở hộp kiểm tra sản phẩm sau khi đã thanh toán với nhân viên giao hàng.</p>
                                            <p>Nếu bạn phát hiện kiện hàng có dấu hiệu móp méo, không còn nguyên vẹn hoặc sai thông tin người nhận, vui lòng từ chối nhận hàng. Lazada khuyến khích người mua nên chụp lại kiện hàng trước và sau khi nhận hàng để làm bằng chứng nếu có tranh chấp về sau.</p>
                                        </div>
                                    </div>
                                    <div class=""dot""></div>
                                    <div>
                                        <p class=""first"">Đơn hàng được giao đến</p>
                                        <table>
                                            <thead>
                                                <tr>
                                                    <td>" + order.NameShip + @"</td>
                                                    <td class=""tach"">Điện thoại: " + order.NumberShip + @"</td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td class=""tach"">Email:  " + order.EmailShip + @"</td>
                                                </tr>
                                            </thead>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </body>
                        </html>
                                ";
            _IaccountRepository.SendTo(user.Email, "Your order" +IdOrder+ "has been delivered",body);
            return IdOrder;
        }


        public  string SendOrderReceived(string IdOrder, string EmailShip, string NameShip, string AddressShip, decimal Total, string TypePayment)
        {

            var htmlbody = @"
            <html lang="" en"">
            <head>
                
                <title>
                    Upcoming topics
                </title>
                <style >
                    .Logo{
                        text-align:center;
                        color:orange;
                        font-size:40;
                    }
            
                    .tks {
                        text-align: center;
                        font-size:20px;
                        color:#4660de ;
                    }
                    .ustora2 {
                        border: 1px solid black;
                        margin-right: 200px;
                        margin-left: 200px;
                        border-radius:10px;
                    }
                    .dot{
                        border-top:1px solid black;
                    }
                    .data{
                        padding-left:40px;
                    }
                    .bien{
                        color:blue
                    }
                </style>
            </head>
            <body>
                <div class=""ustora"">
                    <div class=""ustora2"">
                        <h1 class=""Logo"">Ustora</h1>
                        <div class=""tks"">Cảm ơn bạn đã đặt hàng tại Ustora</div>
                        <p>Xin chào " + NameShip + @"</p>
                        <p>Ustora đã nhận được yêu cầu đặt hàng của bạn và đang xử lý nhé.Bạn sẽ nhận được thông báo tiếp theo khi đơn hàng đã sẵn sàng được giao.</p>
                        <div class=""dot"">  </div>
                        <div>
                            <table class="">
                                <thead>
                                    <tr>
                                        <th class="""">Tên:</th>
                                        <td class=""data"">" + NameShip+ @" </td>
                                    </tr>
                                    <tr>
                                        <th class="""">Địa chỉ:</th>
                                        <td class=""data"">" + AddressShip + @"</td>
                                    </tr>
                                    <tr>
                                        <th>Email:</th>
                                        <td class=""data"">" + EmailShip + @"</td>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div class=""dot""></div>
                        <div class=""price"">
                            <table class="""">
                                <thead>
                                    <tr>
                                        <th class=""bien"">IdOrder</th>
                                        <td class=""data"">" + IdOrder + @"</td>
                                    </tr>
                                    <tr>
                                        <th class=""bien"">Tổng tiền</th>
                                        <td class=""data"">" + Total + @" VNĐ</td>
                                    </tr>
                                    <tr>
                                        <th class=""bien"">Hình thức thanh toán</th>
                                        <td class=""data"">" + TypePayment + @"</td>
                                    </tr>
                                </thead>
                            </table>
            
                        </div>
                    </div>
                </div>
            </body>
            </html>
            ";

            _IaccountRepository.SendTo(EmailShip, "Ustora has been received your order request : " + IdOrder,htmlbody /*"Ustora has been received you order request : " + IdOrder + ", your order will be process soon"*/);
            return  IdOrder;
        }



    }
}
