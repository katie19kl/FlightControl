using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.Servers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Nancy.Json;
using System.Net.Sockets;

namespace FlightControlWeb.Models
{
    public class MyRequestHandler : IRequestHandler
    {
        IServersManager serversManager;

        /* Constructor. */
        public MyRequestHandler(IServersManager manager)
        {
            this.serversManager = manager;
        }

        /* Returns all external flight from the given server(that are relevant). */
        public List<Flight> GetFlightFromServer(Server toGetFrom, DateTime relativeTo)
        {
            List<Flight> fromServer = new List<Flight>();

            try
            {
                var response = GetResponse(toGetFrom, relativeTo);
                response.Wait();
                HttpResponseMessage responseFromServer = response.Result;

                responseFromServer.EnsureSuccessStatusCode();
                Task<string> responseBody = responseFromServer.Content.ReadAsStringAsync();
                responseBody.Wait();
                string content = responseBody.Result;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                IEnumerable<Flight> flights =
                    javaScriptSerializer.Deserialize<IEnumerable<Flight>>(content);

                if (flights != null)
                {
                    fromServer = MaintainFlightsList(flights, toGetFrom);
                }
            } catch (HttpRequestException) { }
            catch (TaskCanceledException) { }
            catch (SocketException) { }
            catch (AggregateException) { }

            return fromServer;
        }

        /* Sends a request to the given server to receive its internal flights. */
        public async Task<HttpResponseMessage> GetResponse(Server toGetFrom,
            DateTime relative_To)
        {
            var client = new HttpClient();

            // Set time out for receiving the response.
            client.Timeout = TimeSpan.FromSeconds(15);

            relative_To = relative_To.AddHours(3);
            string url = toGetFrom.ServerUrl + "/api/Flights?relative_to=" + relative_To.ToString("s");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                return response;

            } catch (TaskCanceledException)
            {

                // There was a time out.
                throw new TaskCanceledException();
            }
        }

        /* Sets all the flights as externa,
         * adds them to the list of flights
         * and maps the flight id to the given
         * server.
         */
        private List<Flight> MaintainFlightsList(IEnumerable<Flight> flights, Server toGetFrom)
        {

            List<Flight> fromServer = new List<Flight>();

            foreach(var fl in flights)
            {

                // Set the flight to be external.
                fl.Is_External = true;

                // Add the mapping between this flight to the given server.
                this.serversManager.AddFlightServerPair(fl.Flight_Id, toGetFrom);
                fromServer.Add(fl);
            }

            return fromServer;
        }
    }
}
