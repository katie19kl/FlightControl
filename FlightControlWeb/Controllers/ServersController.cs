using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models.Servers;
using FlightControlWeb.Models;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {

        private IServersManager serversManager;

        /* Constructor. */
        public ServersController(IServersManager manager)
        {
            this.serversManager = manager;
        }

        // GET: api/Servers
        [HttpGet]
        public IEnumerable<Server> Get()
        {
            return this.serversManager.GetAllServers();
        }

        // POST: api/Servers
        [HttpPost]
        public ActionResult Post([FromBody] Server server)
        {
            JsonValidationChecker jsonChecker = new JsonValidationChecker();
            
            // Check if the json is valid.
            if (!jsonChecker.IsValidServer(server))
            {
                return new BadRequestResult();
            }

            Server myServer = this.serversManager.AddServer(server);

            if (myServer == Server.nullServer)
            {

                // An error has occured when trying to 
                // add the server(id exists already).
                return new BadRequestResult();
            }

            return new OkObjectResult(myServer);
        }

        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (this.serversManager.DeleteServerById(id))
            {
                return Ok();
            }

            // An error has occured when trying to 
            // Delete the server(id doesn't exist).
            return NotFound();
        }
    }
}
