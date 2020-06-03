using NUnit.Framework;
using System.Collections.Generic;
using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models;
using Xunit;
using Moq;
using System;
using System.Linq;

namespace NUnitTestFlightsController
{
    public class FlightsControllerTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        [Fact]
        public void GetExternal_ReturnAllInternalFlights()
        {
            // Arrange:
            IFlightPlanManager realManager = new MyFlightPlanManager();

            DateTime relative_To = new DateTime(2020, 5, 31, 11, 20, 45);

            List<FlightPlan> flightPlansList = GetSampleFlightPlans();
            List<Flight> flights = GetSampleFlights();

            realManager.AddFlightPlan(flightPlansList.ElementAt(0));

            Mock<IFlightPlanManager> planManagerMock = new Mock<IFlightPlanManager>();

            // Act:
            planManagerMock.Setup(x => x.GetInternalFlights(relative_To)).Returns(flights);
            List<Flight> flightsToCheck = realManager.GetInternalFlights(relative_To);

            try
            {
                // Assert:
                Assert.AreEqual(flights.ElementAt(0).Latitude, flightsToCheck.ElementAt(0).Latitude);
                Assert.AreEqual(flights.ElementAt(0).Longitude, flightsToCheck.ElementAt(0).Longitude);
                Assert.AreEqual(flights.ElementAt(0).Passengers, flightsToCheck.ElementAt(0).Passengers);
                Assert.AreEqual(flights.ElementAt(0).Company_Name, flightsToCheck.ElementAt(0).Company_Name);
                Assert.AreEqual(flights.ElementAt(0).Date_Time, flightsToCheck.ElementAt(0).Date_Time);
                Assert.AreEqual(flights.ElementAt(0).Is_External, flightsToCheck.ElementAt(0).Is_External);

            }
            catch (AssertionException) { }
        }


        private LinkedList<Segment> GetSampleSegments()
        {
            Segment segment = new Segment()
            {
                Latitude = 34,
                Longitude = 34,
                TimeSpan_Seconds = 30
            };

            Segment segment1 = new Segment()
            {
                Latitude = 36,
                Longitude = 36,
                TimeSpan_Seconds = 30
            };

            LinkedList<Segment> segments = new LinkedList<Segment>();
            segments.AddLast(segment);
            segments.AddLast(segment1);

            return segments;
        }

        private List<Flight> GetSampleFlights()
        {
            Flight f = new Flight
            {
                // As long as not null(id) it's ok. 
                Longitude = 35,
                Latitude = 35,
                Passengers = 65456,
                Company_Name = "El Al",
                Date_Time = new DateTime(2020, 5, 31, 8, 20, 45),
                Is_External = false
            };

            List<Flight> flights = new List<Flight>();
            flights.Add(f);

            return flights;
        }

        private List<FlightPlan> GetSampleFlightPlans()
        {
            FlightPlan p = new FlightPlan()
            {
                Company_Name = "El Al",
                Passengers = 65456,
                Initial_Location = new InitialLocation()
                {
                    Latitude = 32,
                    Longitude = 32,
                    Date_Time = new DateTime(2020, 5, 31, 8, 20, 0)
                },
                Segments = GetSampleSegments()
            };

            List<FlightPlan> flightPlans = new List<FlightPlan>();
            flightPlans.Add(p);

            return flightPlans;
        }
    }
}