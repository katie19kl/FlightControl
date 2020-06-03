using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;
using FlightControlWeb.Models.FlightInfo;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {

        private IFlightPlanManager flightPlanManager;

        /* Constructor. */
        public FlightPlanController(IFlightPlanManager planManager)
        {
            this.flightPlanManager = planManager;
        }

        // GET: api/FlightPlan/{id}
        [HttpGet("{id}", Name = "Get")]
        public FlightPlan Get(string id)
        {
            return this.flightPlanManager.GetFlightPlanById(id);
        }

        // POST: api/FlightPlan
        [HttpPost]
        public ActionResult Post([FromBody] FlightPlan flightPlan)
        {
            JsonValidationChecker jsonChecker = new JsonValidationChecker();

            // Check if the json file is valid.
            if (!jsonChecker.IsValidFlightPlan(flightPlan))
            {
                return new BadRequestResult();
            }

            FlightPlan fp = this.flightPlanManager.AddFlightPlan(flightPlan);

            if (fp == FlightPlan.NullFlightPlan)
            {

                // There was error when trying to add the given flight plan.
                return new BadRequestResult();
            } 
            
            else
            {
                return new OkObjectResult(fp);
            }
        }
    }
}
