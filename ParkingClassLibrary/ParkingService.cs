using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("parkingSimulatorWebAPI")]

namespace ParkingClassLibrary
{
    
    public class ParkingService  //клас Парковка (патерн Singleton)
    {
        public List<Car> Cars { get; set; }//список машин
        public List<Transaction> Trans { get; private set; }//список транзакцій
        public int ParkingBalance { get; private set; }//баланс парковки
        public int Fine { get; private set; }//штраф
        public int TimeOut { get; }//таймер
        public Dictionary<Car.CarTypes, int> Tarif { get; }//тарифы
        public int ParkingSpace { get; set; }//количество мест



        /// <summary>
        /// я так понял что, прописав в Startup.cs services.AddSingleton<ParkingService>(), 
        /// мы объявляем Singelton и последуующий код можна закомментить. Надеюсь я не ошибся. Вроде все работает)
        /// </summary>
        //public static readonly Lazy<ParkingService> lazy = new Lazy<ParkingService>(() => new ParkingService());
        //public static ParkingService Instance { get { return lazy.Value; } }

        private static readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
                
        public ParkingService()
        {
            this.Cars = new List<Car>();
            this.Trans = new List<Transaction>();
            this.TimeOut = Settings.Timeout;
            this.Tarif = Settings.Tarif;
            this.ParkingSpace = Settings.ParkingSpace;
            this.Fine = Settings.Fine;

            //запускаем списание средств каждые 3 минуты и запись в лог каждую минуту
            Task.Run(() => WithdrawTransaction(1000 * 60 * TimeOut));
            Task.Run(() => WriteTransactionLog(1000 * 30 * 1));

        }
        //списание средств
        public async void WithdrawTransaction(int delay)

        {

            while (true)
            {
                if (Cars.Count > 0)

            {
                foreach (var car in Cars)
                {
                    var tarif = 0;
                    switch (car.TypeofCar)
                    {
                        case Car.CarTypes.Passanger:
                            tarif = Tarif[Car.CarTypes.Passanger];
                            break;
                        case Car.CarTypes.Bus:
                            tarif = Tarif[Car.CarTypes.Bus];
                            break;
                        case Car.CarTypes.Truck:
                            tarif = Tarif[Car.CarTypes.Truck];
                            break;
                        case Car.CarTypes.Moto:
                            tarif = Tarif[Car.CarTypes.Moto];
                            break;

                        default:
                            break;
                    }

                    if (car.Balance < tarif)
                        tarif = tarif + (tarif * Fine);

                    car.Balance = car.Balance - tarif;
                    ParkingBalance = ParkingBalance + tarif;

                    var transaction = new Transaction(DateTime.Now, car.ID, tarif);
                    Trans.Add(transaction);
                }
            }
            
            await Task.Delay(delay);
            }
            
           
        
        }
        //пишем сумму транзакций в файл
        public async void WriteTransactionLog(int delay)
        {
            while (true)
            {
            var sum = 0;

            foreach (var tr in Trans)
            {
                if (tr.TimeOfTransaction > (DateTime.Now - TimeSpan.FromMinutes(1)))
                {
                    sum += tr.TransactionAmount;
                }

            }
            
                var log = string.Format("{0} Сума транзакцій за останню хвилину = {1} грн", DateTime.Now, sum);
                var path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
                var fileName = Path.Combine(path, "Transaction.log");

                _readWriteLock.EnterWriteLock();
                try
                {
                    using (var file = new System.IO.StreamWriter(fileName)) 
                        {
                            file.WriteLine(log);
                        }
                }
                finally
                {
                    _readWriteLock.ExitWriteLock();
                }
            await Task.Delay(delay);
            }
            
            
        } 
        //транзакции за последнюю минуту
        public IEnumerable<Transaction> TransactionLastMinute()

        {
            var query = Trans
                .Where(r => r.TimeOfTransaction > DateTime.Now - TimeSpan.FromMinutes(1));
            return (IEnumerable<Transaction>)query;

        }
        //читаем лог
        public string[] ReadTransactionLog()

        {
            string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            string fileName = Path.Combine(path, "Transaction.log");
            string[] line = new string[] { };

            var workCompleted = false;

            //если в файл пишет логер, то ждем 5 секунд 
            while (!workCompleted)
            {
                try
                {
                    line = File.ReadAllLines(fileName);
                    workCompleted = true;
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                }
            }

            return line;
        }
        
        //добавить авто на парковку
        public bool AddCar(Car _car)
        {
            if (ParkingSpace >= 1)
            {
                Cars.Add(_car);
                ParkingSpace--;
                return true;
            }
            return false;
        }
        //забрать авто с парковки
        public bool RemoveCar(Guid id)
        { 
            var removedCar = Cars.Find(x => x.ID == id);
            if (removedCar.Balance > 0)
            {
                Cars.Remove(removedCar);
                ParkingSpace++;
                return true;
            }
            else 
                return false;
            
        }
        //пополняем баланс авто
        public Car RefillCarBalance(Guid id, int sum)

        {
          var _car = Cars.Find(x => x.ID == id);
          _car.Balance += sum;
          return _car;
        }   
        //баланс авто
        public int ShowCarBalance(Guid car_ID)

        {
            var car = Cars.Find(x => x.ID == car_ID);
            return car.Balance;
             
        }
      
        //баланс парковки
        public int ShowParkingBalance()
        {
            return  ParkingBalance;
        }
        //места на парковке
        public int ShowParkingSpace()

        {
            return ParkingSpace;
        }

    }
}
