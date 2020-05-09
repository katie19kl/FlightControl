using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo.FlightBuilder
{
    public class MyFlightBuilder : IFlightBuilder
    {
        private Flight flight = new Flight();

        public Flight GetFlight()
        {
            return this.flight;
        }

        public void SetCompany_Name(string name)
        {
            flight.Company_Name = name;
        }

        public void SetDate_Time(DateTime time)
        {
            flight.Date_Time = time; // calculation needed!!
        }

        public void SetFlight_Id(string id)
        {
            flight.Flight_Id = id;
        }

        public void SetIs_External(bool isExternal)
        {
            flight.Is_External = isExternal;
        }

        public void SetLatitude(double latitude)
        {
            flight.Latitude = latitude; // calculation needed!!
        }

        public void SetLongitude(double longitude)
        {
            flight.Longitude = longitude; // calculation needed!!
        }

        public void SetPassengers(int passengers)
        {
            flight.Passengers = passengers;
        }
    }
}
