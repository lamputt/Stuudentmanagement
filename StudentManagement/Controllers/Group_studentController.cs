using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class Group_studentController : Controller
    {
        private readonly StudentManagementContext _context;

        public Group_studentController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: Group_student
        public async Task<IActionResult> Index(string searchString)
        {
            var groupStudentQuery = _context.Group_student
       
                .Include(gs => gs.Courses)
                .Include(gs => gs.Groups)
                .Include(gs => gs.Accounts)
                .ThenInclude(a => a.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                groupStudentQuery = groupStudentQuery.Where(gs =>
                    gs.Accounts.Users.Full_name.Contains(searchString) ||
                    gs.Courses.Name.Contains(searchString) ||
                    gs.Groups.Name.Contains(searchString));
            }

            var group_students = await groupStudentQuery.ToListAsync();
            return View(group_students);
        }

        // GET: Group_student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group_student = await _context.Group_student
                .Include(gs => gs.Accounts)
                          .ThenInclude(a => a.Users)
                .Include(gs => gs.Courses)
                .Include(gs => gs.Groups)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (group_student == null)
            {
                return NotFound();
            }

            return View(group_student);
        }

        // GET: Group_student/Create
        public IActionResult Create()
        {
            var student = _context.Accounts
             .Include(a => a.Roles)
             .Where(a => a.Roles.Name == "Student")
             .Select(a => new { a.Id, a.Users.Full_name })
             .ToList();

            ViewData["Student_id"] = new SelectList(student, "Id", "Full_name");
            ViewData["Course_id"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Name");
            return View();
        }

        // POST: Group_student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Student_id,Group_id,Course_id,Absent,Present")] Group_student group_student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group_student);
                TempData["Message"] = "Group student created successfully";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var student = _context.Accounts
             .Include(a => a.Roles)
             .Where(a => a.Roles.Name == "Student")
             .Select(a => new { a.Id, a.Users.Full_name })
             .ToList();
            ViewData["Student_id"] = new SelectList(student, "Id", "Full_name");
            ViewData["Course_id"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Name");
            return View(group_student);

        }

        // GET: Group_student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group_student = await _context.Group_student.FindAsync(id);
            if (group_student == null)
            {
                return NotFound();
            }

            var student = _context.Accounts
            .Include(a => a.Roles)
            .Where(a => a.Roles.Name == "Student")
            .Select(a => new { a.Id, a.Users.Full_name })
            .ToList();
            ViewData["Student_id"] = new SelectList(student, "Id", "Full_name");
            ViewData["Course_id"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Name");
            return View(group_student);
        }

        // POST: Group_student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Student_id,Group_id,Course_id,Absent,Present")] Group_student group_student)
        {
            if (id != group_student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group_student);
                    TempData["Message"] = "Group student edited successfully";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Group_studentExists(group_student.Id))
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

            var student = _context.Accounts
             .Include(a => a.Roles)
             .Where(a => a.Roles.Name == "Student")
             .Select(a => new { a.Id, a.Users.Full_name })
             .ToList();
            ViewData["Student_id"] = new SelectList(student, "Id", "Full_name");
            ViewData["Course_id"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["Group_id"] = new SelectList(_context.Groups, "Id", "Name");
            return View(group_student);
        }

        // GET: Group_student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group_student = await _context.Group_student
                .Include(gs => gs.Accounts)
                .ThenInclude(a => a.Users)
                .Include(gs => gs.Courses)
                .Include(gs => gs.Groups)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (group_student == null)
            {
                return NotFound();
            }

            return View(group_student);
        }

        // POST: Group_student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group_student = await _context.Group_student.FindAsync(id);
            if (group_student != null)
            {
                _context.Group_student.Remove(group_student);
                TempData["Message"] = "Group student deleted successfully";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool Group_studentExists(int id)
        {
            return _context.Group_student.Any(e => e.Id == id);
        }
    }
}
