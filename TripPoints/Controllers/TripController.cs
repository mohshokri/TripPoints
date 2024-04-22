using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripPoints.Services;

namespace TripPoints.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly TripService _tripService;
        public TripController(TripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet("{carTripId}")]
        public IActionResult GetPolygonsContainingCarTrip(int carTripId)
        {
            var polies = _tripService.GetPolygonsContainingCarTrip(carTripId);
            return Ok(polies);
        }
    }
}
