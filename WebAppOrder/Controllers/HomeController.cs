using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using WebAppOrder.Models;

namespace WebAppOrder.Controllers
{
    public class HomeController : Controller
    {
        OrderDataEntities db = new OrderDataEntities();
        public ActionResult Index(string currentFilter, int?page, int maloaisp = 0, string SearchString ="")
        {
            if (SearchString != "")
            {
                page = 1;
                var monAns = db.MonAns.Include(s => s.LoaiSP).Where(x => x.TenMonAn.ToUpper().Contains(SearchString.ToUpper()));
                monAns = monAns.OrderBy(x => x.TenMonAn);
                int pageSize = monAns.Count();
                int pageNumber = (page ?? 1);
                return View(monAns.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                SearchString = currentFilter;
                ViewBag.CurrentFilter = currentFilter;
            }
            if (maloaisp == 0)
            {
                int pageSize = 8;
                int pageNumber = (page ?? 1);
                var monAns = db.MonAns.Include(s => s.LoaiSP).OrderBy(x => x.TenMonAn);
                return View(monAns.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var monAns = db.MonAns.Include(s => s.LoaiSP).Where(x => x.MaLoaiSP == maloaisp);
                monAns = monAns.OrderBy(x => x.TenMonAn);
                int pageSize = monAns.Count();
                int pageNumber = (page ?? 1);
                return View(monAns.ToPagedList(pageNumber,pageSize));
            }    
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}