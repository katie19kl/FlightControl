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

        public Server AddServer(Server server)
        {
            serversInfo.Add(server);

            return server;
        }

        public bool DeleteServerById(string id)
        {
            try
            {
                Server server = this.serversInfo.Where(x => x.Server_Id == id).FirstOrDefault();
                this.serversInfo.Remove(server);
                return true;
            } 
            catch (System.InvalidOperationException)
            {
                return false;
            }
        }

        public IEnumerable<Server> GetAllServers()
        {
            return this.serversInfo;
        }
    }
}
