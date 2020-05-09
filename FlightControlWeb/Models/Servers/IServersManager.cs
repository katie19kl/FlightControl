using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Servers
{
    public interface IServersManager
    {
        IEnumerable<Server> GetAllServers();

        Server AddServer(Server server);

        bool DeleteServerById(string id);
    }
}
