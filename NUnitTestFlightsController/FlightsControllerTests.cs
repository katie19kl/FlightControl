using System;
using System.Collections.Generic;
using FlightControlWeb.Models.FlightInfo;
using FlightControlWeb.Models.Servers;
using FlightControlWeb.Models;
using Moq;
using NUnit.Framework;
using Xunit;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;
using Xunit.Sdk;


namespace FlightControlWeb.Controllers.Tests
{

    [TestClass()]
    public class FlightsControllerTests
    {

        // Actual objects. 
        FlightsController flightPlanController;
        List<Server> TwoServers;

        // Mocked objects.
        Mock<IRequestHandler> mockRequestHandler;
        Mock<IServersManager> mockServerManager;
        Mock<IFlightPlanManager> mockFlightPlanManager;

        [SetUp]
        public void SetUp()
        {

            // Expected server. 
            TwoServers = new List<Server>();

            TwoServers.Add(new Server { ServerId = "1", ServerUrl = "https://localhost:44315", });
            TwoServers.Add(new Server { ServerId = "2", ServerUrl = "https://localhost:44314", });

            mockRequestHandler = new Mock<IRequestHandler>();
            mockServerManager = new Mock<IServersManager>();
            mockFlightPlanManager = new Mock<IFlightPlanManager>();

            flightPlanController = new FlightsController(mockFlightPlanManager.Object,
                mockServerManager.Object, mockRequestHandler.Object);
        }

        [Test]
        [Fact]
        public void GetExternalTest()
        {
            DateTime dateTime = DateTime.Now;
            mockServerManager.Setup(x => x.GetAllServers()).Returns(TwoServers);
            mockFlightPlanManager.Setup(x => x.GetInternalFlights(dateTime)).Returns(InternalFlights());
            mockRequestHandler.Setup(x => x.GetFlightFromServer(It.IsAny<Server>(),
                It.IsAny<DateTime>())).Returns(GetExternalFromServers());
            IEnumerable<Flight> lstController = flightPlanController.GetExternal(dateTime).Result;
            List<Flight> internalFlights = InternalFlights();
            List<Flight> externalFlights = GetExternalFromServers();
            foreach (var fl in externalFlights)
            {
                internalFlights.Add(fl);
            }
            foreach (var fl in externalFlights)
            {
                internalFlights.Add(fl);
            }
            int first = lstController.Count();
            int second = internalFlights.Count();
            try
            {
                Assert.AreEqual(first, second);
                AssertCheckHelper(lstController, internalFlights);
            }
            catch (AssertFailedException) { }
        }

        private void AssertCheckHelper(IEnumerable<Flight> lstController, List<Flight> internalFlights)
        {
            int first = lstController.Count();

            for (int i = 0; i < first; i++)
            {
                Assert.AreEqual(lstController.ElementAt(i).Latitude,
                    internalFlights.ElementAt(i).Latitude);

                Assert.AreEqual(lstController.ElementAt(i).Longitude,
                    internalFlights.ElementAt(i).Longitude);

                Assert.AreEqual(lstController.ElementAt(i).Passengers,
                    internalFlights.ElementAt(i).Passengers);

                Assert.AreEqual(lstController.ElementAt(i).Company_Name,
                    internalFlights.ElementAt(i).Company_Name);

                Assert.AreEqual(lstController.ElementAt(i).Is_External,
                    internalFlights.ElementAt(i).Is_External);
            }
        }

        private List<Flight> InternalFlights()
        {
            // Fullfilling expected values.  
            List<Flight> fromMock = new List<Flight>();
            Flight f = new Flight
            {
                // As long as not null(id) it's ok. 
                Flight_Id = "stam",
                Longitude = 33,
                Latitude = 33,
                Passengers = 65456,
                Company_Name = "foo_FIRST",
                Date_Time = DateTime.Now,
                Is_External = false
            };

            Flight f1 = new Flight
            {
                // As long as not null(id) it's ok. 
                Flight_Id = "stam",
                Longitude = 33,
                Latitude = 33,
                Passengers = 465,
                Company_Name = "foo_SECOND",
                Date_Time = DateTime.Now,
                Is_External = false
            };
            fromMock.Add(f);
            fromMock.Add(f1);
            return fromMock;
        }

        private List<Flight> GetExternalFromServers()
        {
            // Fullfilling expected external. 
            List<Flight> fromMock = new List<Flight>();
            Flight f = new Flight
            {
     
                Flight_Id = "Foo",
                Longitude = 33,
                Latitude = 33,
                Passengers = 65456,
                Company_Name = "foo_POP",
                Date_Time = DateTime.Now,
                Is_External = true
            };
            fromMock.Add(f);
            Flight f1 = new Flight
            {
   
                Flight_Id = "Foo_1",
                Longitude = 33,
                Latitude = 33,
                Passengers = 65456,
                Company_Name = "foo_OUT",
                Date_Time = DateTime.Now,
                Is_External = true
            };
            fromMock.Add(f1);

            return fromMock;
        }
    }
}