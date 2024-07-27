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
    public class Group_termController : Controller
    {
        private readonly StudentManagementContext _context;

        public Group_termController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: Group_term
        public async Task<IActionResult> Index()
        {
            var studentManagementContext = _context.Group_term.Include(g => g.Groups).Include(g => g.Terms);
            return View(await studentManagementContext.ToListAsync());
        }

        // GET: Group_term/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group_term = await _context.Group_term
                .Include(g => g.Groups)
                .Include(g => g.Terms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group_term == null)
            {
                return NotFound();
            }

            return View(group_term);
        }

        // GET: Group_term/Create
        public IActionResult Create()
        {
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Id");
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Id");
            return View();
        }

        // POST: Group_term/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Term_id,Group_id")] Group_term group_term)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group_term);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Id", group_term.Group_id);
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Id", group_term.Term_id);
            return View(group_term);
        }

        // GET: Group_term/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group_term = await _context.Group_term.FindAsync(id);
            if (group_term == null)
            {
                return NotFound();
            }
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Id", group_term.Group_id);
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Id", group_term.Term_id);
            return View(group_term);
        }

        // POST: Group_term/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Term_id,Group_id")] Group_term group_term)
        {
            if (id != group_term.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group_term);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Group_termExists(group_term.Id))
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
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Id", group_term.Group_id);
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Id", group_term.Term_id);
            return View(group_term);
        }

        // GET: Group_term/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group_term = await _context.Group_term
                .Include(g => g.Groups)
                .Include(g => g.Terms)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (group_term == null)
            {
                return NotFound();
            }

            return View(group_term);
        }

        // POST: Group_term/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group_term = await _context.Group_term.FindAsync(id);
            if (group_term != null)
            {
                _context.Group_term.Remove(group_term);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Group_termExists(int id)
        {
            return _context.Group_term.Any(e => e.Id == id);
        }
    }
}
