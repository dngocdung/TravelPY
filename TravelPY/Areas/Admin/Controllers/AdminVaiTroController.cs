using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelPY.Models;

namespace TravelPY.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminVaiTroController : Controller
    {
        private readonly DbToursContext _context;
        public INotyfService _notyfService { get; set; }
        public AdminVaiTroController(DbToursContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService= notyfService;
        }

        // GET: Admin/AdminVaiTro
        public async Task<IActionResult> Index()
        {
              return View(await _context.VaiTros.ToListAsync());
        }

        // GET: Admin/AdminVaiTro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VaiTros == null)
            {
                return NotFound();
            }

            var vaiTro = await _context.VaiTros
                .FirstOrDefaultAsync(m => m.MaVaiTro == id);
            if (vaiTro == null)
            {
                return NotFound();
            }

            return View(vaiTro);
        }

        // GET: Admin/AdminVaiTro/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminVaiTro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaVaiTro,TenVaiTro,MoTa")] VaiTro vaiTro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vaiTro);
                await _context.SaveChangesAsync();
                _notyfService.Success("Tạo mới thành công.");
                return RedirectToAction(nameof(Index));
            }
            return View(vaiTro);
        }

        // GET: Admin/AdminVaiTro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VaiTros == null)
            {
                return NotFound();
            }

            var vaiTro = await _context.VaiTros.FindAsync(id);
            if (vaiTro == null)
            {
                return NotFound();
            }
            return View(vaiTro);
        }

        // POST: Admin/AdminVaiTro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaVaiTro,TenVaiTro,MoTa")] VaiTro vaiTro)
        {
            if (id != vaiTro.MaVaiTro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaiTro);
                    _notyfService.Success("Cập nhập thành công.");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaiTroExists(vaiTro.MaVaiTro))
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
            return View(vaiTro);
        }

        // GET: Admin/AdminVaiTro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VaiTros == null)
            {
                return NotFound();
            }

            var vaiTro = await _context.VaiTros
                .FirstOrDefaultAsync(m => m.MaVaiTro == id);
            if (vaiTro == null)
            {
                return NotFound();
            }

            return View(vaiTro);
        }

        // POST: Admin/AdminVaiTro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VaiTros == null)
            {
                return Problem("Entity set 'DbToursContext.VaiTros'  is null.");
            }
            var vaiTro = await _context.VaiTros.FindAsync(id);
            if (vaiTro != null)
            {
                _context.VaiTros.Remove(vaiTro);
            }
            
            await _context.SaveChangesAsync();
            _notyfService.Success("Xoá quyền truy cập thành công.");
            return RedirectToAction(nameof(Index));
        }

        private bool VaiTroExists(int id)
        {
          return _context.VaiTros.Any(e => e.MaVaiTro == id);
        }
    }
}
