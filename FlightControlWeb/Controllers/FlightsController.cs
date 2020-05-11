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
            FlightCreator creator;
            ConcurrentDictionary<string, FlightPlan> flightPlansInfo = this.planManager.GetAllFlightPlans();
            //List<FlightPlan> relevantFlightPlans = this.planManager.GetRelevantFlightPlans(relative_To);
            List<Flight> flights = new List<Flight>();
            
            foreach(KeyValuePair<string, FlightPlan> entry in flightPlansInfo)
            {
                creator = new FlightCreator(new MyFlightBuilder());
                if (planManager.IsValidFlightPlan(entry.Value, relative_To))
                {
                    creator.CreateFlight(entry.Value, entry.Key);
                    flights.Add(creator.GetFlight());
                }
            }

            return flights;
        }

        /*// GET: api/Flights/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<Flight> Get([FromBody] DateTime relative_to, string sync_all)
        {
            return new List<Flight>();
        }*/

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
