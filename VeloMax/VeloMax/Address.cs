using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;


namespace VeloMax
{
    public class Address
    {

        #region attributes 
        int addressID;
        string street;
        string region;
        string postalCode;
        string city;

        #endregion

        #region constructors
        public Address() { }
        public Address(int addressID, string street, string region, string postalCode, string city)
        {
            this.addressID = addressID;
            this.street = street;
            this.region = region;
            this.postalCode = postalCode;
            this.city = city;
        }
        public Address(int addressIDtoLoad)
        {
            string requete = "Select * from address where addressid="+Convert.ToString(addressIDtoLoad)+";";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.addressID = addressIDtoLoad;
            this.street = (string)reader["street"];
            this.region = (string)reader["region"];
            this.postalCode = (string)reader["postalcode"];
            this.city = (string)reader["city"];

            reader.Close();
            command.Dispose();
        }


        #endregion

        #region properties
        public int AddressID { get { return this.addressID; } set { this.addressID = value; } }
        public string Street { get { return this.street; } set { this.street = value; } }
        public string Region { get { return this.region; } set { this.region = value; } }
        public string PostalCode { get { return this.postalCode; } set { this.postalCode = value; } }
        public string City { get { return this.city; } set { this.city = value; } }


        #endregion

        #region methods
        public override string  ToString()
        {
            return "Adresse:\n  ID : " + addressID + "\n    Voie : " + street + "\n    Code Postale : " + postalCode + "\n     Ville : " + postalCode + "\n    Région : " + region;      
        }
        public void InsertIntoDB()
        {
            string request = "INSERT INTO `VeloMax`.`address` (`addressid`, `street`, `region`, `postalcode`, `city`) VALUES ("+this.addressID+", '"+this.street+"', '"+this.region+"', '"+this.postalCode+"', '"+this.city+"');";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }

        #endregion

    }
}
