# ParkingLotWebAPI

Academy'18 • 2nd stage • 3. .NET Core
На основі домашньої роботи №2 реалізовано ASP.NET Core Web API додаток.

__Parking__


| URL | Description |
| --- | --- |
| http://localhost:59687/api/ParkingLot/ParkingFreeSpaces | Кількість вільних місць (GET) |
| http://localhost:59687/api/ParkingLot/ParkingOccupiedSpaces | Кількість зайнятих місць (GET) |
| http://localhost:59687/api/ParkingLot/ParkingBalance | Загальний дохід (GET) |



__Cars__

| URL | Description |
| --- | --- |
| http://localhost:59687/api/ParkingLot/ShowAllCars | Список всіх машин (GET) |
| http://localhost:59687/api/ParkingLot/ShowCarDetails/{id} | Деталі по одній машині (GET) |
| http://localhost:59687/api/ParkingLot/RemoveCar/{id} | Видалити машину (DELETE) |
| http://localhost:59687/api/ParkingLot/AddCar/{type}/{sum} | Додати машину (POST) |
| http://localhost:59687/api/ParkingLot/RefillCarBalance/{id}/{sum} | Поповнити баланс машини (PUT) |



__Transactions__


| URL | Description |
| --- | --- |
| http://localhost:59687/api/ParkingLot/ReadTransactionLog| Вивести Transactions.log (GET) |
| http://localhost:59687/api/ParkingLot/LastMinuteTransactions | Транзакції за останню хвилину (GET) |
| http://localhost:59687/api/ParkingLot/CarTransactionLastMinute/{id} | Транзакції за останню хвилину по одній конкретній машині (GET) |


