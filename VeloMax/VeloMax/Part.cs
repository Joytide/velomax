using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    public class Part
    {
        #region attributes
        string partNum;
        string partDesc;
        int stockPartNumber;
        int price;
        #endregion

        #region constructors
        public Part() { }
        public Part(string partNum, string partDesc, int stockPartNumber, int price)
        {
            this.partNum = partNum;
            this.partDesc = partDesc;
            this.stockPartNumber = stockPartNumber;
            this.price = price;
        }
        public Part(string partnumtoload)
        {
            string requete = "Select * from part where partnum='" + partnumtoload + "';";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.partNum = partnumtoload;
            this.partDesc = (string)reader["partDesc"];
            this.stockPartNumber = (int)reader["stockpartnumber"];
            this.price = (int)reader["price"];
            

            reader.Close();
            command.Dispose();
        }
        #endregion

        #region properties
        public string PartNum { get { return this.partNum; } set { this.partNum = value; } }
        public string PartDesc { get { return this.partDesc; } set { this.partDesc = value; } }
        public int StockPartNumber { get { return this.stockPartNumber; } set { this.stockPartNumber = value; } }
        public int Price { get { return this.price; } set { this.price = value; } }

        #endregion

        #region methods
        public string ToStringHeavy()
        {
            return "Pièce : \n  ID : " + partNum + "\n    Description : " + partDesc + "\n    Prix : " + price + "\n    Stock : " + stockPartNumber ;
        }
        public override string ToString()
        {
            return "ID: " + partNum + " Description: " + partDesc+" Nombre en Stock: "+stockPartNumber+ " Prix : "+price;
        }
        public void InsertIntoDB()
        {
            string request = "INSERT INTO `VeloMax`.`part` (`partnum`, `partdesc`, `stockpartnumber`,`price`) VALUES (" + this.partNum + ", '" + this.partDesc + "', " + this.stockPartNumber +", "+this.price+ ");";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            
            reader.Close();
            command.Dispose();
        }
        #endregion


    }
}
