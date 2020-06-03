using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.FlightInfo.FlightBuilder
{
    public class FlightCreator
    {
        private IFlightBuilder flightBuilder;

        /* Constructor. */
        public FlightCreator(IFlightBuilder builder)
        {
            this.flightBuilder = builder;
        }

        /* Creates the flight given a flight plan, its id and a date time. */
        public void CreateFlight(FlightPlan flightPlan, string id, DateTime relativeToUTC)
        {
            flightBuilder.SetCompany_Name(flightPlan.Company_Name);
          
            flightBuilder.SetFlight_Id(id);

            // Necessarily an internal flight.
            flightBuilder.SetIs_External(false);
            
            flightBuilder.SetPassengers(flightPlan.Passengers);

            flightBuilder.SetDate_Time(relativeToUTC);

            flightBuilder.SetLongitude(flightPlan,relativeToUTC);

            flightBuilder.SetLatitude(flightPlan, relativeToUTC);
        }

        /* Returns the created flight. */
        public Flight GetFlight()
        {
            return flightBuilder.GetFlight();
        }
    }
}
