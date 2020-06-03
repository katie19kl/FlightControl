using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using FlightControlWeb.Models.Servers;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Models.FlightInfo
{
    public interface IFlightPlanManager
    {

        /* Returns all flights plan mapped to their ids. */
        ConcurrentDictionary<string, FlightPlan> GetAllFlightPlans();

        /* Returns the flight plan that corresponds to the given id. */
        FlightPlan GetFlightPlanById(string id);

        /* Adds the flight plan given. */
        FlightPlan AddFlightPlan(FlightPlan p);

        /* Checks if the flight plan is relevant given a date time. */
        bool IsValidFlightPlan(FlightPlan flightPlan, DateTime dateTimeRelativeTo);

        /* Deletes the flight plan with the given id. */
        bool DeleteFlightPlanById(string id);

        /* Returns a created object of type EndDataOfFlight. */
        Task<EndDataOfFLightPlan> GetEndDataOfPlan(string id , Server map);

        /* Returns internal flights relevant according to the given date time. */
        List<Flight> GetInternalFlights(DateTime relative_To);
    }
}
