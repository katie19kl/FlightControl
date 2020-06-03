using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IRequestHandler
    {

        /* Get flights from the given server, relative to the given date time. */
        List<Flight> GetFlightFromServer(Server toGetFrom, DateTime relativeTo);

        /* Get response to a request sent to the given server. */
        Task <HttpResponseMessage> GetResponse(Server toGetFrom, DateTime relativeTo);
    }
}
