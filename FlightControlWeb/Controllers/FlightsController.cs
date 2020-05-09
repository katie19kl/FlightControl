using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;
using System.Collections.Concurrent;
using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.FlightInfo.FlightBuilder;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        IFlightPlanManager planManager;

        public FlightsController(IFlightPlanManager manager)
        {
            this.planManager = manager;
        }

        // GET: api/Flights
        [HttpGet]
        public IEnumerable<Flight> Get(DateTime relative_To)
        {
            FlightCreator creator = new FlightCreator(new MyFlightBuilder());
            ConcurrentDictionary<string, FlightPlan> flightPlansInfo = this.planManager.GetAllFlightPlans();
            
        }

        // GET: api/Flights/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<Flight> Get([FromBody] DateTime relative_to, string sync_all)
        {
            return "value";
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (this.planManager.DeleteFlightPlanById(id))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
