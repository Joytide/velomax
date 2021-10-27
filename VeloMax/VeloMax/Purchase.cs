using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    public class Purchase
    {


        #region attributes 
        int purchaseNum;
        DateTime deliveryDate;
        DateTime orderDate;
        Client client;
        Address deliveryAddress;
        int purchaseCost;
        #endregion

        #region constructors
        public Purchase() { }
        public Purchase(int purchaseNum, DateTime deliveryDate, DateTime orderDate,Client client, Address deliveryAddress,int purchaseCost)
        {
            this.purchaseNum = purchaseNum;
            this.deliveryDate = deliveryDate;
            this.orderDate = orderDate;
            this.client = client;
            this.deliveryAddress = deliveryAddress;
            this.purchaseCost = purchaseCost;
        }
        public Purchase(int purchasenum, MySqlConnection co)
        {
            string requete = "Select * from purchase where purchasenum=" + Convert.ToString(purchasenum) + ";";
            MySqlCommand command = co.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.purchaseNum = purchasenum;
            this.deliveryDate = (DateTime)reader["deliverydate"];
            this.orderDate = (DateTime)reader["orderdate"];
            this.purchaseCost = (int)reader["purchasecost"];
            int clientid = (int)reader["clientid"];
            int addressid = (int)reader["addressid"];
            

            reader.Close();
            command.Dispose();

            this.client = new Client(clientid, co);
            this.deliveryAddress = new Address(addressid);
        }
        public Purchase(int purchasenum)
        {
            this.purchaseNum = purchasenum;
        }
        #endregion

        #region properties
        public int PurchaseNum { get { return this.purchaseNum; } set { this.purchaseNum = value; } }
        public DateTime DeliveryDate { get { return this.deliveryDate; } set { this.deliveryDate = value; } }
        public DateTime OrderDate { get { return this.orderDate; } set { this.orderDate = value; } }
        public Client Client { get { return this.client; } set { this.client = value; } }
        public Address DeliveryAddress { get { return this.deliveryAddress; }set { this.deliveryAddress = value; } }
        public int PurchaseCost { get { return this.purchaseCost; } set { this.purchaseCost = value; } }

        #endregion

        #region methods
        public override string ToString()
        {
            return "Commande : " + purchaseNum + "   Date de commande : " + orderDate.ToString() + "   Date de réception : " + deliveryDate.ToString() + "     " + client.ClientID.ToString() + "  Prix de la commande : " + purchaseCost;
        }
        public void InsertIntoDB()
        {
            string request = "INSERT INTO `VeloMax`.`purchase` (`purchasenum`, `deliverydate`, `orderdate`, `clientid`, `addressid`,`purchasecost`) VALUES (" + this.purchaseNum + ", '"+ deliveryDate.Year + "-" + deliveryDate.Month + "-" + deliveryDate.Day + " " + deliveryDate.Hour + ":" + deliveryDate.Minute + ":" + deliveryDate.Second + "', '"+ orderDate.Year + "-" + orderDate.Month + "-" + orderDate.Day + " " + orderDate.Hour + ":" + orderDate.Minute + ":" + orderDate.Second + "', " +this.client.ClientID+", " + this.deliveryAddress.AddressID +", "+this.purchaseCost +");";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion


    }
}
