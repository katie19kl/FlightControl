using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {

        private IFlightPlanManager flightPlanManager;

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
        public FlightPlan Post([FromBody] FlightPlan flightPlan)
        {
            return this.flightPlanManager.AddFlightPlan(flightPlan);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (this.flightPlanManager.DeleteFlightPlanById(id))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
