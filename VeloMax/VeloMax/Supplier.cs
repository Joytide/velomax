using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    public class Supplier
    {
        //to do list supplier:
        //definition et evolution du label

        #region attributes 

        private string siret;
        private string supplierName;
        private Address supplierAddress;
        private string contactName;
        private string label; //fonction pour ca ?

        #endregion

        #region constructors
        public Supplier() { }
        public Supplier(string siret, string supplierName,Address supplierAddress, string contactName)
        {
            this.siret = siret;
            this.supplierName = supplierName;
            this.supplierAddress = supplierAddress;
            this.contactName = contactName;
            this.label = "undefined label";
        }
        public Supplier(string siret)
        {
            string requete = "Select * from supplier where siret=" + Convert.ToString(siret) + ";";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.siret = siret;
            this.supplierName = (string)reader["suppliername"];
            this.contactName = (string)reader["contactname"];
            int addressid = (int)reader["addressid"];
            this.label = "undefined label";

            reader.Close();
            command.Dispose();

            this.supplierAddress = new Address(addressid);

        }
        public Supplier(string siret,string bullshit)
        {
            this.siret = siret;
        }

        #endregion

        #region properties
        public string Siret { get { return this.siret; } set { this.siret = value; } }
        public string SupplierName { get { return this.supplierName; } set { this.supplierName = value; } }
        public Address SupplierAddress { get { return this.supplierAddress; }set { this.supplierAddress = value; } }
        public string ContactName { get { return this.contactName; } set { this.contactName = value; } }
        public string Label { get { return this.label; } set { this.label = value; } }
        #endregion

        #region methods
        public override string ToString()
        {
            return "Siret : " + siret + "   Nom : " + supplierName + "   Nom du contact : " + contactName + "   Label : " + label;
        }
        public void InsertIntoDB()
        {
            string request = "INSERT INTO `VeloMax`.`supplier` (`siret`, `suppliername`, `contactname`, `label`, `addressid`) VALUES (" + this.siret + ", '" + this.supplierName + "', '" + this.contactName + "', '" + this.label + "', " + this.supplierAddress.AddressID + ");";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion


    }
}
