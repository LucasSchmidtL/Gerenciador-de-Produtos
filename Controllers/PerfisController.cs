using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_de_Produtos.Data;
using Gerenciador_de_Produtos.Models;

namespace Gerenciador_de_Produtos.Controllers
{
    public class PerfisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerfisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Perfis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Perfis.ToListAsync());
        }

        // GET: Perfis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }

        // GET: Perfis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Perfis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ERP,Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                _context.Add(perfil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(perfil);
        }

        // GET: Perfis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfis.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }
            return View(perfil);
        }

        // POST: Perfis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ERP,Desenho,Descricao,TipoSecao,Peso,AreaBruta,AreaLiq,AreaEq,Ix,Sxt,Sxb,Zx,Rx,yt,yb,Ixy,Iy,Syl,Syr,Zy,ry,xl,xr,xo,yo,jx,jy,Cw,J,Ixe,Sxet,Sxeb,lye,Syel,Syer,p1,p2,p3,SimetricoX,SimetricoY")] Perfil perfil)
        {
            if (id != perfil.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfil);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilExists(perfil.Id))
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
            return View(perfil);
        }

        // GET: Perfis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfil = await _context.Perfis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perfil == null)
            {
                return NotFound();
            }

            return View(perfil);
        }

        // POST: Perfis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfil = await _context.Perfis.FindAsync(id);
            if (perfil != null)
            {
                _context.Perfis.Remove(perfil);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfilExists(int id)
        {
            return _context.Perfis.Any(e => e.Id == id);
        }
    }
}
