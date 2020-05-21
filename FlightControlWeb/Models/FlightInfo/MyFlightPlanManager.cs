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

            return FlightPlan.nullFlightPlan;
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
                return FlightPlan.nullFlightPlan;
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

        public bool IsValidFlightPlan(FlightPlan flightPlan, DateTime dateTimeRelativeTo)
        {
            DateTime dateTimeCumm = flightPlan.Initial_Location.Date_Time;
            //if aircraft didnt take off yet -> take next flight plan
            if (flightPlan.Initial_Location.Date_Time > dateTimeRelativeTo)
                return false;

            //dateTimeCumm = flightPlan.Initial_Location.Date_Time;
            // Cummulate time spans and init time to get land time
            foreach (Segment segment in flightPlan.Segments)
            {
                dateTimeCumm = dateTimeCumm.AddSeconds(segment.TimeSpan_Seconds);

            }

            // if aircraft already landed -> take next flight plan
            if (dateTimeCumm < dateTimeRelativeTo)
                return false;

            return true;
        }

        /*public List<FlightPlan> GetRelevantFlightPlans(DateTime dateTimeRelativeTo)
        {
            List<FlightPlan> flightPlans = new List<FlightPlan>();
            DateTime dateTimeCumm;

            foreach (KeyValuePair<string, FlightPlan> entry in flightPlansInfo)
            {
                //if aircraft didnt take off yet -> take next flight plan
                if (entry.Value.Initial_Location.date_Time > dateTimeRelativeTo)
                    continue;

                dateTimeCumm = entry.Value.Initial_Location.date_Time;
                // Cummulate time spans and init time to get land time
                foreach (Segment segment in entry.Value.Segments)
                {
                    dateTimeCumm = dateTimeCumm.AddSeconds(segment.TimeSpan_Seconds);

                }
                // if aircraft already landed -> take next flight plan
                if (dateTimeCumm < dateTimeRelativeTo)
                    continue;

                flightPlans.Add(entry.Value);

            }

            return flightPlans;

        }*/
    }
}
