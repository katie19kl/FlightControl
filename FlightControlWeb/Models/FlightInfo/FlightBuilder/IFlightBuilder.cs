using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo.FlightBuilder
{
    public interface IFlightBuilder
    {
        void SetFlight_Id(string s);

        void SetLatitude(double latitude);

        void SetLongitude(double longitude);

        void SetPassengers(int passengers);

        void SetCompany_Name(string name);

        void SetDate_Time(DateTime time);

        void SetIs_External(bool isExternal);

        Flight GetFlight();
    }
}
