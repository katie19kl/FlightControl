using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Collections.Concurrent;
using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.FlightInfo.FlightBuilder;
using FlightControlWeb.Models.Servers;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private IFlightPlanManager planManager;
        private IServersManager serversManager;
        private static readonly HttpClient client = new HttpClient();
        

        public FlightsController(IFlightPlanManager manager, IServersManager manager1)
        {
            this.planManager = manager;
            this.serversManager = manager1;
        }

        // GET: api/Flights
        [HttpGet]
        public IEnumerable<Flight> Get(DateTime relative_To)
        {
            List<Flight> flights = GetInternalRelativeFlights(relative_To);
         
            if (Request.QueryString.Value.Contains("sync_all"))
            {
                flights.AddRange(GetExternalFlights(relative_To));
            }

            return flights;
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

        private List<Flight> GetInternalRelativeFlights(DateTime relative_To)
        {
            DateTime time = relative_To.ToUniversalTime();
            FlightCreator creator;
            ConcurrentDictionary<string, FlightPlan> flightPlansInfo = this.planManager.GetAllFlightPlans();
            List<Flight> flights = new List<Flight>();

            foreach (KeyValuePair<string, FlightPlan> entry in flightPlansInfo)
            {
                creator = new FlightCreator(new MyFlightBuilder());
                if (planManager.IsValidFlightPlan(entry.Value, time))
                {
                    creator.CreateFlight(entry.Value, entry.Key, time);
                    flights.Add(creator.GetFlight());
                }
            }

            return flights;
        }

        private List<Flight> GetExternalFlights(DateTime relative_To)
        {
            IEnumerable<Server> servers = this.serversManager.GetAllServers();

            return new List<Flight>();
        }
    }
}