using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent
{
    public class VehicleImage
    {
        public int ImageID { get; set; }
        public string CarCode { get; set; }
        public string ImagePath { get; set; }
        public string ImageLabel { get; set; }
        // Navigation property
        public Vehicle Vehicle { get; set; }
    }
}
