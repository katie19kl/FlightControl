using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class InitialLocation
    {
        public double Latitude { get; set; } = -200;

        public double Longitude { get; set; } = -200;

        public DateTime Date_Time { get; set; }
    }
}
