using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent
{
    public class Rental
    {
        public int RentalID { get; set; }
        public string RentalCode { get; set; }
        public string CustomerCode { get; set; }
        public string CarCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int DaysRented { get; set; }
        public decimal TotalAmount { get; set; }
        public string RentalStatus { get; set; }
        public string ProcessedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal TotalDiscount { get; set; }
        // Navigation properties
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public User ProcessedByUser { get; set; }
    }
}
