using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace VeloMax
{
    class VeloMax
    {
        #region attributes
        List<OrderedBicycle> currentPurchaseBicycles;
        List<OrderedPart> currentPurchaseParts;
        decimal currentbikePrice;
        decimal currentpartprice;
        static MySqlConnection connection;
        #endregion

        #region constructeurs
        public VeloMax()
        {
            currentPurchaseBicycles = new List<OrderedBicycle>();
            currentPurchaseParts = new List<OrderedPart>();
            currentbikePrice = 0;
            currentpartprice = 0;
            connection = null;
        }
        #endregion

        #region properties
        public List<OrderedPart> CurrentPurchaseParts
        {
            get { return this.currentPurchaseParts; }
            set { this.currentPurchaseParts = value; }
        }
        public List<OrderedBicycle> CurrentPurchaseBicycles
        {
            get { return this.currentPurchaseBicycles; }
            set { this.currentPurchaseBicycles = value; }
        }
        public decimal CurrentBikePrice
        {
            get { return this.currentbikePrice; }
            set { this.currentbikePrice = value; }
        }
        public decimal CurrentPartPrice
        {
            get { return this.currentpartprice; }
            set { this.currentpartprice = value; }
        }
        static public MySqlConnection Connection
        {
            get { return connection; }
            set{ connection = value; }
        }
        #endregion


        /// <summary>
        /// Méthode qui demande à l'utilisateur console son id et mdp afin d'ouvrir une connection vers la base de données.
        /// On stocke la connection dans l'attribut statique de VeloMax afin que la connection soit accessible de partout.
        /// </summary>
        /// <returns></returns>
        static public bool DBConnect()
        {
            if (VeloMax.connection != null)
                VeloMax.connection.Close();
            Console.Write("Merci de vous connecter à la base de données:\nUID: ");
            string uid = Console.ReadLine();
            if (uid == null)
            {
                Console.WriteLine("Pas d'UID renseigné.\n");
                return false;
            }
            Console.Write("Pass: ");
            string pass = Console.ReadLine();
            if (pass == null)
            {
                Console.WriteLine("Pas de mot de passe renseigné.\n");
                return false;
            }
            else
            {
                try
                {
                    string connexionString = "SERVER=localhost;PORT=3306;" +
                 "DATABASE=velomax;" +
                 "UID=" + uid + ";PASSWORD=" + pass;

                    MySqlConnection maConnexion = new MySqlConnection(connexionString);
                    VeloMax.connection = maConnexion;
                    VeloMax.connection.Open();
                    Console.WriteLine("Accès autorisé");

                    return true;
                }
                catch
                {
                    Console.WriteLine("Accès non autorisé");
                    VeloMax.connection = null;
                    return false;
                }
                if (IsConnectionReadOnly())
                {
                    Console.WriteLine("La connexion permet la lecture seulement");
                }
                else
                {
                    Console.WriteLine("La connexion permet la lecture et l'écriture");

                }
            }
        }


        /// <summary>
        /// Idem que la précédente, version raccourcie pour le débugage et gagner du temps dans la saisie des identifiants à chaque lancement du programme
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        static public bool DBConnect(string uid,string pass)
        {

            string connexionString = "SERVER=localhost;PORT=3306;" +
            "DATABASE=velomax;" +
            "UID=" + uid + ";PASSWORD=" + pass;

            MySqlConnection maConnexion = new MySqlConnection(connexionString);
            VeloMax.connection = maConnexion;
            VeloMax.connection.Open();
            Console.WriteLine("Accès autorisé");
            return true;


            
        }

        /// <summary>
        /// Méthode qui teste si la connection établié est en lecture seulement. Elle est utilisée dans Program.cs pour tester si ce que l'utilisateur demande de faire (création etc) est faisable avec la connection établie.
        /// </summary>
        /// <returns></returns>
        static public bool IsConnectionReadOnly()
        {
            return (connection.ConnectionString.Split('=')[4] == "bozo") ;
        }
        
        /// <summary>
        /// Méthode qui exécute et lance une requete en fonction de la requete en paramètre. Fonction simplificatrice pour certaines requetes mais pas toujours utilisables, car elle ne peut retourner qu'une seule valeur
        /// </summary>
        /// <param name="request"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string SendRequest(string request,string keyword)
        {
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string result = (string)reader[keyword].ToString();//besoin des guillemets que pour certaines requetes?

            reader.Close();
            command.Dispose();
            return result;
        }

        /// <summary>
        /// Méthode qui execute une requête pour la déletion d'un élément de la base de données
        /// </summary>
        /// <param name="table">Table dont l'élément est à supprimer </param>
        /// <param name="tableid">Nom de la colonne sur lequel le critère de suppression est mis</param>
        /// <param name="id">Valeur test pour suppression</param>
        /// <returns></returns>
        public static bool DeleteRequest(string table,string tableid, string id)
        {
           
            string request= "DELETE  from "+table+" where "+tableid+"='"+id+"';";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            int nb_affected_rows = command.ExecuteNonQuery();
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();

            return nb_affected_rows>0;
        }

        /// <summary>
        /// Méthode qui exécute une requete de modification d'un tuple
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <param name="newValue"></param>
        /// <param name="columnSelection"></param>
        /// <param name="selectionValue"></param>
        /// <returns></returns>
        public static bool UpdateRequest(string table,string columnName,string newValue,string columnSelection,string selectionValue)
        {
            string request = "Update " + table + " set "+columnName+"='"+newValue+"' where " + columnSelection + "='" + selectionValue + "';";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;
            int nb_affected_rows = command.ExecuteNonQuery();
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
            
            return nb_affected_rows > 0;

        }

        /// <summary>
        //Méthode qui affiche une liste d'objets quel que soit son type
        /// </summary>
        /// <param name="liste"></param>mé
        public static void Display(List<Object> liste)
        {
            foreach (Object obj in liste)
            {
                var item = Convert.ChangeType(obj, obj.GetType());
                Console.WriteLine(item);

                if (item.GetType()==typeof(Part))
                {
                    Part part = (Part)item;
                    if(part.StockPartNumber <5)
                    {
                        Utilities.RedWriteLine("Stock bas : Envisager une commande ! ");
                    }
                }
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

            }
        }

        //L'ensemble des méthodes Load_{Nom_table} permettent de récuperer sur la base de données l'ensemble d'une table et de la retourner
        #region LoadTables
        public static List<Address> LoadAddress()
        {
            List<Address> addresses = new List<Address>();
            string requete = "Select * from address;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Address address = new Address((int)reader["addressid"], (string)reader["street"], (string)reader["region"], (string)reader["postalcode"], (string)reader["city"]);
                addresses.Add(address);

            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            return addresses;
        }
        public static List<Bicycle> LoadBicycle(string orderingColumn="")
        {
            List<Bicycle> bicycles = new List<Bicycle>();
            string requete = "";
            if(orderingColumn=="")
            {
                requete = "Select * from bicycle;";
            }
            else if(orderingColumn=="type")
            {
                requete = "select * from bicycle order by type;";
            }
            else
            {
                requete = "select * from bicycle order by size;";
            }

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Bicycle bicycle = new Bicycle((int)reader["bicycleid"], (string)reader["bname"], (string)reader["size"], (decimal)reader["cost"], (string)reader["type"], reader.GetDateTime(5), reader.GetDateTime(6), (int)reader["stockbicyclenumber"]);
                bicycles.Add(bicycle);

            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            return bicycles;
        }
        public static List<Client> LoadClient()
        {
            List<Client> clients = new List<Client>();
            string requete = "Select * from client;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                //pour l instant on laisse l adresse non remplie
                int addressid = (int)reader["addressid"];
                Address ad = new Address(addressid, "", "", "", "");

                if ((reader["companyname"]).GetType() != typeof(string))
                {
                    Client client = new Client((int)reader["clientid"], (string)reader["clientname"], (string)reader["surname"], ad, (string)reader["mail"], (string)reader["phone"]);
                    clients.Add(client);

                }
                else
                {
                    Client client = new Client((int)reader["clientid"], (string)reader["companyname"], ad, (string)reader["mail"], (string)reader["phone"], (string)reader["contact"]);
                    clients.Add(client);

                }
            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            //ensuite on relpit l adresse avec une autre requete

            foreach (Client cli in clients)
            {
                Address ad = new Address(cli.ClientAddress.AddressID);
                //Debug.WriteLine(ad);
                cli.ClientAddress = ad;
            }

            return clients;

        }
        public static List<Composition> LoadComposition()
        {
            List<Composition> compositions = new List<Composition>();
            string requete = "Select * from composition;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Composition compo = new Composition(new Part((string)reader["partnum"], "", 0,0), new Bicycle((int)reader["bicycleid"], "", "", (int)0, "", DateTime.Now, DateTime.Now, 0));
                compositions.Add(compo);
            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();


            foreach (Composition compo in compositions)
            {
                Part part = new Part(compo.Part.PartNum);
                compo.Part = part;
                Bicycle bi = new Bicycle(compo.Bicycle.BicycleId);
                compo.Bicycle = bi;
            }


            return compositions;
        }
        public static List<LoyaltyProgram> LoadLoyaltyProgram()
        {
            List<LoyaltyProgram> programs = new List<LoyaltyProgram>();
            string requete = "Select * from loyaltyprogram;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                LoyaltyProgram program = new LoyaltyProgram((int)reader["programnum"], (int)reader["fee"], (int)reader["length"], (int)reader["discount"], (string)reader["programdesc"]);
                programs.Add(program);
            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            return programs;
        }
        public static List<OrderedBicycle> LoadOrderedBicycle()
        {
            List<OrderedBicycle> orderedbicyles = new List<OrderedBicycle>();
            string requete = "Select * from orderedbicycle;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                OrderedBicycle orderedbicycle = new OrderedBicycle(new Purchase((int)reader["purchasenum"]), new Bicycle((int)reader["bicycleid"], "", "", (decimal)0, "", DateTime.Now, DateTime.Now, 0), (int)reader["orderedbicyclenb"]);
                orderedbicyles.Add(orderedbicycle);
            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            foreach (OrderedBicycle bi in orderedbicyles)
            {
                bi.Purchase = new Purchase(bi.Purchase.PurchaseNum);
                bi.Bicycle = new Bicycle(bi.Bicycle.BicycleId);
            }

            return orderedbicyles;
        }
        public static List<OrderedPart> LoadOrderedPart()
        {
            List<OrderedPart> orderedparts = new List<OrderedPart>();
            string requete = "Select * from orderedpart;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                OrderedPart orderedpart = new OrderedPart(new Purchase((int)reader["purchasenum"]), new Part((string)reader["partnum"], "", 0,0), (int)reader["orderedpartnb"]);
                orderedparts.Add(orderedpart);
            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            foreach (OrderedPart part in orderedparts)
            {
                part.Purchase = new Purchase(part.Purchase.PurchaseNum, VeloMax.Connection);
                part.Part = new Part(part.Part.PartNum);
            }

            return orderedparts;
        }
        public static List<Part> LoadPart(string orderingColumn = "")
        {
            List<Part> parts = new List<Part>();
            string requete = "";
            if (orderingColumn == "")
            {
                requete = "Select * from part;";
            }
            else
            {
                requete = "Select * from part order by " + orderingColumn + ";";

            }
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Part part = new Part((string)reader["partnum"], (string)reader["partdesc"], (int)reader["stockpartnumber"],(int)reader["price"]);
                parts.Add(part);

            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            return parts;
        }
        public static List<ProvidedPart> LoadProvidedPart(string orderingColumn = "")
        {
            List<ProvidedPart> providedParts = new List<ProvidedPart>();
            string requete = "";
            if (orderingColumn == "")
            {
                requete = "Select * from providedpart;";
            }
            else
            {
                requete = "Select * from providedpart order by " + orderingColumn + ";";

            }
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProvidedPart providedpart = new ProvidedPart(new Part((string)reader["partnum"], "", 0,0), new Supplier((string)reader["siret"],""), (decimal)reader["price"], reader.GetDateTime(3), reader.GetDateTime(4), (int)reader["delay"]);
                providedParts.Add(providedpart);

            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            foreach (ProvidedPart part in providedParts)
            {
                part.Part = new Part(part.Part.PartNum);
                part.Supplier = new Supplier(part.Supplier.Siret);
            }


            return providedParts;
        }
        public static List<Subscription> LoadSubscription()
        {
            List<Subscription> subscriptions = new List<Subscription>();
            string requete = "Select * from subscription;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Subscription sub = new Subscription(new Client((int)reader["clientid"]), new LoyaltyProgram((int)reader["programnum"], 0, 0, 0, ""), (DateTime)reader["programstartdate"]);
                subscriptions.Add(sub);
            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            foreach (Subscription sub in subscriptions)
            {
                sub.Client = new Client(sub.Client.ClientID, VeloMax.Connection);
                sub.Program = new LoyaltyProgram(sub.Program.ProgramNum, VeloMax.Connection);
            }

            return subscriptions;
        }
        public static List<Purchase> LoadPurchase()
        {
            List<Purchase> purchases = new List<Purchase>();
            string requete = "Select * from purchase;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Purchase purchase = new Purchase((int)reader["purchaseNum"], reader.GetDateTime(1), reader.GetDateTime(2), new Client((int)reader["clientid"]), new Address((int)reader["addressid"], "", "", "", ""), (int)reader["purchasecost"]);
                purchases.Add(purchase);

            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            foreach (Purchase pur in purchases)
            {
                pur.Client = new Client(pur.Client.ClientID, VeloMax.Connection);
                pur.DeliveryAddress = new Address(pur.DeliveryAddress.AddressID);
            }

            return purchases;
        }
        public static List<Supplier> LoadSupplier()
        {
            List<Supplier> suppliers = new List<Supplier>();
            string requete = "Select * from supplier;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Supplier supplier = new Supplier((string)reader["siret"], (string)reader["supplierName"], new Address((int)reader["addressid"], "", "", "", ""), (string)reader["contactname"]);
                suppliers.Add(supplier);

            }
            Console.WriteLine();
            reader.Close();
            command.Dispose();

            foreach (Supplier supp in suppliers)
            {
                supp.SupplierAddress = new Address(supp.SupplierAddress.AddressID);
            }

            return suppliers;
        }
        #endregion
    }
}
