using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppOrder.Models;
using System.Net;
using System.Net.Mail;

namespace WebAppOrder.Controllers
{
    public class GiohangController : Controller
    {
        private OrderDataEntities db = new OrderDataEntities();
        // GET: Giohang
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
        }
        //Thêm món vào giỏ hàng
        public RedirectToRouteResult AddToCart(string MaMonAn)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<CartItem>();

            }
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            //Kiểm tra món đang chọn có trong giỏ hàng chưa
            if (giohang.FirstOrDefault(m => m.MaMonAn == MaMonAn) == null) // chưa có
            {
                MonAn mn = db.MonAns.Find(MaMonAn);
                CartItem newItem = new CartItem();
                newItem.MaMonAn = MaMonAn;
                newItem.TenMonAn = mn.TenMonAn;
                newItem.SoLuong = 1;
                newItem.DonGia = Convert.ToDouble(mn.DonGia);
                giohang.Add(newItem);
            }
            else // đã có trong giỏ thì tăng lên 1
            {
                CartItem cartItem = giohang.FirstOrDefault(m => m.MaMonAn == MaMonAn);
                cartItem.SoLuong++;
            }
            Session["giohang"] = giohang;
            return RedirectToAction("Index");

        }

        public RedirectToRouteResult Update(string MaMonAn, int txtSoLuong)
        {
            //tìm cartitem muốn xóa
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(m => m.MaMonAn == MaMonAn);
            if (item != null)
            {
                item.SoLuong = txtSoLuong;
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }

        public RedirectToRouteResult DelCartItem(string MaMonAn)
        {
            //tim để xóa
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem item = giohang.FirstOrDefault(m => m.MaMonAn == MaMonAn);
            if (item != null)
            {
                giohang.Remove(item);
                Session["giohang"] = giohang;
            }
            return RedirectToAction("Index");
        }
        public ActionResult Order(string Email, string Phone)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            string sMsg = "<html><body><table border= '1'><caption>Thông tin đặt hàng</caption><tr><th>STT</th><th>Tên món ăn</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>";
            int i = 0;
            double tongTien = 0;
            foreach (CartItem item in giohang)
            {
                i++;
                sMsg += "<tr>";
                sMsg += "<td>" + i.ToString() + "</td>";
                sMsg += "<td>" + item.TenMonAn + "</td>";
                sMsg += "<td>" + item.SoLuong.ToString() + "</td>";
                sMsg += "<td>" + item.DonGia.ToString() + "</td>";
                sMsg += "<td>" + String.Format("{0:#,###}", item.SoLuong * item.DonGia) + "</td>";
                sMsg += "</tr>";
                tongTien += item.SoLuong * item.DonGia;
            }
            sMsg += "<tr><th colspan ='5'> Tổng cộng: " + String.Format("{0:#,### vnđ}", tongTien) + "</th></tr></table>";
            MailMessage mail = new MailMessage("1951050058nhu@ou.edu.vn", Email, "Thông tin đơn hàng", sMsg);
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("1951050058nhu@ou.edu.vn", "0962750905nhu");
            mail.IsBodyHtml = true;
            client.Send(mail);
            return RedirectToAction("Index", "Home");
            //gửi mail

        }
    }
}