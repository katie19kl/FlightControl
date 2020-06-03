using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Servers
{
    public interface IServersManager
    {
        /* Returns all servers. */
        IEnumerable<Server> GetAllServers();

        /* Adds a server. */
        Server AddServer(Server server);

        /* Deletes a server given its id. */
        bool DeleteServerById(string id);

        /* Add a mapping between a flight id to the server it was created in. */
        bool AddFlightServerPair(string flightId, Server server);

        /* Return the server that corresponds to the flight id. */
        Server GetServerIdByFlightId(string flightId);
    }
}
