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

            flightBuilder.SetDate_Time();

            flightBuilder.SetFlight_Id(id);

            flightBuilder.SetIs_External(false); // Since it is an internal flight.

            flightBuilder.SetLatitude(flightPlan);

            flightBuilder.SetLongitude(flightPlan);

            flightBuilder.SetPassengers(flightPlan.Passengers);
        }

        public Flight GetFlight()
        {
            return flightBuilder.GetFlight();
        }
    }
}
