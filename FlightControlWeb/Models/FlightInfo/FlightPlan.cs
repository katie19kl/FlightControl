using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo
{
    [Serializable()]
    public class FlightPlan
    {

        // Null design pattern- initialize default object.
        public static readonly FlightPlan NullFlightPlan = new FlightPlan()
        {
            Company_Name = "",
            Initial_Location = new InitialLocation() 
            {
                Latitude = 0,
                Longitude = 0, 
                Date_Time = new DateTime() 
            },
            Segments = new LinkedList<Segment>()
        };

        public LinkedList<Segment> Segments { get; set; } = null;

        public int Passengers { get; set; } = -1;

        public string Company_Name { get; set; } = null;

        public InitialLocation Initial_Location { get; set; } = null;
    }
}
