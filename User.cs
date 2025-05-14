using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent
{
    public class User
    {
        public int UserID { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public string UserImage { get; set; }

        // Relationships
        public List<Rental> ProcessedRentals { get; set; } = new List<Rental>();
    }
}
