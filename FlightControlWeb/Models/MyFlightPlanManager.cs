using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using FlightControlWeb.Models.Servers;
using FlightControlWeb.Models.FlightInfo.FlightBuilder;


namespace FlightControlWeb.Models.FlightInfo
{
    public class MyFlightPlanManager : IFlightPlanManager
    {

        private readonly ConcurrentDictionary<string, FlightPlan>
            flightPlansInfo = new ConcurrentDictionary<string, FlightPlan>();

        /* Adds a given flight plan to the manager. */
        public FlightPlan AddFlightPlan(FlightPlan p)
        {
            // Generate a unique id.
            string id = GenerateRandomId(p);

            if (this.flightPlansInfo.TryAdd(id, p))
            {
                return p;
            }

            // When trying to add a flightsplan with an existing id.
            return FlightPlan.NullFlightPlan;
        }

        /* 
         * Delete a flight plan given it's id.
         * Returns boolian according
         * to the result of the deletion attempt.
         */
        public bool DeleteFlightPlanById(string id)
        {
            // Check if a Flight Plan with the given id exists.
            return this.flightPlansInfo.TryRemove(id, out FlightPlan dummy);
        }

        /* 
         * Returns a concurrent dictionary which
         * maps id to its flight plan.
         */
        public ConcurrentDictionary<string, FlightPlan> GetAllFlightPlans()
        {
            return this.flightPlansInfo;
        }

        /* 
         * Returns a task of the type
         * EndDataOfFlightPlan for the clients
         * purposes.
         */
        public Task<EndDataOfFLightPlan> GetEndDataOfPlan(string id, Server serverWithId)
        {
            Server server = serverWithId;
            EndDataOfFLightPlan endPlan = new EndDataOfFLightPlan();
            endPlan.SegmentsPath = new List<Segment>();
            FlightPlan flPLan = GetFlightPlanById(id);

            if (flPLan == FlightPlan.NullFlightPlan)
            {

                FlightPlanRequestHandler planRequestHandler = new FlightPlanRequestHandler();

                if (server != Server.nullServer)
                {
                    flPLan = planRequestHandler.GetFlightPlan(server, id).Result;
                }

            }

            return Task.Run(() =>
            {
                return RunSettingEndObject(endPlan, flPLan);
            });
        }

        /* Returns a flight plan object given it id. */
        public FlightPlan GetFlightPlanById(string id)
        {
            // Check if a Flight Plan with the given id exists.
            if (this.flightPlansInfo.ContainsKey(id))
            {
                return this.flightPlansInfo[id];
            }

            // When the id doesn't exist.
            else
            {
                return FlightPlan.NullFlightPlan;
            }
        }

        /* 
         * Checks if a given flight plan object is
         * valid according to a date time given 
         * as a parameter.
         */
        public bool IsValidFlightPlan(FlightPlan flightPlan, DateTime dateTimeRelativeTo)
        {
            DateTime dateTimeCumm = flightPlan.Initial_Location.Date_Time;

            // If aircraft didnt take off yet -> take next flight plan
            if (dateTimeCumm > dateTimeRelativeTo)
            {
                return false;
            }

            // Cummulate time spans and initial time to get land time.
            foreach (Segment segment in flightPlan.Segments)
            {
                dateTimeCumm = dateTimeCumm.AddSeconds(segment.TimeSpan_Seconds);
            }

            // If aircraft already landed -> take next flight plan.
            if (dateTimeCumm < dateTimeRelativeTo)
            {
                return false;
            }

            return true;
        }

        /* 
         * Return a list of internal flights which are relevant
         * according to the date time given(relative_To).
         */
        public List<Flight> GetInternalFlights(DateTime relative_To)
        {
            List<Flight> listOfFlights = new List<Flight>();

            // Convert to UTC.
            DateTime time = relative_To.ToUniversalTime();
            FlightCreator creator;

            ConcurrentDictionary<string, FlightPlan> flightPlansInfo = GetAllFlightPlans();

            foreach (KeyValuePair<string, FlightPlan> pair in flightPlansInfo)
            {
                creator = new FlightCreator(new MyFlightBuilder());

                // Check if the flight is relevant to relative_To.
                if (IsValidFlightPlan(pair.Value, time))
                {

                    // Create a Flight object.
                    creator.CreateFlight(pair.Value, pair.Key, time);
                    listOfFlights.Add(creator.GetFlight());
                }

            }

            return listOfFlights;
        }

        /* Generates a unique id for a given flight plan object. */
        private string GenerateRandomId(FlightPlan flightPlan)
        {
            Random rand = new Random();
            string id = "", outputStr = "";
            string initialRange = "99999999";

            // Find all captial letters and concat them to id.
            outputStr = String.Concat(flightPlan.Company_Name.Where(x => Char.IsUpper(x)));

            if (outputStr.Length == 0)
            {
                // Default would be first and last char in the company name.
                outputStr += flightPlan.Company_Name[0] +
                    flightPlan.Company_Name[flightPlan.Company_Name.Length - 1];
            }

            else if (outputStr.Length > 4)
            {
                outputStr = outputStr.Substring(0, 4);
            }

            // Add random numbers in a range to fill for a total of 8 chars.
            initialRange = initialRange.Substring(0, 8 - outputStr.Length);
            id += outputStr + rand.Next(0, Convert.ToInt32(initialRange));

            return id;
        }

        /* Sets all properties to the EndOfFlightPlan object. */
        private EndDataOfFLightPlan RunSettingEndObject(EndDataOfFLightPlan endPlan,
            FlightPlan flPLan)
        {
            endPlan.CompanyName = flPLan.Company_Name;
            endPlan.StartLatitude = flPLan.Initial_Location.Latitude;
            endPlan.StartLongitude = flPLan.Initial_Location.Longitude;
            endPlan.TakeOffTime = flPLan.Initial_Location.Date_Time;
            endPlan.NumOfPassengers = flPLan.Passengers;

            LinkedList<Segment> listOfSeg = flPLan.Segments;

            endPlan.EndLatitude = endPlan.StartLatitude;
            endPlan.EndLongitude = endPlan.StartLongitude;
            endPlan.LandTime = endPlan.TakeOffTime;

            foreach (Segment item in listOfSeg)
            {
                endPlan.EndLatitude = item.Latitude;
                endPlan.EndLongitude = item.Longitude;
                endPlan.LandTime = endPlan.LandTime.AddSeconds(item.TimeSpan_Seconds);
                endPlan.SegmentsPath.Add(item);
            }

            return endPlan;
        }
    }
}
