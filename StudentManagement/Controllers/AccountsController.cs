// Controllers/AccountsController.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly StudentManagementContext _context;

        public AccountsController(StudentManagementContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index(string searchString)
        {
            var accountsQuery = _context.Accounts
                .Include(a => a.Roles)
                .Include(a => a.Users)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                accountsQuery = accountsQuery.Where(a =>
                    a.Username.Contains(searchString) ||
                    a.Users.Full_name.Contains(searchString) ||
                    a.Roles.Name.Contains(searchString));
            }

            var accounts = await accountsQuery.ToListAsync();
            return View(accounts);
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .Include(a => a.Roles)
                .Include(a => a.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["Role_id"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["User_id"] = new SelectList(_context.Users, "Id", "Full_name");
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Role_id,User_id,Username,Password")] Accounts accounts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accounts);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Account created successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Role_id"] = new SelectList(_context.Roles, "Id", "Name", accounts.Role_id);
            ViewData["User_id"] = new SelectList(_context.Users, "Id", "Full_name", accounts.User_id);
            return View(accounts);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts.FindAsync(id);
            if (accounts == null)
            {
                return NotFound();
            }
            ViewData["Role_id"] = new SelectList(_context.Roles, "Id", "Name", accounts.Role_id);
            ViewData["User_id"] = new SelectList(_context.Users, "Id", "Full_name", accounts.User_id);
            return View(accounts);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Role_id,User_id,Username,Password")] Accounts accounts)
        {
            if (id != accounts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accounts);
                    TempData["Message"] = "Edit Account successfully";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountsExists(accounts.Id))
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
            ViewData["Role_id"] = new SelectList(_context.Roles, "Id", "Name", accounts.Role_id);
            ViewData["User_id"] = new SelectList(_context.Users, "Id", "Full_name", accounts.User_id);
            return View(accounts);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accounts = await _context.Accounts
                .Include(a => a.Roles)
                .Include(a => a.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accounts == null)
            {
                return NotFound();
            }

            return View(accounts);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accounts = await _context.Accounts.FindAsync(id);
            if (accounts != null)
            {
                TempData["Message"] = "Delete Account successfully";
                _context.Accounts.Remove(accounts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountsExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
        // GET: /Accounts/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Tìm tài khoản và bao gồm thông tin người dùng và vai trò
            var account = await _context.Accounts
                .Include(a => a.Users)  // Bao gồm thông tin người dùng
                .Include(a => a.Roles)  // Bao gồm thông tin vai trò
                .FirstOrDefaultAsync(a => a.Username == model.UserName && a.Password == model.Password);

            if (account == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return View(model);
            }

            var user = account.Users;
            var role = account.Roles;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Full_name),
                new Claim(ClaimTypes.Role, role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Chuyển hướng dựa trên vai trò
            if (role.Name == "Admin")
            {
                return RedirectToAction("Index", "Departments"); // Trang chính dành cho Admin
            }
            else if (role.Name == "Teacher")
            {
                return RedirectToAction("Index", "Courses"); // Trang dành cho Teacher (layout tùy chọn)
            }
            else if (role.Name == "Student")
            {
                return RedirectToAction("indexCourse", "Courses"); 
            }

            // Nếu không thuộc vai trò nào trên, chuyển hướng về trang đăng nhập
            else
            {
                ModelState.AddModelError("", "Sorry username or password is not correct");
                return View(model);

            }
        }

        // GET: /Accounts/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}
