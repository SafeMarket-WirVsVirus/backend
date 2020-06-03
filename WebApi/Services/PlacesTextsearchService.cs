using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace WebApi.Services
{
    public class PlacesTextsearchResponse
    {
        public string PlaceId { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
    }

    public class PlacesTextsearchService
    {
        //TODO: Änder mich halt später mal :)
        const string API_KEY = "YOUR_GOOGLE_WEB_API_KEY";
        public async Task<IEnumerable<PlacesTextsearchResponse>> GetFor(string type, double longitude, double latitude, int radiusInMeters)
        {
            string url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?type={type}&radius={radiusInMeters}&location={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}&key={API_KEY}";
            var client = new RestClient(url);
            client.AddDefaultHeader("Accept-Encoding", "gzip, deflate");
            var request = new RestRequest(Method.GET);

            var result = await client.ExecuteAsync(request);

            var data = result.Content != null ? JsonConvert.DeserializeObject<PlacesTextsearch>(result.Content) : null;

            if (data.Status == "ZERO_RESULTS")
                return Enumerable.Empty<PlacesTextsearchResponse>();

            return data.Results.Select(r => new PlacesTextsearchResponse
            {
                PlaceId = r.Id,
                Name = r.Name,
                WebsiteUrl = r.Website,
                Latitude = r.Geometry.Location.Latitude,
                Longitude = r.Geometry.Location.Longitude,
                Address = r.FormattedAddress,
                Type = r.Types.FirstOrDefault()
        });
        }

        public class PlacesTextsearch
        {
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("results")]
            public IEnumerable<Result> Results { get; set; }


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
            public IEnumerable<string> Types{ get; set; }
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