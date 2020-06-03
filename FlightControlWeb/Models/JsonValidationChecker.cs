using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.Servers;


namespace FlightControlWeb.Models
{
    public class JsonValidationChecker
    {

        /* Checks if the given flight plan is valid. */
        public bool IsValidFlightPlan(FlightPlan plan)
        {

            if (plan.Passengers == -1 || plan.Passengers < 0)
            {
                return false;
            }

            if (plan.Company_Name == null)
            {
                return false;
            }

            if (!CheckInitialLocationIsValid(plan.Initial_Location))
            {
                return false;
            }

            if (!CheckSegmentsIfValid(plan.Segments))
            {
                return false;
            }

            return true;

        }

        /* Check if the given server object is valid. */
        public bool IsValidServer(Server server)
        {
            if (server.ServerId == null)
            {
                return false;
            }

            if (server.ServerUrl == null)
            {
                return false;
            }

            return true;
        }

        /* Helps check validation of Initial Location in the given flight plan. */
        private bool CheckInitialLocationIsValid(InitialLocation initial)
        {

            // Missing the whole object.
            if (initial == null)
            {
                return false;
            }

            if ((initial.Latitude == -200) || (initial.Longitude == -200) 
                || (initial.Date_Time == default))
            {
                return false;
            }

            if (initial.Latitude < -90 || initial.Latitude > 90)
            {
                return false;
            }

            if (initial.Longitude < -180 || initial.Longitude > 180)
            {
                return false;
            }

            // Initial location is valid.
            return true;
        }

        /* Check validation of given segments in the flight plan. */
        private bool CheckSegmentsIfValid(LinkedList<Segment> segments)
        {

            // Missing the whole object.
            if (segments == null)
            {
                return false;
            }

            foreach(Segment seg in segments)
            {
                if ((seg.Latitude == -200) || (seg.Longitude == -200)
                    || (seg.TimeSpan_Seconds == -1))
                {
                    return false;
                }
                if (seg.Latitude < -90 || seg.Latitude > 90)
                {
                    return false;
                }
                if (seg.Longitude < -180 || seg.Longitude > 180)
                {
                    return false;
                }
                if (seg.TimeSpan_Seconds < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
