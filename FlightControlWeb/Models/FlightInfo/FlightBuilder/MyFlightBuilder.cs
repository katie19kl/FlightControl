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
            this.flight.Date_Time = DateTime.UtcNow; // change format of date time!!!!!!!!!!!
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
            DateTime time = flightPlan.Initial_Location.date_Time;            
            LinkedList<Segment> segments = flightPlan.Segments;
            Segment seg = segments.ElementAt(0), prevSeg;
            int i = 0;

            while ((time.AddSeconds(seg.TimeSpan_Seconds) < DateTime.UtcNow) && (i < segments.Count))
            {
                i++;
                seg = segments.ElementAt(i);
            }
            if (i == 0)
            {
                prevSeg = new Segment();
                prevSeg.Latitude = flightPlan.Initial_Location.latitude;
                prevSeg.Longitude = flightPlan.Initial_Location.longitude;
            } else {
                prevSeg = segments.ElementAt(i - 1);
            }

            TimeSpan relation = DateTime.UtcNow.Subtract(time).Divide(seg.TimeSpan_Seconds);
            location.latitude = (seg.Latitude - prevSeg.Latitude) * relation.TotalSeconds;
            location.longitude = (seg.Longitude = prevSeg.Longitude) * relation.TotalSeconds;
        }
    }
}
