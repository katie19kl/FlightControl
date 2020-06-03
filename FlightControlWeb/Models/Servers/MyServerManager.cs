using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace FlightControlWeb.Models.Servers
{
    public class MyServerManager : IServersManager
    {
        private readonly IList<Server> serversInfo = new List<Server>();

        private readonly ConcurrentDictionary<string, Server>
            flightIdToServer = new ConcurrentDictionary<string, Server>();

        public bool AddFlightServerPair(string flightId, Server server)
        {

            // Check that indeed the server is the list of servers.
            Server myServer = 
                this.serversInfo.Where(x => x.ServerId == server.ServerId).FirstOrDefault();

            if (myServer != null)
            {
                if (this.flightIdToServer.TryAdd(flightId, server))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            // The server was removed(or wasn't added at all).
            else
            {
                return false;
            }
        }

        public Server AddServer(Server server)
        {
            // Check that a server with this id does not exist.
            Server IsServer =
                this.serversInfo.Where(x => x.ServerId == server.ServerId).FirstOrDefault();

            if (IsServer != null)
            {
                return Server.nullServer;
            }

            // Does not exist.
            serversInfo.Add(server);

            return server;
        }

        public bool DeleteServerById(string id)
        {

            try
            {
                Server server = this.serversInfo.Where(x => x.ServerId == id).FirstOrDefault();
                this.serversInfo.Remove(server);
                return true;
            } 

            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<Server> GetAllServers()
        {
            return this.serversInfo;
        }

        public Server GetServerIdByFlightId(string flightId)
        {

            if (this.flightIdToServer.ContainsKey(flightId))
            {
                return this.flightIdToServer[flightId];
            }

            else
            {

                // The server does not exist.
                return Server.nullServer;
            }
        }
    }
}
