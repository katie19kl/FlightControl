using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo
{
    public class Flight
    {
        public string Flight_Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Passengers { get; set; }

        public string Company_Name { get; set; }

        public DateTime Date_Time { get; set; }

        public bool Is_External { get; set; }

        public DateTime Relative_To { get; set; }
    }
}
