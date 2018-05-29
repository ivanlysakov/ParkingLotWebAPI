# ParkingLotWebAPI

Academy'18 • 2nd stage • 3. .NET Core
На основі домашньої роботи №2 реалізовано ASP.NET Core Web API додаток.

## Parking
| URL | Description |
| --- | --- |
| api/ParkingLot/ParkingFreeSpaces | Кількість вільних місць (GET) |
| api/ParkingLot/ParkingOccupiedSpaces | Кількість зайнятих місць (GET) |
| api/ParkingLot/ParkingBalance | Загальний дохід (GET) |

## Cars
| URL | Description |
| --- | --- |
| api/ParkingLot/ShowAllCars | Список всіх машин (GET) |
| api/ParkingLot/ShowCarDetails/{id} | Деталі по одній машині (GET) |
| api/ParkingLot/RemoveCar/{id} | Видалити машину (DELETE) |
| api/ParkingLot/AddCar/{type}/{balance} | Додати машину (POST) |
| api/ParkingLot/RefillCarBalance/{id}/{sum} | Поповнити баланс машини (PUT) |

## Transactions
| URL | Description |
| --- | --- |
| api/ParkingLot/ReadTransactionLog| Вивести Transactions.log (GET) |
| api/ParkingLot/LastMinuteTransactions | Транзакції за останню хвилину (GET) |
| api/ParkingLot/CarTransactionLastMinute/{id} | Транзакції за останню хвилину по одній конкретній машині (GET) |


