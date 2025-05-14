using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent
{
    public class Vehicle
    {
        public int CarID { get; set; }
        public string CarCode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string PlateNumber { get; set; }
        public decimal RentalPrice { get; set; }
        public string Transmission { get; set; }
        public string FuelType { get; set; }
        public string VehicleType { get; set; }
        public string Availability { get; set; }
        public string VehicleStatus { get; set; }
        public DateTime DateAdded { get; set; }

        // Relationships
        public List<Rental> Rentals { get; set; } = new List<Rental>();
        public List<VehicleImage> VehicleImages { get; set; } = new List<VehicleImage>();
    }
}
