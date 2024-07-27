using System;
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class Users
    {
        public int Id { get; set; }

        public string Extra_code { get; set; }

        public string Fist_name { get; set; }

        public string Last_name { get; set; }

        public string Full_name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateOnly Birthday { get; set; }

        public byte Gender { get; set; }

        public string GenderText
        {
            get
            {
                return Gender == 1 ? "Man" : "Woman";
            }
        }

        public string Avatar { get; set; }

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
