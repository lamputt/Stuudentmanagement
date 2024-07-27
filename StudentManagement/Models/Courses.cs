using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Name_teacher { get; set; }

        [ForeignKey("Departments")]

        public int Department_id { get; set; }

        public byte Status { get; set; }
        public virtual Departments? Departments { get; set; }

        public string StatusText {

            get
            {
                return Status == 1 ? "Active" : "Deactive";
            }
        }

    }
}
