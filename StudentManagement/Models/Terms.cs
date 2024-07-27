namespace StudentManagement.Models
{
    public class Terms
    {
       public int Id { get; set; }  
       public string Name { get; set; } 
        
        public DateOnly From_date { get; set; }

        public DateOnly To_date { get; set;}

        public byte Status {  get; set; }

        public string StatusText
        {
            get
            {
                return Status == 1 ? "Active" : "Deactive";
            }
        }

    }
}
