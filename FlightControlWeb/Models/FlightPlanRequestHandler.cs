using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.Servers;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public  class FlightPlanRequestHandler : IPlanRequestHandler
    {

        public Task<FlightPlan> GetFlightPlan(Server serverToRequestFrom, string id)
        {

            // Asks a server to receive the flight plan with the given id.
            return Task.Run( () => GetFlightFromServer(serverToRequestFrom, id) );
        }

        /* 
         * Connects to the server and sends a request for the
         * flight plan with the given id. The flight plan necesserily
         * belongs to this server.
         */
        public FlightPlan GetFlightFromServer(Server toGetFrom, string id)
        {
            FlightPlan fromServerFlightPlan = FlightPlan.NullFlightPlan;

            try
            {
                var response = GetResponse(toGetFrom, id);
                response.Wait();
                HttpResponseMessage responseFromServer = response.Result;
                responseFromServer.EnsureSuccessStatusCode();
                Task<string> responseBody = responseFromServer.Content.ReadAsStringAsync();
                responseBody.Wait();
                string content = responseBody.Result;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                fromServerFlightPlan = javaScriptSerializer.Deserialize<FlightPlan>(content);
                dynamic response_d = JsonConvert.DeserializeObject(content);
                LinkedList<Segment> segments = response_d.segments.ToObject<LinkedList<Segment>>();
                fromServerFlightPlan.Segments = segments;

                return fromServerFlightPlan;
            } catch (HttpRequestException)
            {
                fromServerFlightPlan = FlightPlan.NullFlightPlan;
                fromServerFlightPlan.Passengers = -2;
            }
            catch (SocketException) { }
            catch (AggregateException) { }

            return fromServerFlightPlan;
        }

        /* Sends the request and gets the response from the server. */
        public async Task<HttpResponseMessage> GetResponse(Server toGetFrom, string id)
        {
            var client = new HttpClient();

            // The time out for receiving response.
            client.Timeout = TimeSpan.FromSeconds(15);

            string url = toGetFrom.ServerUrl + "/api/FlightPlan/" + id;

            try
            {

                // Wait for the response.
                HttpResponseMessage response = await client.GetAsync(url);

                return response;

            } catch (TaskCanceledException)
            {

                // There was a timeout.
                return new HttpResponseMessage(statusCode: System.Net.HttpStatusCode.NotFound);
            }
        }
    }
}
