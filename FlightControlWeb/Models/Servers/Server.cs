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
            ServerId = "",
            ServerUrl = ""
        };

        public string ServerId { get; set; } = null;

        public string ServerUrl { get; set; } = null;
    }
}
