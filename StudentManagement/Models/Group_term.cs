using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Group_term
    {
        public int Id { get; set; }
        [ForeignKey("Terms")]
        public int Term_id { get; set; }
        [ForeignKey("Groups")]
        public int Group_id { get; set; }

        public virtual Terms? Terms { get; set; }

        public virtual Groups? Groups { get; set; }
    }
}
