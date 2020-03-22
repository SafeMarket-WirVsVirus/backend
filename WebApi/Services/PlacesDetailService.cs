using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace WebApi.Services
{
    public class PlacesDetailResponse
    {
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
    }


    public class PlacesDetailService
    {
        const string API_KEY = "GOOGLE_WEB_API_KEY";
        public async Task<PlacesDetailResponse> GetFor(string placesId)
        {
            string referenceIdEncoded = HttpUtility.UrlEncode(placesId, Encoding.UTF8);
            string url = $"https://maps.googleapis.com/maps/api/place/details/json?reference={referenceIdEncoded}&language=de&sensor=false&key={API_KEY}";
            var client = new RestClient(url);
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            var request = new RestRequest(Method.GET);

            var result = await client.ExecuteAsync(request);

            var data = result.Content != null ? JsonConvert.DeserializeObject<PlacesDetail>(result.Content) : null;

            return new PlacesDetailResponse
            {
                PlaceId = data.Result.Id,
                Name = data.Result.Name,
                WebsiteUrl = data.Result.Website,
                Latitude = data.Result.Geometry.Location.Latitude,
                Longitude = data.Result.Geometry.Location.Longitude,
                Address = data.Result.FormattedAddress,
                Type = data.Result.Types.FirstOrDefault()
            };
        }



        public class PlacesDetail
        {
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("result")]
            public Result Result { get; set; }


        }
        public class Result
        {
            [JsonProperty("geometry")]
            public Geometry Geometry { get; set; }
            [JsonProperty("place_id")]
            public string Id { get; set; }
            [JsonProperty("website")]
            public string Website { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("formatted_address")]
            public string FormattedAddress { get; set; }
            [JsonProperty("types")]
            public IEnumerable<string> Types { get; set; }
        }

        public class Geometry
        {
            [JsonProperty("location")]
            public Location Location { get; set; }
        }

        public class Location
        {
            [JsonProperty("lat")]
            public double Latitude { get; set; }
            [JsonProperty("lng")]
            public double Longitude { get; set; }
        }
    }
}