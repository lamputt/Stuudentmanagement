using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;

namespace StudentManagement.Data
{
    public class StudentManagementContext : DbContext
    {
        public StudentManagementContext (DbContextOptions<StudentManagementContext> options)
            : base(options)
        {
        }

        public DbSet<StudentManagement.Models.Departments> Departments { get; set; } = default!;
        public DbSet<StudentManagement.Models.Courses> Courses { get; set; } = default!;
        public DbSet<StudentManagement.Models.Roles> Roles { get; set; } = default!;
        public DbSet<StudentManagement.Models.Users> Users { get; set; } = default!;
        public DbSet<StudentManagement.Models.Accounts> Accounts { get; set; } = default!;
        public DbSet<StudentManagement.Models.Terms> Terms { get; set; } = default!;
        public DbSet<StudentManagement.Models.Groups> Groups { get; set; } = default!;
        public DbSet<StudentManagement.Models.Group_term> Group_term { get; set; } = default!;
        public DbSet<StudentManagement.Models.Group_student> Group_student { get; set; } = default!;
    }
}
