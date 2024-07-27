using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Group_student
    {
        public int Id { get; set; }
        [ForeignKey("Accounts")]
        public int Student_id { get; set; }

        [ForeignKey("Groups")]
        public int Group_id { get; set; }

        [ForeignKey("Courses")]
        public int Course_id { get; set; }
        public virtual Accounts ? Accounts  { get; set; }
        public virtual Groups? Groups { get; set; }

        public virtual Courses ? Courses { get; set; }
        public byte Absent {  get; set; }

        public byte Present { get; set; }
    }
}
