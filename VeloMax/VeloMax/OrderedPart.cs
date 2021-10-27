using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    class OrderedPart
    {
        #region attributes 
        Purchase purchase;
        Part part;
        int orderedPartNumber;
        #endregion

        #region constructors
        public OrderedPart(Purchase purchase, Part part, int orderedPartNumber)
        {
            this.purchase = purchase;
            this.part = part;
            this.orderedPartNumber = orderedPartNumber;
        }
        #endregion

        #region properties
        public Purchase Purchase { get { return this.purchase; } set { this.purchase = value; } }
        public Part Part { get { return this.part; } set { this.part = value; } }
        public int OrderedPartNumber { get { return this.orderedPartNumber; } }

        #endregion

        #region methods
        public override string ToString()
        {
            return "Pièce commandée :\n    " + purchase + "\n     " + part + "\n    Nombre : " + orderedPartNumber ;
        }
        public void InsertIntoDB()
        {
            string request = "INSERT INTO `VeloMax`.`orderedpart` (`purchasenum`, `partnum`, `orderedpartnb`) VALUES (" + this.purchase.PurchaseNum + ", '" + this.part.PartNum + "', " + this.orderedPartNumber + ");";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion


    }
}
