using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppOrder.Controllers;
using WebAppOrder.Models;
using System.Collections;

namespace WebAppOrder.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {

            using (OrderDataEntities db = new OrderDataEntities())
            {
                var loaisp = db.LoaiSPs.ToList();
                Hashtable tenloaisp = new Hashtable();
                foreach (var item in loaisp)
                {
                    tenloaisp.Add(item.MaLoaiSP, item.TenLoaiSP);
                }
                ViewBag.TenLoaiSP = tenloaisp;
                return PartialView("Index");
            }
        }
    }
}