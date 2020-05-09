using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FlightControlWeb.Models.FlightInfo
{
    public class MyFlightPlanManager : IFlightPlanManager
    {

        private readonly ConcurrentDictionary<string, FlightPlan>
            flightPlansInfo = new ConcurrentDictionary<string, FlightPlan>();

        public FlightPlan AddFlightPlan(FlightPlan p)
        {
            // Generate a unique id:
            //string id = GenerateRandomId(p);
            Random rand = new Random();

            string id = rand.Next(0, 999999).ToString();

            if (this.flightPlansInfo.TryAdd(id, p))
            {
                return p;
            }

            return new FlightPlan().NullFlightPlan;
        }

        public bool DeleteFlightPlanById(string id)
        {
            // Check if a Flight Plan with the given id exists.
            return this.flightPlansInfo.TryRemove(id, out FlightPlan dummy);
        }

        public ConcurrentDictionary<string, FlightPlan> GetAllFlightPlans()
        {
            return this.flightPlansInfo;
        }

        public FlightPlan GetFlightPlanById(string id)
        {
            // Check if a Flight Plan with the given id exists.
            if (this.flightPlansInfo.ContainsKey(id))
            {
                return this.flightPlansInfo[id];
            }
            else
            {
                return new FlightPlan().NullFlightPlan;
            }
        }


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
                outputStr += flightPlan.Company_Name[0] + flightPlan.Company_Name[flightPlan.Company_Name.Length - 1];

            } else if (outputStr.Length > 4)
            {
                outputStr = outputStr.Substring(0, 4);
            }

            initialRange = initialRange.Substring(0, 8 - outputStr.Length);
            id += outputStr + rand.Next(0, Convert.ToInt32(initialRange));

            return id;
        }
    }
}
