using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class GroupsController : Controller
    {
        private readonly StudentManagementContext _context;

        public GroupsController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index(string searchString)
        {
            var groupsQuery = _context.Groups
                .Include(g => g.Departments)
                .Include(g => g.Terms)
                .Include(g => g.Accounts)
                .ThenInclude(a => a.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                groupsQuery = groupsQuery.Where(g =>
                    g.Name.Contains(searchString) ||
                    g.Departments.Name.Contains(searchString) ||
                    g.Terms.Name.Contains(searchString) ||
                    g.Accounts.Users.Full_name.Contains(searchString));
            }

            var groups = await groupsQuery.ToListAsync();
            return View(groups);
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Departments)
                .Include(g => g.Terms)
                .Include(g => g.Accounts)
                    .ThenInclude(a => a.Users)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            // Fetch roles to filter teachers
            var teachers = _context.Accounts
                .Include(a => a.Roles)
                .Where(a => a.Roles.Name == "Teacher")
                .Select(a => new { a.Id, a.Users.Full_name })
                .ToList();

            ViewData["Teacher_id"] = new SelectList(teachers, "Id", "Full_name");
            ViewData["Department_id"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Name");
            return View();
        }

        // POST: Groups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Department_id,Name,Student_number,Term_id,Teacher_id,Status")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groups);
                TempData["Message"] = "Group created successfully";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate select lists if validation fails
            var teachers = _context.Accounts
                .Include(a => a.Roles)
                .Where(a => a.Roles.Name == "Teacher")
                .Select(a => new { a.Id, a.Users.Full_name })
                .ToList();

            ViewData["Teacher_id"] = new SelectList(teachers, "Id", "Full_name", groups.Teacher_id);
            ViewData["Department_id"] = new SelectList(_context.Departments, "Id", "Name", groups.Department_id);
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Name", groups.Term_id);
            return View(groups);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }

            // Fetch roles to filter teachers
            var teachers = _context.Accounts
                .Include(a => a.Roles)
                .Where(a => a.Roles.Name == "Teacher")
                .Select(a => new { a.Id, a.Users.Full_name })
                .ToList();

            ViewData["Teacher_id"] = new SelectList(teachers, "Id", "Full_name", groups.Teacher_id);
            ViewData["Department_id"] = new SelectList(_context.Departments, "Id", "Name", groups.Department_id);
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Name", groups.Term_id);
            return View(groups);
        }

        // POST: Groups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Department_id,Name,Student_number,Term_id,Teacher_id,Status")] Groups groups)
        {
            if (id != groups.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["Message"] = "Edit group successfully";
                    _context.Update(groups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsExists(groups.Id))
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

            // Repopulate select lists if validation fails
            var teachers = _context.Accounts
                .Include(a => a.Roles)
                .Where(a => a.Roles.Name == "Teacher")
                .Select(a => new { a.Id, a.Users.Full_name })
                .ToList();

            ViewData["Teacher_id"] = new SelectList(teachers, "Id", "Full_name", groups.Teacher_id);
            ViewData["Department_id"] = new SelectList(_context.Departments, "Id", "Name", groups.Department_id);
            ViewData["Term_id"] = new SelectList(_context.Terms, "Id", "Name", groups.Term_id);
            return View(groups);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(g => g.Departments)
                .Include(g => g.Terms)
                .Include(g => g.Accounts)
                    .ThenInclude(a => a.Users)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                TempData["Message"] = "Delete group successfully";
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GroupsExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
