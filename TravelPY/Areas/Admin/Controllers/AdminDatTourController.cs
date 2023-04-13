using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using TravelPY.Helpper;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminDatTourController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; set; }


        public AdminDatTourController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminDatTour

        public IActionResult Index(int page = 1, int MaTrangThai = 0)
        {
            var pageNumber = page;
            var pageSize = Utilities.PAGE_SIZE;
            /*var lsDatTours = _context.DatTours
                .Include(d => d.MaKhachHangNavigation)
                .Include(d => d.MaTrangThaiNavigation)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayDatTour);*/
            List<DatTour> ls = new List<DatTour>();
            if (MaTrangThai != 0)
            {
                ls = _context.DatTours
                .AsNoTracking()
                .Where(b => b.MaTrangThai == MaTrangThai)
                .Include(b => b.MaTrangThaiNavigation)
                .Include(b => b.MaKhachHangNavigation)
                .OrderBy(x => x.MaDatTour).ToList();
            }
            else
            {
                ls = _context.DatTours
                .AsNoTracking()
                .Include(b => b.MaTrangThaiNavigation)
                .Include(b => b.MaKhachHangNavigation)
                .OrderBy(x => x.MaDatTour).ToList();
            }
            PagedList<DatTour> models = new PagedList<DatTour>(ls.AsQueryable(), pageNumber, pageSize);
            ViewData["TrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai");
            ViewBag.MaTrangThai = MaTrangThai;
            ViewBag.CurrentPage = pageNumber;
            return View(models);

        }

        public IActionResult Filtter(int MaTrangThai = 0)
        {
            var url = $"/Admin/AdminDatTour?MaTrangThai={MaTrangThai}";
            if (MaTrangThai == 0)
            {
                url = $"/Admin/AdminDatTour";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
        // GET: Admin/AdminDatTour/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datTour = await _context.DatTours
                .Include(d => d.MaKhachHangNavigation)
                .Include(d => d.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(m => m.MaDatTour == id);
            if (datTour == null)
            {
                return NotFound();
            }
            var Chitietdonhang = _context.ChiTietDatTours
                .Include(x => x.MaTourNavigation)
                .Include(x=>x.MaDatTourNavigation)
                .AsNoTracking()
                .Where(x => x.MaDatTour == datTour.MaDatTour)
                .OrderBy(x => x.MaChiTiet)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;

            string phuongxa = GetNameLocation(datTour.PhuongXa.Value);
            string Quanhuyen = GetNameLocation(datTour.QuanHuyen.Value);
            string TinhThanh = GetNameLocation(datTour.LocationId.Value);
            string fullAddress = $"{datTour.DiaChi}, {phuongxa}, {Quanhuyen}, {TinhThanh}";
            ViewBag.FullName = fullAddress;
            return View(datTour);
        }

        public string GetNameLocation(int idlocation)
        {
            try
            {
                var location = _context.Locations.AsNoTracking().SingleOrDefault(x => x.LocationId == idlocation);
                if (location != null)
                {
                    return location.NameWithType;
                }
            }
            catch
            {
                return string.Empty;
            }
            return string.Empty;
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.DatTours                
                .Include(x => x.MaKhachHangNavigation)
                //.Include(x => x.MaTrangThaiNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaDatTour == id);
            if (order == null)
            {
                return NotFound();
            }
            //ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang", order.MaKhachHang);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai", order.MaTrangThai);
            return PartialView("ChangeStatus", order);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, [Bind("MaDatTour,NgayDi,MaKhachHang,SoCho,NgayDatTour,GhiChu,ThanhToan,MaThanhToan,NgayThanhToan,DiaChi,LocationId,QuanHuyen,PhuongXa,TongTien,MaTrangThai,Delated,Paid")] DatTour datTour)
        {
            if (id != datTour.MaDatTour)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                try
                {
                    var donhang = await _context.DatTours                       
                        .Include(x => x.MaKhachHangNavigation)
                        //.Include(x=>x.MaTrangThaiNavigation)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.MaDatTour == id);
                    if (donhang != null)
                    {
                        donhang.Paid = datTour.Paid;
                        donhang.Deleted = datTour.Deleted;
                        donhang.MaTrangThai = datTour.MaTrangThai;
                        if (donhang.Paid == true)
                        {
                            donhang.NgayThanhToan = DateTime.Now;
                        }
                        if (donhang.MaTrangThai == 4) donhang.Deleted = true;
                        //if (donhang.MaTrangThai == 2) donhang.NgayDi = DateTime.Now;
                    }
                    _context.Update(donhang);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật trạng thái đơn hàng thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatTourExists(datTour.MaDatTour))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang", datTour.MaKhachHang);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai", datTour.MaTrangThai);
            return PartialView("ChangeStatus", datTour);
        }
        // GET: Admin/AdminDatTour/Create
        public IActionResult Create()
        {
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "MaKhachHang");
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai");
            return View();
        }

        // POST: Admin/AdminDatTour/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDatTour,NgayDi,MaKhachHang,SoCho,NgayDatTour,GhiChu,ThanhToan,MaThanhToan,NgayThanhToan,DiaChi,LocationId,QuanHuyen,PhuongXa,TongTien,MaTrangThai,Delated,Paid")] DatTour datTour)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datTour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenKhachHang", datTour.MaKhachHang);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai", datTour.MaTrangThai);
            return View(datTour);
        }

        // GET: Admin/AdminDatTour/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DatTours == null)
            {
                return NotFound();
            }

            var datTour = await _context.DatTours.FindAsync(id);
            if (datTour == null)
            {
                return NotFound();
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenKhachHang", datTour.MaKhachHang);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai", datTour.MaTrangThai);
            return View(datTour);
        }

        // POST: Admin/AdminDatTour/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaDatTour,NgayDi,MaKhachHang,SoCho,NgayDatTour,GhiChu,ThanhToan,MaThanhToan,NgayThanhToan,DiaChi,LocationId,QuanHuyen,PhuongXa,TongTien,MaTrangThai,Deleted,Paid")] DatTour datTour)
        {
            if (id != datTour.MaDatTour)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datTour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatTourExists(datTour.MaDatTour))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKhachHang"] = new SelectList(_context.KhachHangs, "MaKhachHang", "TenKhachHang", datTour.MaKhachHang);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "TenTrangThai", datTour.MaTrangThai);
            return View(datTour);
        }

        // GET: Admin/AdminDatTour/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DatTours == null)
            {
                return NotFound();
            }

            var datTour = await _context.DatTours
                .Include(d => d.MaKhachHangNavigation)
                .Include(d => d.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(m => m.MaDatTour == id);
            if (datTour == null)
            {
                return NotFound();
            }
            var Chitietdonhang = _context.ChiTietDatTours
                .Include(x => x.MaTourNavigation)
                .AsNoTracking()
                .Where(x => x.MaDatTour == datTour.MaDatTour)
                .OrderBy(x => x.MaChiTiet)
                .ToList();
            ViewBag.ChiTiet = Chitietdonhang;
            return View(datTour);
        }

        // POST: Admin/AdminDatTour/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.DatTours.FindAsync(id);
            order.Deleted = true;
            _context.Update(order);
            await _context.SaveChangesAsync();
            //_notyfService.Success("Xóa đơn hàng thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool DatTourExists(int id)
        {
          return _context.DatTours.Any(e => e.MaDatTour == id);
        }
    }
}
