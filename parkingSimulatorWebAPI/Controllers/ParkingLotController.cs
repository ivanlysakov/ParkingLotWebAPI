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

        // GET: api/ParkingLot/ParkingFreeSpaces
        [Route("api/[controller]/ParkingFreeSpaces")]
        [HttpGet]
        public JsonResult GetParkingFreeSpaces()
        {
            return Json(new {free_spaces = service.ShowParkingSpace()});

        }

        // GET: api/ParkingLot/ParkingOccupiedSpaces
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



        // GET: api/ParkingLot/ShowAllCars
        [Route("api/[controller]/ShowAllCars")]
        [HttpGet]
        public JsonResult GetAllCars()
        {
            return Json(service.Cars);
        }

        // POST: api/CarAdd
        [Route("api/[controller]/AddCar/{type}/{balance}")]
        [HttpPost]
        public JsonResult PostNewCar(string type,  int balance )
        {
            try
            {
               Car.CarTypes _type = (Car.CarTypes)Enum.Parse(typeof(Car.CarTypes), type);
               bool add =  service.AddCar(new Car(balance, _type));

                if (add)
                    return Json(new { ok = "Your car  was parked!" });
                else
                    return Json(new { error = "There are no places in our parkinglot. Sorry" });
            }
            catch 
            {

                return Json(new { error = "There is no such type of car. Repeat please" });
            }
            
            
        }

        // GET api/ParkingLot/ShowCarBalance/{id}"
        [Route("api/[controller]/ShowCarBalance/{id}")]
        [HttpGet("{id}")]
        public JsonResult GetCarBalance(int id)
        {
            try
            {
                return Json(new { car_id = id, balance = service.ShowCarBalance(id) });
            }
            catch 
            {

                return Json(new { error = "There is no car with ID {id}. Repeat please" });
            }
            
        }

        // DELETE: api/ParkingLot/RemoveCar
        [Route("api/[controller]/DeleteCar/{id}")]
        [HttpDelete("{id}")]
        public JsonResult RemoveCar(int id)
        {
            try
            {
              
                bool remove = service.RemoveCar(id);

                if (remove)
                    return Json(new { car_id = id, IsRemoved = "OK" });
                else
                    return Json(new { error = "You need to pay first. Sorry" });
            }
            catch
            {

                return Json(new { error = "There is no car with ID {id}. Repeat please" });
            }


        }



    }
}
