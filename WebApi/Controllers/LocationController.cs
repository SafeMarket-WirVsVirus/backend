using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnect.Data;
using MySqlConnect.Entities;
using ReservationSystem.Contracts;
using WebApi.Services;

namespace ReservationSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        PlacesTextsearchService _placesSearch;
        PlacesDetailService _placesDetail;
        public LocationController(PlacesTextsearchService placesSearch,
                                  PlacesDetailService placesDetail)
        {
            _placesSearch = placesSearch;
            _placesDetail = placesDetail;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationModel model)
        {
            using (var context = new ReservationContext())
            {
                var user = await context.User.Where(user => user.Id == model.UserId).FirstOrDefaultAsync();
                if (user == null)
                {
                    return NotFound(nameof(User));
                }

                var location = new Location()
                {
                    Name = model.Name,
                    FillStatus = model.FillStatus,
                    Capacity = model.Capacity,
                    SlotSize = model.SlotSize,
                    SlotDuration = model.SlotDuration,
                    ShopType = model.ShopType,
                    User = user,
                    PlacesId = model.PlacesId
                };

                foreach (LocationOpeningModel locModel in model.LocationOpening) {
                    location.LocationOpenings.Add(new LocationOpening() {
                        OpeningDays = locModel.DayOfWeek,
                        OpeningTime = locModel.OpeningHours,
                        ClosingTime = locModel.ClosingHours
                    });
                }

                await context.Location.AddAsync(location);
                await context.SaveChangesAsync();

                return Ok(new CreateLocationResult()
                {
                    Id = location.Id
                });
            }
        }

        [HttpGet("{locationId}")]
        public async Task<LocationResult> GetLocation(int locationId)
        {
            using (var context = new ReservationContext())
            {
                Location location = await context.Location.FirstOrDefaultAsync(x => x.Id == locationId);

                if (location == null)
                {
                    NotFound();
                    return null;
                }

                var googleLocation = await _placesDetail.GetFor(location.PlacesId);

                Ok();
                return new LocationResult()
                {
                    Id = location.Id,
                    Name = location.Name,
                    FillStatus = location.FillStatus,
                    Capacity = location.Capacity,
                    SlotDuration = location.SlotDuration,
                    SlotSize = location.SlotSize,
                    Openings = location.LocationOpenings.Select(x => new OpeningResult(x)).ToList(),
                    Latitude = googleLocation.Latitude,
                    Longitude = googleLocation.Longitude,
                    Address = googleLocation.Address
                };
            }
        }

        [HttpGet("GetLocationByUserId/{userId}")]
        public async Task<List<LocationResult>> GetLocationByUserId(int userId)
        {
            using (var context = new ReservationContext())
            {
                if (await context.User.AnyAsync(x => x.Id == userId) == false)
                {
                    NotFound();
                    return null;
                }

                Ok();
                return await context.Location.Where(x => x.UserId == userId)
                                    .Select(x => new LocationResult(x))
                                    .ToListAsync();
            }
        }

        [HttpGet("Search")]
        public async Task<IEnumerable<PlacesTextsearchResponse>> SearchLocations(
            [FromQuery] string type,
            [FromQuery] double longitude,
            [FromQuery] double latitude,
            [FromQuery] int radiusInMeters)
        {
            var locations = await _placesSearch.GetFor(type, longitude, latitude, radiusInMeters);
            return locations;
        }

        [HttpGet("SearchRegistered")]
        public async Task<IEnumerable<LocationResult>> SearchRegisteredLocations(
            [FromQuery] string type,
            [FromQuery] double longitude,
            [FromQuery] double latitude,
            [FromQuery] int radiusInMeters)
        {
            var googleLocations = await _placesSearch.GetFor(type, longitude, latitude, radiusInMeters);
            var placesIds = googleLocations.Select(l => l.PlaceId);
            using (var context = new ReservationContext())
            {
                var registeredLocations = await context.Location
                    .Where(x => placesIds.Contains(x.PlacesId))
                    .Select(x => new LocationResult(x))
                    .ToListAsync();


                var response = new List<LocationResult>();

                foreach (var registeredLocation in registeredLocations)
                {
                    //Find corresponding places location
                    //PlacesId gets mapped to LocationResult, only to pass it for this mapping part.
                    //Find a better solution, when not tired anymore
                    //Also should never throw or be empty here, as we used the otherway round to find the locations
                    var placesLocation = googleLocations.First(x => x.PlaceId == registeredLocation.PlacesId);
                    registeredLocation.Latitude = placesLocation.Latitude;
                    registeredLocation.Longitude = placesLocation.Longitude;
                    registeredLocation.Address = placesLocation.Address;

                    response.Add(registeredLocation);
                }

                return response;
            }
        }


        [HttpDelete("{locationId}")]
        public async Task<IActionResult> DeleteLocation(int locationId)
        {
            using (var context = new ReservationContext())
            {
                Location location = await context.Location.FirstOrDefaultAsync(x => x.Id == locationId);
                if (location == null)
                {
                    return NotFound();
                }

                context.Location.Remove(location);

                await context.SaveChangesAsync();

                return Ok();
            }
        }

        [HttpPut("{locationId}")]
        public async Task<LocationModel> Update(int locationId, [FromBody] LocationModel model)
        {
            using (var context = new ReservationContext())
            {
                Location location = await context.Location.FirstOrDefaultAsync(x => x.Id == locationId);
                if (location == null)
                {
                    NotFound();
                    return null;
                }

                location.Name = model.Name;
                location.FillStatus = model.FillStatus;
                location.Capacity = model.Capacity;
                location.SlotSize = model.SlotSize;
                location.SlotDuration = model.SlotDuration;
                location.ShopType = model.ShopType;
                location.PlacesId = model.PlacesId;
                // todo openings and owner

                await context.SaveChangesAsync();

                Ok();
                return model;
            }
        }

    }

}
