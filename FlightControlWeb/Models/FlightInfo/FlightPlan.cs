using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo
{
    public class FlightPlan
    {
        /*public struct InitialLocation
        {
            public double latitude;
            public double longitude;
            public DateTime date_Time;
        }*/

        // Null design pattern- initialize default object.
        public static readonly FlightPlan nullFlightPlan = new FlightPlan()
        {
            Company_Name = "",
            Initial_Location = new InitialLocation() { Latitude = 0, Longitude = 0, Date_Time = new DateTime() },
            Segments = new LinkedList<Segment>()
        };

        public int Passengers { get; set; }

        public string Company_Name { get; set; }

        public InitialLocation Initial_Location { get; set; }

        public LinkedList<Segment> Segments { get; set; } = new LinkedList<Segment>();
    }
}
