using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo.FlightBuilder
{
    public class FlightCreator
    {
        private IFlightBuilder flightBuilder;

        public FlightCreator(IFlightBuilder builder)
        {
            this.flightBuilder = builder;
        }

        public void CreateFlight(FlightPlan flightPlan, string id)
        {
            flightBuilder.SetCompany_Name(flightPlan.Company_Name);
            flightBuilder.SetDate_Time(flightPlan.Initial_Location.date_Time); // where should the calculation take place?
            flightBuilder.SetFlight_Id(id);
            //flightBuilder.SetIs_External(); // how to find out?
            flightBuilder.SetLatitude(flightPlan.Initial_Location.latitude); // requires calculation(may also need segments)
            flightBuilder.SetLongitude(flightPlan.Initial_Location.longitude); // requires calculation(may also need segments)
            flightBuilder.SetPassengers(flightPlan.Passengers);
        }

        public Flight GetFlight()
        {
            return flightBuilder.GetFlight();
        }
    }
}
