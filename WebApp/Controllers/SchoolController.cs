using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaladBarWeb.DBModels;

namespace SaladBarWeb.Controllers
{
    public class SchoolController : Controller
    {
        private readonly AppDbContext _context;

        public SchoolController(AppDbContext context)
        {
            _context = context;
        }

        // GET: School
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Schools.Include(s => s.SchoolType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: School/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schools = await _context.Schools
                .Include(s => s.SchoolType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (schools == null)
            {
                return NotFound();
            }

            return View(schools);
        }

        // GET: School/Create
        public IActionResult Create()
        {
            ViewData["SchoolTypeId"] = new SelectList(_context.SchoolTypes, "Id", "CreatedBy");
            return View();
        }

        // POST: School/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,District,Mascot,Colors,SchoolLogo,SchoolTypeId,Active,DtCreated,CreatedBy,DtModified,ModifiedBy")] Schools schools)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schools);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolTypeId"] = new SelectList(_context.SchoolTypes, "Id", "CreatedBy", schools.SchoolTypeId);
            return View(schools);
        }

        // GET: School/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schools = await _context.Schools.SingleOrDefaultAsync(m => m.Id == id);
            if (schools == null)
            {
                return NotFound();
            }
            ViewData["SchoolTypeId"] = new SelectList(_context.SchoolTypes, "Id", "CreatedBy", schools.SchoolTypeId);
            return View(schools);
        }

        // POST: School/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,District,Mascot,Colors,SchoolLogo,SchoolTypeId,Active,DtCreated,CreatedBy,DtModified,ModifiedBy")] Schools schools)
        {
            if (id != schools.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schools);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolsExists(schools.Id))
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
            ViewData["SchoolTypeId"] = new SelectList(_context.SchoolTypes, "Id", "CreatedBy", schools.SchoolTypeId);
            return View(schools);
        }

        //// GET: School/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var schools = await _context.Schools
        //        .Include(s => s.SchoolType)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (schools == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(schools);
        //}

        //// POST: School/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var schools = await _context.Schools.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Schools.Remove(schools);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool SchoolsExists(long id)
        {
            return _context.Schools.Any(e => e.Id == id);
        }
    }
}
