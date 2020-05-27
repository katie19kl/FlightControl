using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo.FlightBuilder
{
    public interface IFlightBuilder
    {
        void SetFlight_Id(string s);

        void SetLatitude(FlightPlan flightPlan, DateTime relative_to);

        void SetLongitude(FlightPlan flightPlan, DateTime relative_to);

        void SetPassengers(int passengers);

        void SetCompany_Name(string name);

        void SetDate_Time();

        void SetIs_External(bool isExternal);

        Flight GetFlight();
    }
}
