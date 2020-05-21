using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo.FlightBuilder
{
    public class MyFlightBuilder : IFlightBuilder
    {
        private struct Location
        {
            public double latitude;
            public double longitude;
        }

        private Flight flight = new Flight();
        private Location location;

        public Flight GetFlight()
        {
            return this.flight;
        }

        public void SetCompany_Name(string name)
        {
            flight.Company_Name = name;
        }

        public void SetDate_Time()
        {
            this.flight.Date_Time = DateTime.UtcNow;
        }

        public void SetFlight_Id(string id)
        {
            flight.Flight_Id = id;
        }

        public void SetIs_External(bool isExternal)
        {
            flight.Is_External = isExternal;
        }

        public void SetLatitude(FlightPlan flightPlan)
        {
            CalculateLinearInterpolation(flightPlan);
            flight.Latitude = location.latitude;
        }

        public void SetLongitude(FlightPlan flightPlan)
        {
            flight.Longitude = location.longitude;
        }

        public void SetPassengers(int passengers)
        {
            flight.Passengers = passengers;
        }

        private void CalculateLinearInterpolation(FlightPlan flightPlan)
        {
            DateTime time = flightPlan.Initial_Location.Date_Time;            
            LinkedList<Segment> segments = flightPlan.Segments;
            Segment seg = segments.ElementAt(0), prevSeg;
            int i = 0;

            while ((i < segments.Count) && (time.AddSeconds(seg.TimeSpan_Seconds) < DateTime.UtcNow))
            {
                i++;
                seg = segments.ElementAt(i);
            }
            if (i == 0)
            {
                prevSeg = new Segment();
                prevSeg.Latitude = flightPlan.Initial_Location.Latitude;
                prevSeg.Longitude = flightPlan.Initial_Location.Longitude;

            } else {
                prevSeg = segments.ElementAt(i - 1);
            }

            TimeSpan sub = time.Subtract(DateTime.UtcNow);
            TimeSpan relation = sub.Divide(seg.TimeSpan_Seconds);

            location.latitude = prevSeg.Latitude + (seg.Latitude - prevSeg.Latitude) * relation.TotalSeconds;
            location.longitude = prevSeg.Longitude + (seg.Longitude - prevSeg.Longitude) * relation.TotalSeconds;
        }
    }
}
