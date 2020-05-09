using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo
{
    public class FlightPlan
    {
        public struct InitialLocation
        {
            public double latitude;
            public double longitude;
            public DateTime date_Time;
        }

        // Null design pattern- initialize default object.
        public static readonly FlightPlan nullFlightPlan = new FlightPlan()
        {
            Company_Name = "",
            initial_Location = new InitialLocation() { latitude = 0, longitude = 0, date_Time = new DateTime() },
            Segments = new LinkedList<Segment>()
        };

        InitialLocation initial_Location;
        LinkedList<Segment> segments = new LinkedList<Segment>();

        public FlightPlan NullFlightPlan { get; }

        public int Passengers { get; set; }

        public string Company_Name { get; set; }

        public InitialLocation Initial_Location
        {
            get
            {
                return this.initial_Location;
            }

            set
            {
                this.initial_Location = value;
            }

        }

        public LinkedList<Segment> Segments
        {
            get
            {
                return this.segments;
            }

            set
            {
                this.segments = value;
            }
        }
    }
}
