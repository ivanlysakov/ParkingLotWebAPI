using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
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

        private ParkingService()
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
        public void AddCar(Car.CarTypes cartype)
        {

            if (ParkingSpace >= 1)
            {
                Car car = new Car(15, cartype);
                Cars.Add(car);
                ParkingSpace--;
            }


        }
        //забрать авто с парковки
        public void RemoveCar()

        {
            Console.Clear();
            Console.WriteLine("Авто наявні на парковці");
            Console.WriteLine("=============================");
            Console.WriteLine("id    баланс грн   тип авто");
            foreach (var car in Cars)
            {
                Console.WriteLine("{0}     {1} грн      {2}", car.ID, car.Balance, car.TypeofCar);
            }
            Console.WriteLine("=============================");



            Console.WriteLine("Який номер вашого авто [1,2,3...]?");

            try
            {
                int userChoice = Int32.Parse(Console.ReadLine());
                Car removeCar = Cars.Find(x => x.ID == userChoice);
                if (removeCar.Balance > 0)
                {
                    Cars.Remove(removeCar);
                    ParkingSpace++;
                }
                else
                {
                    Console.WriteLine("Ви не можете забрати Ваше авто через брак коштів на балансі!");
                    Console.WriteLine("Спочатку поповніть баланс Вашого авто і повторіть спробу)");

                }
            }
            catch

            {
                Console.WriteLine("Такого авто немає в списку");
            }


        }
        //списание средств
        public void WithdrawTransaction()

        {
            if (Cars.Count > 0)

            {
                foreach (var car in Cars)
                {
                    int tarif = 0;
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

                    Transaction transaction = new Transaction(DateTime.Now, car.ID, tarif);
                    Trans.Add(transaction);
                }
            }

        }
        //транзакции за последнюю минуту
        public void TransactionLastMinute()

        {
            Console.Clear();

            foreach (var tr in Trans)
            {

                if (tr.TimeOfTransaction > (DateTime.Now - TimeSpan.FromMinutes(1)))
                {
                    Console.WriteLine(" {0} з авто № {1} {2} списано {3} грн", tr.TransactionID, tr.CarID, tr.TimeOfTransaction, tr.TransactionAmount);
                }
                else
                {

                }

            }
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

            string log = string.Format("{0} Сума транзакцій за останню хвилину = {1} грн", DateTime.Now, sum);

            string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            string fileName = Path.Combine(path, "test.txt");

            using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(fileName))
            {
                file.WriteLine(log);
            }


        }
        //читаем лог
        public void ReadTransactionLog()

        {
            string path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
            string fileName = Path.Combine(path, "test.txt");
            string[] lines = new string[] { };

            var workCompleted = false;

            //если в файл пишет логер, то ждем 10 секунд 
            while (!workCompleted)
            {
                try
                {
                    lines = System.IO.File.ReadAllLines(fileName);
                    workCompleted = true;
                }
                catch (Exception)
                {
                    Thread.Sleep(10000);
                }
            }

            Console.Clear();
            Console.WriteLine("Лог транзакцій за останню хвилину");
            foreach (string line in lines)
            {
                Console.WriteLine("\t" + line);
            }

        }
        //баланс авто
        public void ShowCarBalance()

        {

            Console.Clear();
            Console.WriteLine("Який номер вашого авто [1,2,3...]?");

            try
            {
                int userChoice = Int32.Parse(Console.ReadLine());
                Car _Car = Cars.Find(x => x.ID == userChoice);
                Console.Clear();
                Console.WriteLine("Баланс Вашого авто {0} грн. ", _Car.Balance.ToString());

            }
            catch

            {
                Console.WriteLine("На жаль, таке авто у нас не припарковане");
                Console.WriteLine("Натисніть будь-яку клавішу...");
                Console.ReadLine();
            }

        }
        //пополняем баланс авто
        public void RefillCarBalance()

        {

            Console.Clear();
            Console.WriteLine("Який номер вашого авто [1,2,3...]?");

            try
            {
                int userChoice = Int32.Parse(Console.ReadLine());
                Car _Car = Cars.Find(x => x.ID == userChoice);

                Console.WriteLine("Введіть суму поповнення:");
                int refillSum = Int32.Parse(Console.ReadLine());

                _Car.Balance += refillSum;
                Console.Clear();

                Console.WriteLine("Внесено {0} грн. ", refillSum.ToString());
                Console.WriteLine("Баланс Вашого авто {0} грн. ", _Car.Balance.ToString());

            }
            catch

            {
                Console.WriteLine("На жаль, таке авто у нас не припарковане");
                Console.WriteLine("Натисніть будь-яку клавішу...");
                Console.ReadLine();
            }

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
