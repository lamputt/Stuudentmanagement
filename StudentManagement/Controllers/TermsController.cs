using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class TermsController : Controller
    {
        private readonly StudentManagementContext _context;

        public TermsController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: Terms
        public async Task<IActionResult> Index(string searchString = null)
        {
            var terms = from d in _context.Terms
                              select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                terms = terms.Where(d => d.Name.Contains(searchString));
            }

            var result = await terms.ToListAsync();

            return View(result);
        }

        // GET: Terms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terms = await _context.Terms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terms == null)
            {
                return NotFound();
            }

            return View(terms);
        }

        // GET: Terms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Terms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,From_date,To_date,Status")] Terms terms)
        {
            if (ModelState.IsValid)
            {
                _context.Add(terms);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Course create successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(terms);
        }

        // GET: Terms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terms = await _context.Terms.FindAsync(id);
            if (terms == null)
            {
                return NotFound();
            }
            return View(terms);
        }

        // POST: Terms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,From_date,To_date,Status")] Terms terms)
        {
            if (id != terms.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(terms);
                    TempData["Message"] = "Course edit successfully";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TermsExists(terms.Id))
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
            return View(terms);
        }

        // GET: Terms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terms = await _context.Terms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (terms == null)
            {
                return NotFound();
            }

            return View(terms);
        }

        // POST: Terms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var terms = await _context.Terms.FindAsync(id);
            if (terms != null)
            {
                TempData["Message"] = "Course delete successfully";
                _context.Terms.Remove(terms);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TermsExists(int id)
        {
            return _context.Terms.Any(e => e.Id == id);
        }
    }
}
