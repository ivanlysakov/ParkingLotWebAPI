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
        public ParkingService ParkingLotService { get; set; } = ParkingService.Instance;
        
        private readonly ILogger<ParkingLotController> _logger;

        public ParkingLotController(ILogger<ParkingLotController> logger, ParkingService service)
        {
            _logger = logger;
            this.ParkingLotService = service;
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
            return Json(new {free_spaces = ParkingLotService.ShowParkingSpace()});

        }

        // GET: api/ParkingLot/ParkingOccupiedSpaces
        [Route("api/[controller]/ParkingOccupiedSpaces")]
        [HttpGet]
        public JsonResult GetParkingOccupiedSpaces()
        {
            return Json(new { occupied_spaces = ParkingLotService.Cars.Count });
        }
               
        // GET: api/ParkingLot/Balance
        [Route("api/[controller]/ParkingBalance")]
        [HttpGet]
        public JsonResult GetParkingBalance()
        {
            return Json(new { time = DateTime.Now.ToString("g"), parking_balance = ParkingLotService.ShowParkingBalance() });
        }
        
        // GET: api/ParkingLot/ShowAllCars
        [Route("api/[controller]/ShowAllCars")]
        [HttpGet]
        public JsonResult GetAllCars()
        {
            return Json(ParkingLotService.Cars);
        }

        // POST: api/CarAdd
        [Route("api/[controller]/AddCar/{type}/{balance}")]
        [HttpPost]
        public JsonResult PostNewCar(string type,  int balance )
        {
            try
            {
               var cartype = (Car.CarTypes)Enum.Parse(typeof(Car.CarTypes), type);
               bool add =  ParkingLotService.AddCar(new Car(balance, cartype));

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
                return Json(new { car_id = id, balance = ParkingLotService.ShowCarBalance(id) });
            }
            catch 
            {

                return Json(new { error = "There is no car with ID "+ id +". Repeat please" });
            }
            
        }

        // DELETE: api/ParkingLot/RemoveCar
        [Route("api/[controller]/DeleteCar/{id}")]
        [HttpDelete("{id}")]
        public JsonResult RemoveCar(int id)
        {
            try
            {
              
                bool remove = ParkingLotService.RemoveCar(id);

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


        // GET: api/ParkingLot/ReadTransactionLog
        [Route("api/[controller]/ReadTransactionLog")]
        [HttpGet]
        public JsonResult ReadTransactionLog()
        {
           return Json(ParkingLotService.ReadTransactionLog());
        }


        // GET: api/ParkingLot/LastMinuteTransactions
        [Route("api/[controller]/LastMinuteTransactions")]
        [HttpGet]
        public JsonResult ShowLastMinuteTransactions()
        {
            return Json(ParkingLotService.TransactionLastMinute());
        }

        // GET api/ParkingLot/CarTransactionLasMinute/{id}"
        [Route("api/[controller]/CarTransactionLastMinute/{id}")]
        [HttpGet("{id}")]
        public JsonResult GetCarTransactionLastMinute(int id)
        {
            var transactions = ParkingLotService.TransactionLastMinute().Where(r => r.CarID == id);

            if (transactions.Any())
                return Json(transactions);
           
            else
                return Json(new { message = "There are no any transactions or car_ID is wrong."});
           
        }



        // PUT api/ParkingLot/RefillCarBalance/{id}/{sum}"
        [Route("api/[controller]/RefillCarBalance/{id}/{sum}")]
        [HttpPut]
        public JsonResult RefillCarBalance(int id, int sum)
        {
            try
            {
                var car = ParkingLotService.RefillCarBalance(id, sum);
                return Json(new { amount = sum, status = "OK", car_id = car.ID, balance = car.Balance });
            }
            catch 
            {
                return Json(new { error = "There is no car with ID "+ id +". Repeat please" });
            }
            
        }
    }
}
