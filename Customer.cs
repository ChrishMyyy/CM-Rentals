using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int LoyaltyPoints { get; set; }
        public string Status { get; set; }
        public DateTime DateRegistered { get; set; }
        public string LicenseImage { get; set; }
        public decimal AccumulatedAmount { get; set; }
        public string CustomerStatus { get; set; }

        // Relationships
        public List<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
