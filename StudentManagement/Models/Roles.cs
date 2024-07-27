namespace StudentManagement.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Byte Status { get; set; }

        public string StatusText
        {
            get
            {
                return Status == 1 ? "Active" : "Deactive";
            }
        }
    }
}
