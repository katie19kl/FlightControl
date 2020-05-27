using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo.FlightBuilder
{
    public class MyFlightBuilder : IFlightBuilder
    {
        private Flight flight = new Flight();

        private struct Location
        {
            public double longitude;
            public double latitude;
        }

        public Flight GetFlight()
        {
            return this.flight;
        }

        public void SetCompany_Name(string name)
        {
            flight.Company_Name = name;
        }


        public void SetFlight_Id(string id)
        {
            flight.Flight_Id = id;
        }

        public void SetIs_External(bool isExternal)
        {
            flight.Is_External = isExternal;
        }

        public void SetPassengers(int passengers)
        {
            flight.Passengers = passengers;
        }

        public void SetDate_Time()
        {
            flight.Date_Time = DateTime.UtcNow;
        }




        public void SetLatitude(FlightPlan flightPlanOutside, DateTime relative_To)
        {
            //flight.Latitude = latitude; // calculation needed!!
            flight.Latitude = CalculateLinInterpolation(flightPlanOutside, relative_To).latitude;
        }

        private Location CalculateLinInterpolation(FlightPlan flightPlan, DateTime relative_To)
        {

            Location toReturn;
            //////////////////////////////maybe problem ibo they are pointing to the same place 
            DateTime cummultTime = flightPlan.Initial_Location.Date_Time;
            LinkedList<Segment> segments = flightPlan.Segments;
            int index = 0;
            double relation;
            Segment cur = new Segment();
            Segment prev = new Segment();

            DateTime prevTimeCum = flightPlan.Initial_Location.Date_Time;

            while (index < segments.Count)
            {
                cummultTime = cummultTime.AddSeconds(segments.ElementAt(index).TimeSpan_Seconds);
                if (index >= 1)
                {
                    prevTimeCum = prevTimeCum.AddSeconds(segments.ElementAt(index - 1).TimeSpan_Seconds);
                }


                index += 1;


                if (cummultTime >= relative_To)
                {
                    break;
                }
            }

            if (index > 1)
            {
                cur = segments.ElementAt(index - 1);
                prev = segments.ElementAt(index - 2);
                //TimeSpan timeSpanDif = cummultTime.Subtract(relative_To);

                // where is in line
                /*                TimeSpan timeSpanDif = relative_To.Subtract(prevTimeCum);

                                //len of line
                                TimeSpan ofLastSegment = TimeSpan.FromSeconds(cur.TimeSpan_Seconds);
                                relation = timeSpanDif / ofLastSegment;*/
                //start position

            }
            //there is only one index
            else
            {
                cur = segments.ElementAt(0);
                prev.Latitude = flightPlan.Initial_Location.Latitude;
                prev.Longitude = flightPlan.Initial_Location.Longitude;
                //TimeSpan timeSpanDif = cummultTime.Subtract(relative_To);

                //prev = new Segment();

            }
            TimeSpan timeSpanDif = relative_To.Subtract(prevTimeCum);

            TimeSpan ofLastSegment = TimeSpan.FromSeconds(cur.TimeSpan_Seconds);
            relation = timeSpanDif / ofLastSegment;
            //interpolation
            toReturn.longitude = prev.Longitude + (cur.Longitude - prev.Longitude) * Math.Abs(relation);
            toReturn.latitude = prev.Latitude + (cur.Latitude - prev.Latitude) * Math.Abs(relation);
            return toReturn;
        }


        public void SetLongitude(FlightPlan flightPlanOutside, DateTime relativeTo)
        {
            flight.Longitude = CalculateLinInterpolation(flightPlanOutside, relativeTo).longitude;
        }

    }
}
