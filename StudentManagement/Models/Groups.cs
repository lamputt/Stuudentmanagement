using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Groups
    {
        public int Id { get; set; }

        [ForeignKey("Departments")]
        public int Department_id { get; set; }

        public string Name { get; set; }
        public int Student_number { get; set; }

        [ForeignKey("Terms")]
        public int Term_id { get; set; }

        [ForeignKey("Accounts")]
        public int Teacher_id { get; set; } 

        public byte Status { get; set; }

        public virtual Departments? Departments { get; set; }
        public virtual Terms? Terms { get; set; }
        public virtual Accounts? Accounts { get; set; } 

        public string StatusText
        {
            get
            {
                return Status == 1 ? "Active" : "Deactive";
            }
        }
    }
}
