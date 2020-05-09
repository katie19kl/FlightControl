using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models.Servers
{
    public class Server
    {

        // Null design pattern- initialize default object.
        public static readonly Server nullServer = new Server()
        {
            Server_Id = "",
            Server_Url = ""
        };

        public string Server_Id { get; set; }

        public string Server_Url { get; set; }
    }
}
