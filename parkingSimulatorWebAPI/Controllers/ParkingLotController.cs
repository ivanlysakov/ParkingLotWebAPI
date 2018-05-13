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
   
    public class ParkingLotController : Controller
    {
        public ParkingService service { get; set; } = ParkingService.Instance;
        
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
                            

        [Route("api/[controller]/ParkingFreeSpaces")]
        [HttpGet]
        public JsonResult GetParkingFreeSpaces()
        {
            return Json(new {free_spaces = service.ShowParkingSpace()});

        }

        [Route("api/[controller]/ParkingOccupiedSpaces")]
        [HttpGet]
        public JsonResult GetParkingOccupiedSpaces()
        {
            return Json(new { occupied_spaces = service.Cars.Count });
        }
               

        // GET: api/ParkingLot/Balance
        [Route("api/[controller]/ParkingBalance")]
        [HttpGet]
        public JsonResult GetParkingBalance()
        {
            return Json(new { time = DateTime.Now.ToString("g"), parking_balance = service.ShowParkingBalance() });
        }

        // POST: api/CarAdd
        [Route("api/[controller]/AddCar")]
        [HttpPost]
        public JsonResult PostNewCar(string type, int balance )
        {
            try
            {
                Car.CarTypes _type = (Car.CarTypes)Enum.Parse(typeof(Car.CarTypes), type);
                Car car = new Car(balance, _type);
                service.AddCar(car);
                return Json(Ok());
            }
            catch 
            {

                return Json(BadRequest());
            }
            
            
        }
        
        // GET api/values/5
        [Route("api/[controller]/ShowCarBalance/{id}")]
        [HttpGet("{id}")]
        public JsonResult GetCarBalance(int id)
        {
            return Json(new { car_id = id, balance = service.ShowCarBalance(id) });
        }

                
    }
}
