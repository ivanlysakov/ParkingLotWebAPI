using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingClassLibrary
{
    public class Settings
    {
        //Свойство Timeout (каждые N-секунд списывает средства за парковочное место) - по умолчанию 3 секунды
        public static int Timeout { get; } = 3;

        //Dictionary - словарь для хранения цен за парковку
        public static Dictionary<Car.CarTypes, int> Tarif { get; } = new Dictionary<Car.CarTypes, int>
            {
               { Car.CarTypes.Truck ,       5 } ,
               { Car.CarTypes.Passanger ,   3 },
               { Car.CarTypes.Bus ,         2 },
               { Car.CarTypes.Moto ,        1 }
            };
        //Свойство ParkingSpace - вместимость парковки(общее кол-во мест)
        public static int ParkingSpace { get; } = 5;

        //Свойство Fine - коэффициент штрафа
        public static int Fine { get; } = 2;


    }
}
