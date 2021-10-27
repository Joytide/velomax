using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    public class Export
    {
        /// <summary>
        /// Permet l'export vers le dossier debug/export de n'importe quelle table en JSON
        /// </summary>
        /// <param name="list">list d'objects à exporter</param>
        /// <param name="filename">nom du fichier d'export (avec le .json)</param>
        public static void JSONExport(List<Object> list,string filename)
        {
            string json =Newtonsoft.Json.JsonConvert.SerializeObject(list);
            System.IO.Directory.CreateDirectory("exports");
            File.WriteAllText("exports\\"+filename, json);
        }

        /// <summary>
        /// Permet l'export en JSON des clients ayant un programme de fidelité expirant dans moins de 2 mois
        /// </summary>
        public static void ExpiringJSONExport()
        {
            string request = "Select * from client natural join subscription natural join loyaltyprogram where date_add(curdate(), INTERVAL 2 MONTH)>date_add(programstartdate, INTERVAL length YEAR);";
            List<Client> clients = new List<Client>();
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
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
            reader.Close();
            command.Dispose();
            foreach (Client cli in clients)
            {
                Address ad = new Address(cli.ClientAddress.AddressID);
                cli.ClientAddress = ad;
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(clients);
            System.IO.Directory.CreateDirectory("exports");
            File.WriteAllText("exports\\" + "ClientExpired.json", json);
        }

        /// <summary>
        /// Permet l'export en XML des pièces ayant un stock inférieur à 5 avec le meilleur fournisseur pour une commande
        /// </summary>
        public static void StockFaibleXMLExport()
        {
            List<ProvidedPart> providedParts = new List<ProvidedPart>();
            string request = "select partnum,siret,price,introdate,deprecateddate,delay from "+
            "(select pp.partnum as part, min(pp.price) as prix from providedpart pp join part p where pp.partnum = p.partnum and p.stockpartnumber < 5 group by pp.partnum)"+
            " as t natural join providedpart where part = partnum and prix = price;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = request;

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ProvidedPart providedpart = new ProvidedPart(new Part((string)reader["partnum"], "", 0, 0), new Supplier((string)reader["siret"], ""), (decimal)reader["price"], reader.GetDateTime(3), reader.GetDateTime(4), (int)reader["delay"]);
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
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<ProvidedPart>));
            System.IO.Directory.CreateDirectory("exports");
            var path = "exports\\StockFaible.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            Console.WriteLine(path);
            writer.Serialize(file, providedParts);
            file.Close();


        }

        /// <summary>
        /// Permet l'export vers le dossier debug/export de la table pièce en XML
        /// </summary>
        public static void PartXMLExport()
        {
            List<Part> list = VeloMax.LoadPart();
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Part>));
            System.IO.Directory.CreateDirectory("exports");
            var path = "exports\\Part.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            Console.WriteLine(path);
            writer.Serialize(file, list);
            file.Close();
        }

        /// <summary>
        /// Permet l'export vers le dossier debug/export de la table vélo en XML
        /// </summary>
        public static void BicycleXMLExport()
        {
            List<Bicycle> list = VeloMax.LoadBicycle();
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Bicycle>));

            System.IO.Directory.CreateDirectory("exports");
            var path = "exports\\Bicycle.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            Console.WriteLine(path);
            writer.Serialize(file, list);
            file.Close();
        }

        /// <summary>
        /// Permet l'export vers le dossier debug/export de la table client en XML
        /// </summary>
        public static void ClientXMLExport()
        {
            List<Client> list = VeloMax.LoadClient();
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Client>));

            System.IO.Directory.CreateDirectory("exports");
            var path = "exports\\Client.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            Console.WriteLine(path);
            writer.Serialize(file, list);
            file.Close();
        }

        /// <summary>
        /// Permet l'export vers le dossier debug/export de la table fournisseur en XML
        /// </summary>
        public static void SupplierXMLExport()
        {
            List<Supplier> list = VeloMax.LoadSupplier();
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Supplier>));

            System.IO.Directory.CreateDirectory("exports");
            var path = "exports\\Supplier.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            Console.WriteLine(path);
            writer.Serialize(file, list);
            file.Close();
        }

        /// <summary>
        /// Permet l'export vers le dossier debug/export de la table commande en XML
        /// </summary>
        public static void PurchaseXMLExport()
        {
            List<Purchase> list = VeloMax.LoadPurchase();
            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Purchase>));

            System.IO.Directory.CreateDirectory("exports");
            var path = "exports\\Purchase.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            Console.WriteLine(path);
            writer.Serialize(file, list);
            file.Close();
        }
    }
}
