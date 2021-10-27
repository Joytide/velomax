using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    class Program
    {
        /// <summary>
        /// Menu principal pour la redirection des exercices et des fonctionnalités du programme
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.SetWindowSize(140, 40);

            //Instanciation et DB connexion
            VeloMax VeloMax = new VeloMax();
            Console.Clear();


            ConsoleKeyInfo cki;
            // Menu
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;

                Console.ResetColor();
                ShowMenu("Menu");
                if (VeloMax.Connection != null)
                {
                    Utilities.GreenWriteLine("Connected to the database");
                    if (VeloMax.IsConnectionReadOnly())
                    {
                        Console.WriteLine("La connexion permet la lecture seulement.");
                    }
                    else
                    {
                        Console.WriteLine("La connexion permet la lecture et l'écriture.");

                    }
                }

                else
                    Utilities.RedWriteLine("Not connected to the database");

                Console.Write("[+] 0 : Change connection\n");
                ShowMenu("Accès aux tables");
                Console.Write("[+] 1: Pièces\n"
                + "[+] 2 : Vélo\n"
                + "[+] 3 : Client\n"
                + "[+] 4 : Fournisseur\n"
                + "[+] 5 : Commande\n");
                ShowMenu("Gestion du stock");
                Console.Write("[+] 6 : Pièces en stock\n"
                + "[+] 7 : Pièces triées par fournisseur\n"
                + "[+] 8 : Vélos en stock\n"
                + "[+] 9 : Vélos par catégorie\n"
                + "[+] 10 : Vélos par taille\n");
                ShowMenu("Statistiques");
                Console.Write("[+] 11 : Nombre de ventes par item\n"
                + "[+] 12 : Clients par programmes\n"
                + "[+] 13 : Meilleurs clients par nombre d'items\n"
                + "[+] 14 : Meilleurs clients par cumul de prix\n"
                + "[+] 15 : Analyse des commandes\n");
                //Label supplier
                ShowMenu("Démonstration");
                Console.Write("[+] 16 : Lancer la démonstration\n");
                Console.Write("\n[+] 17 : Quitter\n");

                Utilities.BlueWriteLine("\n[+] Option nb > ",true);

                int option = Utilities.IntQuery(0, 17);
                if (VeloMax.Connection == null && option != 0 && option !=16)
                {
                    Utilities.RedWriteLine("[!] Please connect to the database first!");
                    cki = Console.ReadKey();
                    continue;
                }
                //Switchcase principal
                switch (option)
                {
                    case 0:
                        VeloMax.DBConnect();
                        break;
                    case 1:
                        PartTable(TableMenu("Pièces"));
                        break;
                    case 2:
                        BicycleTable(TableMenu("Vélos"));
                        break;
                    case 3:
                        ClientTable(TableMenu("Clients"));
                        break;
                    case 4:
                        SupplierTable(TableMenu("Fournisseurs"));
                        break;
                    case 5:
                        PurchaseTable(TableMenu("Commandes"));
                        break;
                    case 6:
                        List<Part> listeparts = VeloMax.LoadPart("partnum");
                        List<Object> objectpartsList = listeparts.ConvertAll(x => (Object)x);
                        VeloMax.Display(objectpartsList);
                        break;
                    case 7:
                        List<ProvidedPart> listeprovidedparts = VeloMax.LoadProvidedPart("siret");
                        List<Object> objectprovidedpartlist = listeprovidedparts.ConvertAll(x => (Object)x);
                        VeloMax.Display(objectprovidedpartlist);
                        break;
                    case 8:
                        List<Bicycle> listebicycles = VeloMax.LoadBicycle();
                        List<Object> objectbicyclelist = listebicycles.ConvertAll(x => (Object)x);
                        VeloMax.Display(objectbicyclelist);
                        break;
                    case 9:
                        List<Bicycle> listebicycles2 = VeloMax.LoadBicycle("type");
                        List<Object> objectbicyclelist2 = listebicycles2.ConvertAll(x => (Object)x);
                        VeloMax.Display(objectbicyclelist2); 
                        break;
                    case 10:
                        List<Bicycle> listebicycles3 = VeloMax.LoadBicycle("size");
                        List<Object> objectbicyclelist3 = listebicycles3.ConvertAll(x => (Object)x);
                        VeloMax.Display(objectbicyclelist3); 
                        break;
                    case 11:
                        SoldbyItem();
                        break;
                    case 12:
                        ClientsbyProgram();
                        break;
                    case 13:
                        BestClientsByParts();   
                        break;
                    case 14:
                        BestClientsByPrice();
                        break;
                    case 15:
                        PurchaseAnalysis();
                        break;
                    case 16:
                        Demo();

                        break;
                    default:
                        Utilities.RedWriteLine("[!] Sorry, i don't know this exercise yet... try again");
                        break;
                }
                Utilities.GreenWriteLine("[+] Press esc to exit, press anything else to return to the menu");
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);



            //Console.ReadLine();



        }
        /// <summary>
        /// Fonction d'affichage pour les titres dans les menus
        /// </summary>
        /// <param name="word"></param>
        static void ShowMenu(string word)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            string menu = "[" + word + "]";
            while (menu.Length < 90)
            {
                menu = "-" + menu;
                if (menu.Length < 90) { menu += "-"; }
            }

            Console.WriteLine(menu);
            Console.ResetColor();
        }
        /// <summary>
        /// Menu principal commun aux tables
        /// </summary>
        /// <param name="tablename">Nom de la table à afficher en</param>
        /// <returns>Choix de l'action demandée pour le switchcase de la table</returns>
        static int TableMenu(string tablename)
        {
            Console.Clear();
            ShowMenu(tablename);
            Console.Write("[+] 1 : Affichage\n"
            + "[+] 2 : Création\n"
            + "[+] 3 : Suppression\n"
            + "[+] 4 : Mise à jour\n"
            + "[+] 5 : Export XML\n"
            + "[+] 6 : Export JSON\n");
            if (tablename == "Clients")
            {
                Console.Write("[+] 7 : Export JSON des clients avec programmes de fidélités expirants\n"+
                    "\n[+] 8 : Quitter\n");
            }
            else
            {
                if (tablename == "Pièces")
                {
                    Console.Write("[+] 7 : Export XML des stock faibles et meilleur founisseur pour commandes\n" +
                    "\n[+] 8 : Quitter\n");
                }
                else
                {
                    Console.Write("\n[+] 7 : Quitter\n");
                }
            }
            
            Utilities.BlueWriteLine("\n[+] Option nb > ", true);
            int ret= Utilities.IntQuery(0, 8);
            Console.Clear();
            if (VeloMax.IsConnectionReadOnly() && (ret > 1 && ret < 5))
            {
                return -1;
            }

            return ret;
        }

        #region tables switchcases


        /// <summary>
        /// Fonction switchcase appliquant une action sur la table des pièces
        /// </summary>
        /// <param name="action">numero de l'action a effectuer</param>
        static void PartTable(int action)
        {
            switch (action)
            {
                case -1:
                    Utilities.RedWriteLine("Cette action n'est pas possible avec une connexion en lecture !");
                    break;
                case 1:
                    List<Part> liste = VeloMax.LoadPart();
                    List<Object> objectList = liste.ConvertAll(x => (Object)x);
                    VeloMax.Display(objectList);
                    break;
                case 2:
                    //Création
                    bool goodpartnum = false;
                    string partnum = "";
                    do
                    {
                        Utilities.BlueWriteLine("[-] Numéro de la pièce > ", true);
                        partnum = Console.ReadLine();
                        goodpartnum = VeloMax.SendRequest("select count(partnum) from part where partnum='" + partnum + "'", "count(partnum)") == "0";
                        if (!goodpartnum)
                        {
                            Utilities.RedWriteLine("This part number already exists!");
                        }
                    } while (!goodpartnum);

                    Utilities.BlueWriteLine("[-] Description (Cadre,...) > ", true);
                    string desc = Console.ReadLine();
                    Utilities.BlueWriteLine("[-] Nombre en stock > ", true);
                    int stock = Utilities.IntQuery(0, 1000000000);
                    Utilities.BlueWriteLine("[-] Prix > ", true);
                    int prix = Utilities.IntQuery(0, 1000000000);
                    Part newpart = new Part(partnum, desc, stock,prix);
                    newpart.InsertIntoDB();
                    Utilities.GreenWriteLine("[+] La pièce à été insérée dans la base de données!");

                    break;
                case 3:
                    //Suppression
                    bool success = false;
                    while(!success)
                    {
                        Utilities.BlueWriteLine("Part number to delete > ",true);
                        string partnumToDelete = Console.ReadLine(); 

                        VeloMax.DeleteRequest("composition", "partnum",partnumToDelete);
                        VeloMax.DeleteRequest("providedpart", "partnum", partnumToDelete);
                        VeloMax.DeleteRequest("orderedpart", "partnum", partnumToDelete);
                        success = VeloMax.DeleteRequest("part", "partnum", partnumToDelete);

                        if (success==false)
                        {
                            Utilities.RedWriteLine("This part number doesn't exist!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if(Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Deletion succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 4:
                    //MAJ
                    bool successUpdate = false;
                    while (!successUpdate)
                    {
                        Utilities.BlueWriteLine("Part number to update >", true);
                        string partnumToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("What do you want to update >", true);
                        string columnToUpdate = Console.ReadLine(); 
                        Utilities.BlueWriteLine("To which value >", true);
                        string newValue = Console.ReadLine();
                        successUpdate = VeloMax.UpdateRequest("part", columnToUpdate, newValue,"partnum",partnumToUpdate);

                        if (successUpdate == false)
                        {
                            Utilities.RedWriteLine("Update didn't affect any row!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                successUpdate = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Update succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 5:
                    Export.PartXMLExport();
                    Utilities.GreenWriteLine("[+] Les pièces ont été exportées vers ./debug/export/Part.xml ");
                    break;
                case 6:
                    List<Part> strictlist = VeloMax.LoadPart();
                    List<Object> objlist = strictlist.ConvertAll(x => (Object)x);
                    Export.JSONExport(objlist, "Part.json");
                    Utilities.GreenWriteLine("[+] Les pièces ont été exportées vers ./debug/export/Part.json ");
                    break;
                case 7:
                    Export.StockFaibleXMLExport();
                    Utilities.GreenWriteLine("[+] Les stocks faibles ont été exportés vers ./debug/export/StockFaible.xml ");
                    break;
                case 8:
                    break;
                default:
                    Utilities.RedWriteLine("[!] Sorry, i don't know this exercise yet... try again");
                    break;
            }
        }

        /// <summary>
        /// Fonction switchcase appliquant une action sur la table des vélos
        /// </summary>
        /// <param name="action">numero de l'action a effectuer</param>
        static void BicycleTable(int action)
        {
            switch (action)
            {
                case -1:
                    Utilities.RedWriteLine("Cette action n'est pas possible avec une connexion en lecture !");
                    break;
                case 1:
                    List<Bicycle> liste = VeloMax.LoadBicycle();
                    List<Object> objectList = liste.ConvertAll(x => (Object)x);
                    VeloMax.Display(objectList); 
                    break;
                case 2:
                    //Création
                    int bicyleid = Convert.ToInt32(VeloMax.SendRequest("select max(bicycleid) from bicycle", "max(bicycleid)")) + 1; //New unique ID generator

                    Utilities.BlueWriteLine("[-] Nom > ", true);
                    string bname = Console.ReadLine();
                    Utilities.BlueWriteLine("[-] Taille > ", true);
                    string size = Console.ReadLine();
                    Utilities.BlueWriteLine("[-] Prix (Euros) > ", true);
                    int cost = Utilities.IntQuery(0, 1000000000);
                    Utilities.BlueWriteLine("[-] Type (VTT,...) > ", true);
                    string type = Console.ReadLine();

                    Utilities.BlueWriteLine("En stock > ", true);
                    int stock = Utilities.IntQuery(0, 1000000000);



                    Bicycle newBicycle = new Bicycle(bicyleid, bname, size, cost, type, DateTime.Now, DateTime.Now, stock);
                    newBicycle.InsertIntoDB();
                    Utilities.GreenWriteLine("[+] Le vélo à été insérée dans la base de données! Il a l'ID " + bicyleid);

                    break;
                case 3:
                    //Suppression
                    bool success = false;
                    while (!success)
                    {
                        Utilities.BlueWriteLine("Bicycle id to delete >", true);
                        string numToDelete = Console.ReadLine();

                        VeloMax.DeleteRequest("composition", "bicycleid", numToDelete);
                        VeloMax.DeleteRequest("orderedbicycle", "bicycleid", numToDelete);
                        success = VeloMax.DeleteRequest("bicycle", "bicycleid", numToDelete);

                        if (success == false)
                        {
                            Utilities.RedWriteLine("This bicycle id doesn't exist!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Deletion succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 4:
                    //MAJ
                    bool successUpdate = false;
                    while (!successUpdate)
                    {
                        Utilities.BlueWriteLine("Bicycle number to update >", true);
                        string numToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("What do you want to update >", true);
                        string columnToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("To which value >", true);
                        string newValue = Console.ReadLine();
                        successUpdate = VeloMax.UpdateRequest("bicycle", columnToUpdate, newValue, "bicycleid", numToUpdate);

                        if (successUpdate == false)
                        {
                            Utilities.RedWriteLine("Update didn't affect any row!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                successUpdate = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Update succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 5:
                    Export.BicycleXMLExport();
                    Utilities.GreenWriteLine("[+] Les vélos ont été exportés vers ./debug/export/Bicycle.xml ");
                    break;
                case 6:
                    List<Bicycle> strictlist = VeloMax.LoadBicycle();
                    List<Object> objlist = strictlist.ConvertAll(x => (Object)x);
                    Export.JSONExport(objlist, "Bicycle.json");
                    Utilities.GreenWriteLine("[+] Les vélos ont été exportés vers ./debug/export/Bicycle.json ");
                    break;
                case 7:
                    break;
                default:
                    Utilities.RedWriteLine("[!] Sorry, i don't know this exercise yet... try again");
                    break;
            }
        }

        /// <summary>
        /// Fonction switchcase appliquant une action sur la table des clients
        /// </summary>
        /// <param name="action">numero de l'action a effectuer</param>
        static void ClientTable(int action)
        {
            switch (action)
            {
                case -1:
                    Utilities.RedWriteLine("Cette action n'est pas possible avec une connexion en lecture !");
                    break;
                case 1:
                    List<Client> liste = VeloMax.LoadClient();
                    List<Object> objectList = liste.ConvertAll(x => (Object)x);
                    VeloMax.Display(objectList); 
                    break;
                case 2:
                    //Création
                    int clientid = Convert.ToInt32(VeloMax.SendRequest("select max(clientid) from client", "max(clientid)")) + 1; //New unique ID generator
                    Utilities.BlueWriteLine("[-] Voulez vous créer un client individuel? > ", true);
                    bool indiv = Utilities.BoolQuery();
                    string nom = "";
                    string prenom = "";
                    string compagnyname = "";
                    string contact = "";
                    if (indiv)
                    {
                        Utilities.BlueWriteLine("[-] Nom > ", true);
                        nom = Console.ReadLine();
                        Utilities.BlueWriteLine("[-] Prénom > ", true);
                        prenom = Console.ReadLine();
                    }
                    else
                    {
                        Utilities.BlueWriteLine("[-] Nom de l'entreprise > ", true);
                        compagnyname = Console.ReadLine();
                        Utilities.BlueWriteLine("[-] Nom Prénom du contact entreprise > ", true);
                        contact = Console.ReadLine();
                    }
                    Utilities.BlueWriteLine("[-] Email > ", true);
                    string mail = Console.ReadLine();
                    Utilities.BlueWriteLine("[-] Téléphone > ", true);
                    string phone = Console.ReadLine();
                    Utilities.BlueWriteLine("[-] Veuillez renseigner l'adresse: ");
                    Address address = CreateNewAddress();

                    if (indiv)
                    {
                        Client newClient = new Client(clientid, nom, prenom, address, mail, phone);
                        newClient.InsertIntoDB();
                    }
                    else
                    {
                        Client newClient = new Client(clientid, compagnyname, address, mail, phone, contact);
                        newClient.InsertIntoDB();
                    }

                    Utilities.GreenWriteLine("[+] Le client à été insérée dans la base de données! Il a l'ID " + clientid);
                    break;
                case 3:
                    //Suppression
                    bool success = false;
                    while (!success)
                    {
                        Utilities.BlueWriteLine("client id to delete >", true);
                        string numToDelete = Console.ReadLine();

                        string request = "select purchasenum from purchase where clientid='" + numToDelete + "';";
                        MySqlCommand command = VeloMax.Connection.CreateCommand();
                        command.CommandText = request;
                        int nbaffected = command.ExecuteNonQuery();
                        MySqlDataReader reader = command.ExecuteReader();
                        List<string> purchasenumbers = new List<string>();
                        while (reader.Read())
                        {
                            purchasenumbers.Add((string)reader["purchasenum"].ToString());
                        }
                        reader.Close();
                        command.Dispose();

                        foreach (string nb in purchasenumbers)
                        {
                            VeloMax.DeleteRequest("orderedbicycle", "purchasenum", nb);
                            VeloMax.DeleteRequest("orderedpart", "purchasenum", nb);
                            success = VeloMax.DeleteRequest("purchase", "purchasenum", nb);
                        }


                        VeloMax.DeleteRequest("subscription", "clientid", numToDelete);
                        success = VeloMax.DeleteRequest("client", "clientid", numToDelete);

                        if (success == false)
                        {
                            Utilities.RedWriteLine("This client id doesn't exist!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Deletion succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 4:
                    //MAJ
                    bool successUpdate = false;
                    while (!successUpdate)
                    {
                        Utilities.BlueWriteLine("Client number to update >", true);
                        string numToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("What do you want to update >", true);
                        string columnToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("To which value >", true);
                        string newValue = Console.ReadLine();
                        successUpdate = VeloMax.UpdateRequest("client", columnToUpdate, newValue, "clientid", numToUpdate);

                        if (successUpdate == false)
                        {
                            Utilities.RedWriteLine("Update didn't affect any row!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                successUpdate = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Update succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 5:
                    Export.ClientXMLExport();
                    Utilities.GreenWriteLine("[+] Les clients ont été exportés vers ./debug/export/Client.xml ");
                    break;
                case 6:
                    List<Client> strictlist = VeloMax.LoadClient();
                    List<Object> objlist = strictlist.ConvertAll(x => (Object)x);
                    Export.JSONExport(objlist, "Client.json");
                    Utilities.GreenWriteLine("[+] Les clients ont été exportés vers ./debug/export/Client.json ");
                    break;
                case 7:
                    Export.ExpiringJSONExport();
                    Utilities.GreenWriteLine("[+] Les clients avec un programme de fidélité arrivant a expiration dans moins de 2 mois ont été exportés vers ./debug/export/ClientExpired.json ");
                    break;
                case 8:
                    break;
                default:
                    Utilities.RedWriteLine("[!] Sorry, i don't know this exercise yet... try again");
                    break;
            }
        }

        /// <summary>
        /// Fonction switchcase appliquant une action sur la table des fournisseurs
        /// </summary>
        /// <param name="action">numero de l'action a effectuer</param>
        static void SupplierTable(int action)
        {
            switch (action)
            {
                case -1:
                    Utilities.RedWriteLine("Cette action n'est pas possible avec une connexion en lecture !");
                    break;
                case 1:
                    List<Supplier> liste = VeloMax.LoadSupplier();
                    List<Object> objectList = liste.ConvertAll(x => (Object)x);
                    VeloMax.Display(objectList);
                    break;
                case 2:
                    //Création
                    bool goodsiret = false;
                    string siret = "";
                    do
                    {
                        Utilities.BlueWriteLine("[-] SIRET > ", true);
                        siret = Console.ReadLine();
                        goodsiret = VeloMax.SendRequest("select count(siret) from supplier where siret='" + siret + "'", "count(siret)") == "0";
                        if (!goodsiret)
                        {
                            Utilities.RedWriteLine("Ce SIRET existe déjà!");
                        }
                        else
                        {
                            if (siret.Length != 14)
                            {
                                Utilities.RedWriteLine("Le numéro SIRET doit faire 14 chiffres!");
                                goodsiret = false;
                            }
                        }
                    } while (!goodsiret);

                    Utilities.BlueWriteLine("[-] Nom du fournisseur > ", true);
                    string name = Console.ReadLine();
                    Utilities.BlueWriteLine("[-] Prénom Nom du contact fournisseur > ", true);
                    string contact = Console.ReadLine();
                    Utilities.BlueWriteLine("[-] Veuillez renseigner l'adresse: ");
                    Address address = CreateNewAddress();
                    Supplier newsupplier = new Supplier(siret, name, address, contact);
                    newsupplier.InsertIntoDB();


                    Utilities.GreenWriteLine("[+] Le fournisseur à été insérée dans la base de données!");
                    break;
                case 3:
                    //Suppression
                    bool success = false;
                    while (!success)
                    {
                        Utilities.BlueWriteLine("supplier id to delete >", true);
                        string numToDelete = Console.ReadLine();

                        VeloMax.DeleteRequest("providedpart", "siret", numToDelete);
                        success = VeloMax.DeleteRequest("supplier", "siret", numToDelete);

                        if (success == false)
                        {
                            Utilities.RedWriteLine("This supplier id doesn't exist!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Deletion succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 4:
                    //MAJ
                    bool successUpdate = false;
                    while (!successUpdate)
                    {
                        Utilities.BlueWriteLine("Supplier siret to update >", true);
                        string numToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("What do you want to update >", true);
                        string columnToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("To which value >", true);
                        string newValue = Console.ReadLine();
                        successUpdate = VeloMax.UpdateRequest("supplier", columnToUpdate, newValue, "siret", numToUpdate);

                        if (successUpdate == false)
                        {
                            Utilities.RedWriteLine("Update didn't affect any row!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                successUpdate = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Update succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 5:
                    Export.SupplierXMLExport();
                    Utilities.GreenWriteLine("[+] Les fournisseurs ont été exportés vers ./debug/export/Supplier.xml ");
                    break;
                case 6:
                    List<Supplier> strictlist = VeloMax.LoadSupplier();
                    List<Object> objlist = strictlist.ConvertAll(x => (Object)x);
                    Export.JSONExport(objlist, "Supplier.json");
                    Utilities.GreenWriteLine("[+] Les fournisseurs ont été exportés vers ./debug/export/Supplier.json ");
                    break;
                case 7:
                    break;
                default:
                    Utilities.RedWriteLine("[!] Sorry, i don't know this exercise yet... try again");
                    break;
            }
        }

        /// <summary>
        /// Fonction switchcase appliquant une action sur la table des commandes
        /// </summary>
        /// <param name="action">numero de l'action a effectuer</param>
        static void PurchaseTable(int action)
        {
            switch (action)
            {
                case -1:
                    Utilities.RedWriteLine("Cette action n'est pas possible avec une connexion en lecture !");
                    break;
                case 1:
                    List<Purchase> liste = VeloMax.LoadPurchase();
                    List<Object> objectList = liste.ConvertAll(x => (Object)x);
                    VeloMax.Display(objectList);
                    break;

                case 2:
                    //Création
                    int purchasenum = Convert.ToInt32(VeloMax.SendRequest("select max(purchasenum) from purchase", "max(purchasenum)")) + 1; //New unique ID generator

                    Utilities.BlueWriteLine("[-] ID client > ",true);
                    int clientid = Utilities.IntQuery(0, Convert.ToInt32(VeloMax.SendRequest("select max(clientid) from client", "max(clientid)")));
                    Utilities.BlueWriteLine("[-] Voulez vous utilisez une adresse différente que celle du client?");
                    bool newaddr = Utilities.BoolQuery();
                    Address address = null;
                    if (newaddr)
                    {
                        Utilities.BlueWriteLine("[-] Veuillez renseigner l'adresse: ");
                        address = CreateNewAddress();
                    }
                    else
                    {
                        address = new Address(Convert.ToInt32(VeloMax.SendRequest("select addressid from client where clientid=" + clientid, "addressid")));
                    }

                    Purchase newpurchase = new Purchase(purchasenum, DateTime.Now, DateTime.Now, new Client(clientid), address,0);
                    
                    //Add to DB after checking the purchase is not empty
                    List<OrderedPart> parts = new List<OrderedPart>();
                    List<OrderedBicycle> bicycles = new List<OrderedBicycle>();

                    Utilities.BlueWriteLine("[-] Voulez vous acheter des pièces?");
                    bool buyingpart = Utilities.BoolQuery();
                    if (buyingpart)
                    {
                        string partnum="substitute";
                        bool goodpartnum = false;
                        while (partnum != "")
                        {
                            do //Vérification partnum
                            {
                                Utilities.BlueWriteLine("[-] Référence pièce (laisser vide pour quitter)> ", true);
                                partnum = Console.ReadLine();
                                goodpartnum = VeloMax.SendRequest("select count(partnum) from part where partnum='" + partnum + "'", "count(partnum)") == "1";
                                if (partnum == "")
                                {
                                    break;
                                }
                                if (!goodpartnum)
                                {
                                    Utilities.RedWriteLine("Cette référence n'existe pas!");
                                }
                                else
                                {
                                    Utilities.BlueWriteLine("[-] Quantité > ", true);
                                    int orderedpartnb = Utilities.IntQuery(0,1000000000);
                                    OrderedPart part = new OrderedPart(newpurchase, new Part(partnum), orderedpartnb);
                                    parts.Add(part);
                                    newpurchase.PurchaseCost += (int)part.Part.Price * orderedpartnb;
                                    if (orderedpartnb>1)
                                        Utilities.GreenWriteLine("[+] Les pièces ont été ajouté à la commande!");
                                    else
                                        Utilities.GreenWriteLine("[+] La pièce à été ajouté à la commande!");
                                }
                            } while (!goodpartnum);
                        }
                        

                    }
                    Utilities.BlueWriteLine("[-] Voulez vous acheter des vélos?");
                    bool buyingbicycle = Utilities.BoolQuery();
                    if (buyingbicycle)
                    {
                        int bicycleid = -1;
                        bool goodbicycleid = false;
                        while (bicycleid != 0)
                        {
                            do //Vérification bicycleid
                            {
                                Utilities.BlueWriteLine("[-] Référence vélo (0 pour quitter) > ", true);
                                bicycleid = Utilities.IntQuery(0,10000000);
                                goodbicycleid = VeloMax.SendRequest("select count(bicycleid) from bicycle where bicycleid=" + bicycleid, "count(bicycleid)") == "1";
                                if (bicycleid == 0)
                                {
                                    break;
                                }
                                if (!goodbicycleid)
                                {
                                    Utilities.RedWriteLine("Cette référence n'existe pas!");
                                }
                                else
                                {
                                    Utilities.BlueWriteLine("[-] Quantité > ", true);
                                    int orderedbicyclenb = Utilities.IntQuery(0, 1000000000);
                                    OrderedBicycle bicycle = new OrderedBicycle(newpurchase, new Bicycle(bicycleid), orderedbicyclenb);
                                    bicycles.Add(bicycle);
                                    newpurchase.PurchaseCost += (int)bicycle.Bicycle.Cost * orderedbicyclenb;

                                    if (orderedbicyclenb > 1)
                                        Utilities.GreenWriteLine("[+] Les vélos ont été ajouté à la commande!");
                                    else
                                        Utilities.GreenWriteLine("[+] Le vélo a été ajouté à la commande!");
                                }
                            } while (!goodbicycleid);
                        }
                    }
                    if (bicycles.Count()==0 && parts.Count()==0)
                    {
                        Utilities.RedWriteLine("Votre commande est vide! Elle n'a pas été ajoutée.");
                    }
                    else
                    {
                        newpurchase.InsertIntoDB();
                        foreach (OrderedBicycle b in bicycles)
                        {
                            b.InsertIntoDB();
                        }
                        foreach (OrderedPart p in parts)
                        {
                            p.InsertIntoDB();
                        }
                        Utilities.GreenWriteLine("[+] La commande à été ajouté dans la base de données!");
                    }









                    break;
                case 3:
                    //Suppression
                    bool success = false;
                    while (!success)
                    {
                        Utilities.BlueWriteLine("purchase number to delete >", true);
                        string numToDelete = Console.ReadLine();

                        VeloMax.DeleteRequest("orderedbicycle", "purchasenum", numToDelete);
                        VeloMax.DeleteRequest("orderedpart", "purchasenum", numToDelete);
                        success = VeloMax.DeleteRequest("purchase", "purchasenum", numToDelete);

                        if (success == false)
                        {
                            Utilities.RedWriteLine("This purchase number doesn't exist!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                success = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Deletion succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 4:
                    //MAJ
                    bool successUpdate = false;
                    while (!successUpdate)
                    {
                        Utilities.BlueWriteLine("Purchase number to update >", true);
                        string numToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("What do you want to update >", true);
                        string columnToUpdate = Console.ReadLine();
                        Utilities.BlueWriteLine("To which value >", true);
                        string newValue = Console.ReadLine();
                        successUpdate = VeloMax.UpdateRequest("purchase", columnToUpdate, newValue, "purchasenum", numToUpdate);

                        if (successUpdate == false)
                        {
                            Utilities.RedWriteLine("Update didn't affect any row!");
                            Utilities.GreenWriteLine("[+] Press esc to to return to the menu, press anything else to try again ");
                            if (Console.ReadKey().Key == ConsoleKey.Escape)
                            {
                                successUpdate = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Update succeeded");
                            Console.ReadLine();
                        }
                        Console.Clear();
                    }
                    break;
                case 5:
                    Export.PurchaseXMLExport();
                    Utilities.GreenWriteLine("[+] Les commandes ont été exportées vers ./debug/export/Purchase.xml ");
                    break;
                case 6:
                    List<Purchase> strictlist = VeloMax.LoadPurchase();
                    List<Object> objlist = strictlist.ConvertAll(x => (Object)x);
                    Export.JSONExport(objlist, "Purchase.json");
                    Utilities.GreenWriteLine("[+] Les commandes ont été exportées vers ./debug/export/Purchase.json ");
                    break;
                case 7:
                    break;
                default:
                    Utilities.RedWriteLine("[!] Sorry, i don't know this exercise yet... try again");
                    break;
            }
        }

        /// <summary>
        /// Affichage des pièces et vélo ainsi que de leur stock et le nombre de fois où elles ont été achetées
        /// </summary>
        static void SoldbyItem()
        {
            string requete = "select partnum, stockpartnumber,sum(orderedpartnb) from orderedpart natural join part group by partnum order by sum(orderedpartnb) desc;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                Console.WriteLine("Numéro de pièce : "+(string)reader["partnum"].ToString()+"  Nombre en stock : "+(int)Convert.ToInt32(reader["stockpartnumber"])+"  Nombre total d'achats : "+(int)Convert.ToInt32(reader["sum(orderedpartnb)"]));
            }
            reader.Close();
            command.Dispose();

            Console.WriteLine();
            string requete2 = "select bicycleid, stockbicyclenumber,sum(orderedbicyclenb) from orderedbicycle natural join bicycle group by bicycleid order by sum(orderedbicyclenb) desc;";
            MySqlCommand command2 = VeloMax.Connection.CreateCommand();
            command2.CommandText = requete2;
            MySqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                Console.WriteLine("Id de vélo : " + (string)reader2["bicycleid"].ToString() + "  Nombre en stock : " + (int)Convert.ToInt32(reader2["stockbicyclenumber"]) + "  Nombre total d'achats : " + (int)Convert.ToInt32(reader2["sum(orderedbicyclenb)"]));
            }
            reader2.Close();
            command2.Dispose();
        }

        /// <summary>
        /// Affichage de tous les clients avec programme de fidélité
        /// </summary>
        static void ClientsbyProgram()
        {
            string requete = "select clientid,programnum,programstartdate,length from client natural join subscription natural join loyaltyprogram order by programnum; ";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(" Numéro du programme  : " + (int)Convert.ToInt32(reader["programnum"])+ "  ClientID : " + (string)reader["clientid"].ToString() + "  Date d'expiration: " + reader.GetDateTime(2).AddYears((int)Convert.ToInt32(reader["length"])));
            }
            reader.Close();
            command.Dispose();
        }

        /// <summary>
        /// Affiche le montant total dépensé pour chaque clients
        /// </summary>
        static void TotalPricebyClient()
        {
            string requete = "select clientid,sum(purchasecost) from purchase group by clientid; ";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Numéro de client : "+(int)Convert.ToInt32(reader["clientid"])+"  Montant total dépensé : "+(int)Convert.ToInt32(reader["sum(purchasecost)"])) ;
            }
            reader.Close();
            command.Dispose();
        }

        /// <summary>
        /// Affiche des statistiques sur le prix moyen des commandes, le nombre moyen de vélos et de pièces par commandes
        /// </summary>
        static void PurchaseAnalysis()
        {
            string requete = "select avg(purchasecost) from purchase;";

            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Prix moyen des commandes : " + (decimal)Convert.ToDecimal(reader["avg(purchasecost)"]));
            }
            reader.Close();
            command.Dispose();

            string requete2 = "select avg(sum) from(select sum(orderedbicyclenb) as sum from purchase natural join orderedbicycle group by purchasenum) as sumorderedbicycle;";
            MySqlCommand command2 = VeloMax.Connection.CreateCommand();
            command2.CommandText = requete2;
            MySqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                Console.WriteLine("Nombre moyen de vélos par commande : " +(decimal)Convert.ToDecimal(reader2["avg(sum)"]) );
            }
            reader2.Close();
            command2.Dispose();

            string requete3 = "select avg(sum) from (select sum(orderedpartnb) as sum from purchase natural join orderedpart group by purchasenum) as sumorderedpart;";
            MySqlCommand command3 = VeloMax.Connection.CreateCommand();
            command3.CommandText = requete3;
            MySqlDataReader reader3 = command3.ExecuteReader();
            while (reader3.Read())
            {
                Console.WriteLine("Nombre moyen de pièces par commande : " + (decimal)Convert.ToDecimal(reader3["avg(sum)"]));
            }
            reader3.Close();
            command3.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        static void PartNbbySupplier()
        {
            string requete = "select siret, count(*) from supplier natural join providedpart group by siret; ";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("Siret de l'entreprise : " + (string)(reader["siret"]) + "  Nombre de pièces fournies: " + (int)Convert.ToInt32(reader["count(*)"]));
            }
            reader.Close();
            command.Dispose();

        }

        /// <summary>
        /// Affiche les 5 meilleurs client par nombre de pièces commandées
        /// </summary>
        static void BestClientsByParts()
        {
            Console.WriteLine("Les 5 meilleurs clients par nombre de pièces commandées : ");
            string requete = "select clientid, sum(orderedpartnb) from purchase natural join orderedpart group by clientid order by sum(orderedpartnb) desc;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            int count = 0;
            while (reader.Read() && count<5)
            {
                Console.WriteLine("ID du client: " + (int)Convert.ToInt32(reader["clientid"]) + "  Nombre total de pièces commandées : " + (int)Convert.ToInt32(reader["sum(orderedpartnb)"]));
                count += 1;
            }
            reader.Close();
            command.Dispose();
        }

        /// <summary>
        /// Affiche les 5 meilleurs client par cumul de prix
        /// </summary>
        static void BestClientsByPrice()
        {
            Console.WriteLine("Les 5 meilleurs clients par montant total commandé : ");
            string requete = "select clientid, sum(purchasecost) from purchase group by clientid order by sum(purchasecost) desc;";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            int count = 0;
            while (reader.Read() && count < 5)
            {
                Console.WriteLine("ID du client: " + (int)Convert.ToInt32(reader["clientid"]) + "  Montant total commandé : " + (int)Convert.ToInt32(reader["sum(purchasecost)"]));
                count += 1;
            }
            reader.Close();
            command.Dispose();
        }

        /// <summary>
        /// Fonction démonstration pour la revue de code TD10 avec les 6 actions demandées
        /// </summary>
        static void Demo()
        {
            //Nombre de clients
            string requete = "select count(clientid) from client ; ";
            MySqlCommand command = VeloMax.Connection.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("Nombre de clients : "+ (int)Convert.ToInt32(reader["count(clientid)"]));
            reader.Close();
            command.Dispose();
            Console.WriteLine("\npress enter to continue");
            Console.ReadLine();

            //Noms des clients avec cumul des commandes en euros
            Console.WriteLine("Montant total des achats par client  :");

            TotalPricebyClient();
            Console.WriteLine("\npress enter to continue");
            Console.ReadLine();


            //liste des produits avec une quantité en stock <=2
            //parts
            Console.WriteLine("Pièces avec moins de 2 unités en stock :");
            string requete2 = "select partnum, stockpartnumber from part where stockpartnumber<=2 ; ";
            MySqlCommand command2 = VeloMax.Connection.CreateCommand();
            command2.CommandText = requete2;
            MySqlDataReader reader2 = command2.ExecuteReader();
            while(reader2.Read())
            {
                Console.WriteLine("Part number : " + (string)reader2["partnum"].ToString() + "  Nombre en stock : " + (int)Convert.ToInt32(reader2["stockpartnumber"]));
            }
            reader2.Close();
            command2.Dispose();
            Console.WriteLine("\npress enter to continue");
            Console.ReadLine();

            //bicycles
            Console.WriteLine("Vélos avec moins de 2 unités en stock :");
            string requete3 = "select bicycleid,stockbicyclenumber from bicycle where stockbicyclenumber<=2 ; ";
            MySqlCommand command3 = VeloMax.Connection.CreateCommand();
            command3.CommandText = requete3;
            MySqlDataReader reader3 = command3.ExecuteReader();
            reader3.Read();
            Console.WriteLine("Bicycle id : " + (int)Convert.ToInt32(reader3["bicycleid"]) + "  Nombre en stock : " + (int)Convert.ToInt32(reader3["stockbicyclenumber"]));
            reader3.Close();
            command3.Dispose();
            Console.WriteLine("\npress enter to continue");
            Console.ReadLine();

            //nombre de pièces par fournisseur
            Console.WriteLine("Nombre de pièces fournis par fournisseur : ");
            PartNbbySupplier();
            Console.WriteLine("\npress enter to continue");
            Console.ReadLine();

            //export en json/xml d'une table
            Console.WriteLine("Exemple d'export en XML: ");
            Export.ClientXMLExport();
            Utilities.GreenWriteLine("[+] Les clients ont été exportés vers ./debug/export/Client.xml ");
            Console.WriteLine("\npress enter to continue");
            Console.ReadLine();
        }

        /// <summary>
        /// Demande a l'utilisateur de créer une nouvelle adresse
        /// </summary>
        /// <returns>L'adresse créée</returns>
        static Address CreateNewAddress()
        {
            int addressid = Convert.ToInt32(VeloMax.SendRequest("select max(addressid) from address", "max(addressid)")) + 1; //New unique ID generator

            Utilities.BlueWriteLine("[-] Pays/Région > ", true);
            string region = Console.ReadLine();
            Utilities.BlueWriteLine("[-] Code Postal > ", true);
            string postal = Console.ReadLine();
            Utilities.BlueWriteLine("[-] Ville > ", true);
            string city = Console.ReadLine();
            Utilities.BlueWriteLine("[-] Rue > ", true);
            string street = Console.ReadLine();

            Address newaddr = new Address(addressid, street, region, postal, city);
            newaddr.InsertIntoDB();
            return newaddr;
        }
        #endregion

    }
}
