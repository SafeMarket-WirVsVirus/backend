using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySqlConnect.Data;
using MySqlConnect.Entities;
using WebApi.Services;

namespace ReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly PlacesDetailService _placesDetailService;
        private readonly PlacesTextsearchService _placesTextsearchService;

        public ValuesController(
            PlacesDetailService placesDetailService,
            PlacesTextsearchService placesTextsearchService)
        {
            _placesDetailService = placesDetailService;
            _placesTextsearchService = placesTextsearchService;
        }

        [HttpGet("test/{name}")]
        public async Task<string> AddData(string name)
        {
            using (var context = new ReservationContext())
            {
                var x = context.Location.FirstOrDefault();
            }
            using (var context = new ReservationContext())
            {
                context.Location.Add(new Location()
                {
                    FillStatus = FillStatus.Green,
                    Name = name
                });
                await context.SaveChangesAsync();
            }
            return "hallo welt";
        }


        [HttpGet("places")]
        public async Task<PlacesDetailResponse> TestPlaces()
        {
            var response = await _placesDetailService.GetFor("ChIJeZ0RHcNRqEcRZfunLIjlWuY");
            return response;
        }


        [HttpGet("textsearch")]
        public async Task<IEnumerable<PlacesTextsearchResponse>> TestTextsearch()
        {
            var response = await _placesTextsearchService.GetFor("supermarket", 52.52008, 13.404954, 10000);
            return response;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}