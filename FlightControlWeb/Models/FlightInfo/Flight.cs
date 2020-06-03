using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FlightControlWeb.Models.FlightInfo
{
    public class Flight
    {

        [JsonProperty("flight_Id")]
        [JsonPropertyName("flight_Id")]
        public string Flight_Id { get; set; }

        [JsonProperty("latitude")]
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("passengers")]
        [JsonPropertyName("passengers")]
        public int Passengers { get; set; }

        [JsonProperty("company_Name")]
        [JsonPropertyName("company_Name")]
        public string Company_Name { get; set; }

        [JsonProperty("date_Time")]
        [JsonPropertyName("date_Time")]
        public DateTime Date_Time { get; set; }

        [JsonProperty("is_External")]
        [JsonPropertyName("is_External")]
        public bool Is_External { get; set; }
    }
}
