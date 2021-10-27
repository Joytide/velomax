using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace VeloMax
{
    public class Bicycle
    {

        #region attributes 
        int bicycleId;
        string bName;
        string size;
        decimal cost;
        string type;
        DateTime introDate;
        DateTime deprecDate;
        int stockBicycleNumber;
        #endregion

        #region constructors
        public Bicycle() { }
        public Bicycle(int bicycleId,string bName, string size, decimal cost, string type,DateTime introDate,DateTime deprecDate,int stockBicycleNumber)
        {
            this.bicycleId = bicycleId;
            this.bName = bName;
            this.size = size;
            this.cost = cost;
            this.type = type;
            this.introDate = introDate;
            this.deprecDate = deprecDate;
            this.stockBicycleNumber = stockBicycleNumber;
        }
        public Bicycle(int bicycleid)
        {
            string requete = "Select * from bicycle where bicycleid=" + Convert.ToString(bicycleid) + ";";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.bicycleId = bicycleid;
            this.bName = (string)reader["bname"];
            this.size = (string)reader["size"];
            this.cost = (decimal)reader["cost"];
            this.type = (string)reader["type"];
            this.introDate = reader.GetDateTime(5);
            this.deprecDate = reader.GetDateTime(6);
            this.stockBicycleNumber = (int)reader["stockbicyclenumber"];


            reader.Close();
            command.Dispose();
        }
        #endregion

        #region properties
        public int BicycleId { get { return this.bicycleId; } set { this.bicycleId = value; } }
        public string BName { get { return this.bName; } set { this.bName = value; } }
        public string Size { get { return this.size; } set { this.size = value; } }
        public decimal Cost { get { return this.cost; } set { this.cost = value; } }
        public string Type { get { return this.type; } set { this.type = value; } }
        public DateTime IntroDate { get { return this.introDate; } set { this.introDate = value; } }
        public DateTime DeprecDate { get { return this.deprecDate; } set { this.deprecDate = value; } }
        public int StockBiicycleNumber { get { return this.stockBicycleNumber; } set { this.stockBicycleNumber = value; } }

        #endregion

        #region methods
        public string ToStringHeavy()
        {
            return "Vélo : \n  ID : " + bicycleId + "\n    Nom du modèle : " + bName + "\n    Taille : " + size + "\n     Prix : " + cost + "\n    Type : " + type + "\n    Date d'entrée : " + introDate.ToString() + "\n     Date de sortie : " + deprecDate.ToString() + "\n    Stock : " + stockBicycleNumber;
        }
        public override string ToString()
        {
            return "Modèle: " + bName + "  Taille: " + size + "   Prix: " + cost + "€    Type: " + type;
        }

        public void InsertIntoDB()
        {
            string request = "INSERT INTO `VeloMax`.`bicycle` (`bicycleid`, `bname`, `size`, `cost`, `type`, `introddate`, `deprecdate`, `stockbicyclenumber`) VALUES (" + this.bicycleId + ", '" + this.bName + "', '" + this.size + "', " + this.cost + ", '" + this.type + "', '" + introDate.Year + "-" + introDate.Month + "-" + introDate.Day + " " + introDate.Hour + ":" + introDate.Minute + ":" + introDate.Second + "', '" + deprecDate.Year + "-" + deprecDate.Month + "-" + deprecDate.Day + " " + deprecDate.Hour + ":" + deprecDate.Minute + ":" + deprecDate.Second + "', " + this.stockBicycleNumber + ");";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion

    }
}
