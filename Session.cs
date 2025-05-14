using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent
{
    public static class Session
    {
        public static string CurrentUserCode { get; set; }
        public static string CurrentUserFullName { get; set; }
        public static string CurrentUserImagePath { get; set; }
        public static string CurrentUserRole { get; set; }
    }
}
