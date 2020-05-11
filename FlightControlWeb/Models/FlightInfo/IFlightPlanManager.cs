using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FlightControlWeb.Models.FlightInfo
{
    public interface IFlightPlanManager
    {

        ConcurrentDictionary<string, FlightPlan> GetAllFlightPlans();

        FlightPlan GetFlightPlanById(string id);

        FlightPlan AddFlightPlan(FlightPlan p);

        bool DeleteFlightPlanById(string id);

        public bool IsValidFlightPlan(FlightPlan flightPlan, DateTime dateTimeRelativeTo);

        //List<FlightPlan> GetRelevantFlightPlans(DateTime dateTime);
    }
}
