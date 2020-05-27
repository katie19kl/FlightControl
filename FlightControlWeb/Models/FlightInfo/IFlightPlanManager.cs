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

        //List<FlightPlan> GetRelevantFlightPlans(DateTime dateTime);

        bool IsValidFlightPlan(FlightPlan flightPlan, DateTime dateTimeRelativeTo);

        bool DeleteFlightPlanById(string id);

        EndDataOfFLightPlan GetEndDataOfPlan(string id);

        //LinkedList<Segment> ListOfSmallerSegments(LinkedList<Segment> orinialSegments, string id);
    }
}
