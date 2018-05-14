using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;


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

        public static readonly Lazy<ParkingService> lazy = new Lazy<ParkingService>(() => new ParkingService());

        public static ParkingService Instance { get { return lazy.Value; } }

        private static readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        
        public ParkingService()
        {
            this.Cars = new List<Car>();
            this.Trans = new List<Transaction>();
            this.TimeOut = Settings.Timeout;
            this.Tarif = Settings.Tarif;
            this.ParkingSpace = Settings.ParkingSpace;
            this.Fine = Settings.Fine;

            //запускаем списание средств каждые 3 минуты

            Thread transactionTimer = new Thread(new ThreadStart(InvokeMethod1));
            transactionTimer.Start();

            void InvokeMethod1()
            {
                while (true)
                {
                    WithdrawTransaction();
                    Thread.Sleep(1000 * 60 * TimeOut);
                }
            }

            //запускаем логирование транзакций каждую минуту

            Thread logTimer = new Thread(new ThreadStart(InvokeMethod2));
            logTimer.Start();

            void InvokeMethod2()
            {
                while (true)
                {
                    WriteTransactionLog();
                    Thread.Sleep(1000 * 60 * 1);
                }
            }

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
        public bool RemoveCar(int id)
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
        //списание средств
        public void WithdrawTransaction()

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

        }
        //транзакции за последнюю минуту
        public IEnumerable<Transaction> TransactionLastMinute()

        {
            var query = Trans
                .Where(r => r.TimeOfTransaction > DateTime.Now - TimeSpan.FromMinutes(1));
            return (IEnumerable<Transaction>)query;

        }
        //пишем сумму транзакций в файл
        public void WriteTransactionLog()
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
        //баланс авто
        public int ShowCarBalance(int car_ID)

        {
            var car = Cars.Find(x => x.ID == car_ID);
            return car.Balance;
             
        }
        //пополняем баланс авто
        public Car RefillCarBalance(int id, int sum)

        {
          var _car = Cars.Find(x => x.ID == id);
          _car.Balance += sum;
          return _car;
        }
        //баланс парковки
        public int ShowParkingBalance()
        {
            return  ParkingBalance;
        }
        //мета на парковке
        public int ShowParkingSpace()

        {
            return ParkingSpace;
        }

    }
}
