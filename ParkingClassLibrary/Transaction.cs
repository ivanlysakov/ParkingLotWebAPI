using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ParkingClassLibrary
{

    public class Transaction
    {
        //Свойство Дата/Время Транзакции
        public int TransactionID { get; set; }
        //Свойство Дата/Время Транзакции
        public DateTime TimeOfTransaction { get; set; }
        //Свойство Идентификатор машины
        public int CarID { get; set; }
        //Свойство Списанные средства
        public int TransactionAmount { get; set; }

        public static int globalTransactionID;

        public Transaction(DateTime time, int id, int amount)

        {
            this.TransactionID = Interlocked.Increment(ref globalTransactionID);
            this.TransactionAmount = amount;
            this.CarID = id;
            this.TimeOfTransaction = time;
        }
    }
}
