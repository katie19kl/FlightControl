using FlightControlWeb.Models.FlightInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Servers
{
    interface IPlanRequestHandler
    {

        /* Get the flight plan with the given id from the given server. */
        Task<FlightPlan> GetFlightPlan(Server s, string id);
    }
}
