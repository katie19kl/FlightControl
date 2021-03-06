﻿using FlightControlWeb.Models.FlightInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class EndDataOfFLightPlan
    {
        public DateTime LandTime { get; set; }

        public DateTime TakeOffTime { get; set; }

        public Double EndLongitude { get; set; }

        public Double EndLatitude { get; set; }

        public Double StartLongitude { get; set; }

        public Double StartLatitude { get; set; }

        public String CompanyName { get; set; }

        public Double NumOfPassengers { get; set; }

        public List<Segment> SegmentsPath { get; set; }
    }  
};
