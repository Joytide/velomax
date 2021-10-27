using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    class OrderedBicycle
    {

        #region attributes 
        Purchase purchase;
        Bicycle bicycle;
        int orderedBicycleNumber;
        #endregion

        #region constructors
        public OrderedBicycle(Purchase purchase, Bicycle bicycle, int orderedBicycleNumber)
        {
            this.purchase = purchase;
            this.bicycle = bicycle;
            this.orderedBicycleNumber = orderedBicycleNumber;
        }
        #endregion

        #region properties
        public Purchase Purchase { get { return this.purchase; } set { this.purchase = value; } }
        public Bicycle Bicycle { get { return this.bicycle; } set { this.bicycle = value; } }
        public int OrderedBicycleNumber { get { return this.orderedBicycleNumber; } }

        #endregion

        #region methods
        public override string ToString()
        {
            return "Vélo commandé : \n  : " + purchase + "\n     " + bicycle + "\n    Nombre : " + orderedBicycleNumber ;
        }
        public void InsertIntoDB()
        {
            string request = "INSERT INTO `VeloMax`.`orderedbicycle` (`purchasenum`, `bicycleid`, `orderedbicyclenb`) VALUES (" + this.purchase.PurchaseNum + ", " + this.bicycle.BicycleId + ", " + this.orderedBicycleNumber + ");";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion

    }
}
