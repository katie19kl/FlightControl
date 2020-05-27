using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using FlightControlWeb.Models.FlightInfo;
using Microsoft.AspNetCore.Mvc;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : Controller
    {

        private IFlightPlanManager flightPlanManager;

        public HelperController(IFlightPlanManager planManager)
        {
            this.flightPlanManager = planManager;
        }

        [HttpGet("{id}", Name = "GetData")]
        public EndDataOfFLightPlan GetData(string id)
        {
            return this.flightPlanManager.GetEndDataOfPlan(id);
        }

    }
}
