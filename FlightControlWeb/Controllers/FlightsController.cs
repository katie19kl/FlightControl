using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;
using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.Servers;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        IFlightPlanManager planManager;
        IServersManager serversManager;
        IRequestHandler requestHandler;

        /* Constructor. */
        public FlightsController(IFlightPlanManager manager,
            IServersManager serverManager,
            IRequestHandler handler)
        {
            this.serversManager = serverManager;
            this.planManager = manager;
            this.requestHandler = handler;
        }

        
        [HttpGet]
        public async Task<IEnumerable<Flight>> GetExternal(DateTime relative_To)
        {
             return await Task.Run(() => GetFlights(relative_To));

        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (this.planManager.DeleteFlightPlanById(id))
            {
                return Ok();
            }

            // In case the deletion has failed(id doesn't exist).
            return NotFound();
        }

        private IEnumerable<Flight> GetFlights(DateTime relative_To)
        {
            DateTime time = relative_To.ToUniversalTime();
            bool sync_all = true;
            if (Request != null)
            {
                sync_all = Request.QueryString.Value.Contains("sync_all");
            }
            if (sync_all)
            {
                IEnumerable<Server> listOfServer = serversManager.GetAllServers();
                List<Flight> listOfFlights = this.planManager.GetInternalFlights(relative_To);

                foreach (var server in listOfServer)
                {
                    List<Flight> listFromServer = new List<Flight>();
                    listFromServer = requestHandler.GetFlightFromServer(server, time);

                    foreach (var flight in listFromServer)
                    {
                        listOfFlights.Add(flight);
                    }
                }
                return listOfFlights;
            }
            else
            {
                List<Flight> listOfFlights = this.planManager.GetInternalFlights(relative_To);
                return listOfFlights;
            }
        }
    }
}
