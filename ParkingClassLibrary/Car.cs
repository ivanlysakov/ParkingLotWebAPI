using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ParkingClassLibrary
{
    public class Car
    {
        public int ID { set; get; } //Свойство идентификатор
        public int Balance { set; get; }//Свойство баланс
        public CarTypes TypeofCar { set; get; } //Свойство тип машины

        public static int globalCarID;

        //виды авто
        public enum CarTypes { Undefined, Passanger, Truck, Bus, Moto }
        public Car(int balance, CarTypes type)

        {
            this.ID = Interlocked.Increment(ref globalCarID);
            this.Balance = balance;
            this.TypeofCar = type;


        }
    }
}
