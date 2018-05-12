using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingClassLibrary;
using Microsoft.Extensions.Logging;

namespace parkingSimulatorWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/ParkingLot")]
    public class ParkingLotController : Controller
    {
        public ParkingService service { get; set; }
        private readonly ILogger<ParkingLotController> _logger;

        public ParkingLotController(ILogger<ParkingLotController> logger, ParkingService service)
        {
            _logger = logger;
            this.service = service;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page says hello");
            return View();
        }
        
        //public ParkingLotController(ParkingService service)
        //{
           
        //}

        // GET: api/ParkingLot
        [HttpGet]
        public int GetParkingSpaces()
        {
            return service.ShowParkingSpace();

        }
        //// GET: api/ParkingLot/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
        //// POST: api/ParkingLot
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}
        
        //// PUT: api/ParkingLot/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
