using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace VeloMax
{
    public class Client
    {

        #region attributes 
        int clientId;
        string clientName;
        string clientSurname;
        string companyName;
        string mail;
        string phone;
        Address clientAddress;
        string contact;
        #endregion

        #region constructors
        public Client() { }
        //cas individu
        public Client(int clientID, string clientName,string clientSurname,Address clientAddress,string mail,string phone )
        {
            this.clientId = clientID;
            this.clientName = clientName;
            this.clientSurname = clientSurname;
            this.clientAddress = clientAddress;
            this.mail = mail;
            this.phone = phone;

            //autres

            this.companyName = "";
            this.contact = "";
        }

        //cas entreprise
        public Client(int clientID, string companyName, Address clientAddress, string mail, string phone,string contact)
        {
            this.clientId = clientID;
            this.companyName = companyName;
            this.clientAddress = clientAddress;
            this.mail = mail;
            this.phone = phone;
            this.contact = contact;

            //autres

            this.clientName = "";
            this.clientSurname = "";
        }
        public Client(int clientid, MySqlConnection co)
        {
            string requete = "Select * from client where clientid=" + Convert.ToString(clientid) + ";";
            MySqlCommand command = co.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.clientId = clientid;
            if ( typeof(string)!= reader["companyname"].GetType())
            {
                this.clientName = (string)reader["clientname"];
                this.clientSurname = (string)reader["surname"];


                //autres

                this.companyName = "";
                this.contact = "";
            }
            else
            {

                this.contact = (string)reader["contact"];
                this.companyName = (string)reader["companyname"];

                //autres

                this.clientName = "";
                this.clientSurname = "";
            }
            this.mail = (string)reader["mail"];
            this.phone = (string)reader["phone"];
            int addressid = (int)reader["addressid"];
            reader.Close();
            command.Dispose();

            this.clientAddress = new Address(addressid);

        }
        public Client(int clientid)
        {
            this.clientId = clientid;
        }
        #endregion

        #region properties
        public int ClientID { get { return this.clientId; } set { this.clientId = value; } }
        public string ClientName { get { return this.clientName; } set { this.clientName = value; } }
        public string ClientSurname { get { return this.clientSurname; } set { this.clientSurname = value; } }
        public string CompanyName { get { return this.companyName; } set { this.companyName = value; } }
        public string Mail { get { return this.mail; } set { this.mail = value; } }
        public string Phone { get { return this.phone; } set { this.phone = value; } }
        public Address ClientAddress { get { return this.clientAddress; } set { this.clientAddress = value; } }
        public string Contact { get { return this.contact; } set { this.contact = value; } }

        #endregion

        #region methods
        public override string ToString()
        {
            if (companyName == "")
            {
                return "(Individu) Nom : " + clientSurname + " Prénom : " + clientName + "   Mail : " + mail + "  Numéro : " + phone;
            }
            else //cas entreprise
            {
                return "(Entreprise) Nom : " + companyName + " Contact : " + contact + "   Mail : " + mail + "  Numéro : " + phone;
            }
        }
        public void InsertIntoDB()
        {

            //rajouter la création de l adresse associée?

            string request = "";
            if (this.CompanyName == "")
            {
                request = "INSERT INTO `VeloMax`.`client` (`clientid`, `clientname`, `surname`, `companyname`, `mail`, `phone`, `contact`,`addressid`) VALUES (" + this.clientId + ", '" + this.clientName + "', '" + this.clientSurname + "', NULL, '" + this.mail + "', '" + this.phone + "', NULL, " + this.ClientAddress.AddressID + ");";
            }
            else
            {
                request = "INSERT INTO `VeloMax`.`client` (`clientid`, `clientname`, `surname`, `companyname`, `mail`, `phone`, `contact`,`addressid`) VALUES (" + this.clientId + ", NULL,NULL, '" + this.companyName + "', '" + this.mail + "', '" + this.phone + "', '" + this.contact + "', " + this.ClientAddress.AddressID + ");";
            }
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion





    }
}
