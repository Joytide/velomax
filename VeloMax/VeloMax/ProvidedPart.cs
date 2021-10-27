using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    public class ProvidedPart
    {
        #region attributes 
        Part part;
        Supplier supplier;
        decimal price;
        DateTime introDate;
        DateTime deprecDate;
        int delay;
        #endregion

        #region constructors
        public ProvidedPart() { }
        public ProvidedPart(Part part, Supplier supplier, decimal price, DateTime introDate, DateTime deprecDate, int delay)
        {
            this.part = part;
            this.supplier = supplier;
            this.price = price;
            this.introDate = introDate;
            this.deprecDate = deprecDate;
            this.delay = delay;
        }
        #endregion

        #region properties
        public Part Part { get { return this.part; } set { this.part = value; } }
        public Supplier Supplier { get { return this.supplier; } set { this.supplier = value; } }
        public decimal Price { get { return this.price; } set { this.price = value; } }
        public DateTime IntroDate { get { return this.introDate; } set { this.introDate = value; } }
        public DateTime DeprecDate { get { return this.deprecDate; } set { this.deprecDate = value; } }
        public int Delay { get { return this.delay; } set { this.delay = value; } }

        #endregion

        #region methods
        public override string ToString()
        {
            return "  Siret du fournisseur : " + supplier.Siret +" "+part ;
        }
        public void InsertIntoDB(MySqlConnection co)
        {
            string request = "INSERT INTO `VeloMax`.`providedpart` (`partnum`, `siret`, `price`, `introdate`, `deprecateddate`,`delay`) VALUES (" + this.part.PartNum + ", " + this.supplier.Siret + ", " + this.price + ", '" + +introDate.Year + "-" + introDate.Month + "-" + introDate.Day + " " + introDate.Hour + ":" + introDate.Minute + ":" + introDate.Second  + "', '"+ deprecDate.Year + "-" + deprecDate.Month + "-" + deprecDate.Day + " " + deprecDate.Hour + ":" + deprecDate.Minute + ":" + deprecDate.Second + "', " + this.delay + ");";

            MySqlCommand command = co.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion

    }
}
