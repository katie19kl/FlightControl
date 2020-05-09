using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models.Servers;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {

        private IServersManager serversManager;

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
        public Server Post([FromBody] Server server)
        {
            return this.serversManager.AddServer(server);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (this.serversManager.DeleteServerById(id))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}
