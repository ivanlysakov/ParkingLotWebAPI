using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ParkingClassLibrary
{
    public class Car
    {
        public Guid ID { set; get; } //Свойство идентификатор
        public int Balance { set; get; }//Свойство баланс
        public CarTypes TypeofCar { set; get; } //Свойство тип машины

      
        

        //виды авто
        [JsonConverter(typeof(StringEnumConverter))]
        public enum CarTypes
        {
            Undefined,
            Passanger,
            Truck,
            Bus,
            Moto

        }
        public Car(int balance, CarTypes type)

        {
            this.ID = Guid.NewGuid();
            this.Balance = balance;
            this.TypeofCar = type;
            
        }
    }
}
