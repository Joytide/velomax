using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace VeloMax
{
    class Composition
    {

        #region attributes 
        Part part;
        Bicycle bicycle;
        #endregion

        #region constructors
        public Composition(Part part, Bicycle bicycle)
        {
            this.part = part;
            this.bicycle = bicycle;
        }
        #endregion

        #region properties
        public Part Part { get { return this.part; } set { this.part = value; } }
        public Bicycle Bicycle { get { return this.bicycle; } set { this.bicycle = value; } }

        #endregion

        #region methods
        public override string ToString()
        {
            return "Composition : \n    " + bicycle + "\n      " + part;
        }
        public void InsertIntoDB(MySqlConnection co)
        {
            string request = "INSERT INTO `VeloMax`.`composition` (`partnum`, `bicycleid`) VALUES (" + this.part.PartNum + ", " + this.bicycle.BicycleId + ");";

            MySqlCommand command = co.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();

            #endregion

        }
    }
}
