using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppOrder.Models;
using System.IO;
using PagedList;


namespace WebAppOrder.Controllers
{
    public class MonAnsController : Controller
    {
        private OrderDataEntities db = new OrderDataEntities();

        // GET: MonAns
        public ActionResult Index(string sortOrder, int?page)
        {
            ViewBag.SortByName = String.IsNullOrEmpty(sortOrder) ? "ten_desc" : "";
            ViewBag.SortByPrice = (sortOrder == "dongia" ? "dongia_desc" : "dongia");
            ViewBag.CurrentSort = sortOrder;
            var monAns = db.MonAns.Include(m => m.LoaiSP).Include(m => m.NhaCungCap);
            switch (sortOrder)
            {
                case "ten_desc":
                    monAns = monAns.OrderByDescending(s => s.TenMonAn);
                    break;
                case "dongia_desc":
                    monAns = monAns.OrderByDescending(s => s.DonGia);
                    break;
                case "dongia":
                    monAns = monAns.OrderBy(s => s.DonGia);
                    break;
                default: //mac dinh sap xep theo ten mon an
                    monAns = monAns.OrderBy(s => s.TenMonAn);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(monAns.ToPagedList(pageNumber, pageSize));
        }

        // GET: MonAns/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAn monAn = db.MonAns.Find(id);
            if (monAn == null)
            {
                return HttpNotFound();
            }
            return View(monAn);
        }

        // GET: MonAns/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC");
            return View();
        }

        // POST: MonAns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaMonAn,TenMonAn,MaNCC,MaLoaiSP,DonGia,SoLuong,Anh,NgayTao,TrangThai")] MonAn monAn, HttpPostedFileBase Anh)
        {
            if (ModelState.IsValid)
            {
                if (Anh != null && Anh.ContentLength >0)
                {
                    string filename = Path.GetFileName(Anh.FileName);
                    string path = Server.MapPath("~/Image/" + filename);
                    monAn.Anh = @"Image/" + filename;
                    Anh.SaveAs(path);
                }    
                db.MonAns.Add(monAn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP", monAn.MaLoaiSP);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", monAn.MaNCC);
            return View(monAn);
        }

        // GET: MonAns/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAn monAn = db.MonAns.Find(id);
            if (monAn == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP", monAn.MaLoaiSP);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", monAn.MaNCC);
            return View(monAn);
        }

        // POST: MonAns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaMonAn,TenMonAn,MaNCC,MaLoaiSP,DonGia,SoLuong,Anh,NgayTao,TrangThai")] MonAn monAn, HttpPostedFileBase AnhUpload, string Anh)
        {
            if (ModelState.IsValid)
            {
                if (AnhUpload != null && AnhUpload.ContentLength > 0)
                {
                    string filename = Path.GetFileName(AnhUpload.FileName);
                    string path = Server.MapPath("~/Image/" + filename);
                    monAn.Anh = @"Image/" + filename;
                    AnhUpload.SaveAs(path);
                }
                else
                {
                    monAn.Anh = Anh;
                }
                db.Entry(monAn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSPs, "MaLoaiSP", "TenLoaiSP", monAn.MaLoaiSP);
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps, "MaNCC", "TenNCC", monAn.MaNCC);
            return View(monAn);
        }

        // GET: MonAns/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonAn monAn = db.MonAns.Find(id);
            if (monAn == null)
            {
                return HttpNotFound();
            }
            return View(monAn);
        }

        // POST: MonAns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MonAn monAn = db.MonAns.Find(id);
            db.MonAns.Remove(monAn);
            db.SaveChanges();
            System.IO.File.Delete(Server.MapPath("~/" + monAn.Anh));
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
