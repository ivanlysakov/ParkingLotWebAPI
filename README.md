# ParkingLotWebAPI

Academy'18 • 2nd stage • 3. .NET Core
На основі домашньої роботи №2 необхідно реалізувати ASP.NET Core Web API додаток (не потрібно розробляти графічний інтерфейс).

Для тестування роботи додатка можна скористатися наступними тулзами: Postman (https://www.getpostman.com/) або Fiddler (https://www.telerik.com/fiddler)


__REST API:__


__Parking__
* Кількість вільних місць (GET) - api/ParkingLot/ParkingFreeSpaces
* Кількість зайнятих місць (GET) - api/ParkingLot/ParkingOccupiedSpaces
* Загальний дохід (GET) - api/ParkingLot/ParkingBalance

__Cars__
* Список всіх машин (GET) - api/ParkingLot/ShowAllCars
* Деталі по одній машині (GET) - api/ParkingLot/ShowCarDetails/{id}
* Видалити машину (DELETE) - api/ParkingLot/RemoveCar/{id}
* Додати машину (POST) api/AddCar/{type}/{id}

 _type (тип string)_ Passanger, Truck, Bus or Moto
* Поповнити баланс машини (PUT) - api/ParkingLot/RefillCarBalance/{id}/{sum}

__Transactions__
* Вивести Transactions.log (GET) - api/ParkingLot/ReadTransactionLog
* Вивести транзакції за останню хвилину (GET) - api/ParkingLot/LastMinuteTransactions
* Вивести транзакції за останню хвилину по одній конкретній машині (GET) - api/ParkingLot/CarTransactionLastMinute/{id}
