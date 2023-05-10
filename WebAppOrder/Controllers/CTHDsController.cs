using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppOrder.Models;

namespace WebAppOrder.Controllers
{
    public class CTHDsController : Controller
    {
        private OrderDataEntities db = new OrderDataEntities();

        // GET: CTHDs
        public ActionResult Index(string MaHD)
        {
            using (OrderDataEntities db = new OrderDataEntities())
            {
                List<KhachHang> khachhang = db.KhachHangs.ToList();
                List<HoaDon> hoadon = db.HoaDons.ToList();
                List<NhanVien> nhanvien = db.NhanViens.ToList();
                List<MonAn> monan = db.MonAns.ToList();
                List<CTHD> cthd = db.CTHDs.ToList();
                var main = from h in hoadon
                           join k
                           in khachhang on h.MaKH equals k.MaKH
                           where (h.MaHD == MaHD)
                           select new ViewModel
                           {
                               hoadon = h,
                               khachhang = k
                           };
                var sub = from c in cthd
                          join m in monan on c.MaMonAn equals m.MaMonAn
                          where (c.MaHD == MaHD)
                          select new ViewModel
                          {
                              cthd = c,
                              monan = m,
                              Thanhtien = Convert.ToDouble(c.DonGiaBan * c.SoLuong)
                          };
                ViewBag.Main = main;
                ViewBag.Sub = sub;
                return View();
            }
            //var cTHDs = db.CTHDs.Include(c => c.HoaDon).Include(c => c.MonAn);
            //return View(cTHDs.ToList());
        }

        // GET: CTHDs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD cTHD = db.CTHDs.Find(id);
            if (cTHD == null)
            {
                return HttpNotFound();
            }
            return View(cTHD);
        }

        // GET: CTHDs/Create
        public ActionResult Create()
        {
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH");
            ViewBag.MaMonAn = new SelectList(db.MonAns, "MaMonAn", "TenMonAn");
            return View();
        }

        // POST: CTHDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,MaMonAn,SoLuong,DonGiaBan")] CTHD cTHD)
        {
            if (ModelState.IsValid)
            {
                db.CTHDs.Add(cTHD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH", cTHD.MaHD);
            ViewBag.MaMonAn = new SelectList(db.MonAns, "MaMonAn", "TenMonAn", cTHD.MaMonAn);
            return View(cTHD);
        }

        // GET: CTHDs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD cTHD = db.CTHDs.Find(id);
            if (cTHD == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH", cTHD.MaHD);
            ViewBag.MaMonAn = new SelectList(db.MonAns, "MaMonAn", "TenMonAn", cTHD.MaMonAn);
            return View(cTHD);
        }

        // POST: CTHDs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaMonAn,SoLuong,DonGiaBan")] CTHD cTHD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTHD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHD = new SelectList(db.HoaDons, "MaHD", "MaKH", cTHD.MaHD);
            ViewBag.MaMonAn = new SelectList(db.MonAns, "MaMonAn", "TenMonAn", cTHD.MaMonAn);
            return View(cTHD);
        }

        // GET: CTHDs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTHD cTHD = db.CTHDs.Find(id);
            if (cTHD == null)
            {
                return HttpNotFound();
            }
            return View(cTHD);
        }

        // POST: CTHDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CTHD cTHD = db.CTHDs.Find(id);
            db.CTHDs.Remove(cTHD);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
