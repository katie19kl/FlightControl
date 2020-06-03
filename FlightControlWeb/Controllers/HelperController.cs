using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.Servers;
using Microsoft.AspNetCore.Mvc;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : Controller
    {

        private IFlightPlanManager flightPlanManager;
        private IServersManager serversManager;

        /* Constructor. */
        public HelperController(IFlightPlanManager planManager,
            IServersManager serversGetManager)
        {
            this.flightPlanManager = planManager;
            serversManager = serversGetManager;
        }

        [HttpGet("{id}", Name = "GetData")]
        public async Task<EndDataOfFLightPlan> GetData(string id)
        {
            Server server = serversManager.GetServerIdByFlightId(id);
            
            // Ask an external server for a flightPlan to
            // create an EndDataOfFlight object.
            var endData = await flightPlanManager.GetEndDataOfPlan(id, server);
            return endData;
        }

    }
}