using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagement.Models
{
    public class Accounts
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey ("Roles")] 


        public int Role_id { get; set; }

      
        [ForeignKey("Users")]

        public int User_id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public virtual Users ? Users  { get; set; }

        public virtual Roles? Roles { get; set; }

    }
}
