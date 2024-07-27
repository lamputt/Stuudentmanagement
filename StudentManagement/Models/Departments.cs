using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Departments
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Leader { get; set; }

        public DateOnly Begining_date { get; set; }

        public byte Status { get; set; }

        public string StatusText
        {
            get
            {
                return Status == 1 ? "Active" : "Deactive";
            }
        }




    }
}
