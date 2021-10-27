using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    class Subscription
    {


        #region attributes 
        Client client;
        LoyaltyProgram program;
        DateTime programStartDate;
        #endregion

        #region constructors
        public Subscription (Client client, LoyaltyProgram program, DateTime programStartDate)
        {
            this.client = client;
            this.program = program;
            this.programStartDate = programStartDate;
        }
        #endregion

        #region properties
        public Client Client { get { return this.client; } set { this.client = value; } }
        public LoyaltyProgram Program { get { return this.program; } set { this.program = value; } }
        public DateTime ProgramStartdate { get { return this.programStartDate; } }

        #endregion

        #region methods
        public override string ToString()
        {
            return "Souscription : \n    " + client + "\n     " + program + "\n    Date de début : " + programStartDate.ToString();
        }
        public void InsertIntoDB(MySqlConnection co)
        {
            string request = "INSERT INTO `VeloMax`.`subscription` (`clientid`, `programnum`, `programstartdate`) VALUES (" + this.client.ClientID + ", " + this.program.ProgramNum + ", '" + programStartDate.Year + "-" + programStartDate.Month + "-" + programStartDate.Day + " " + programStartDate.Hour + ":" + programStartDate.Minute + ":" + programStartDate.Second + "');";

            MySqlCommand command = co.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion



    }
}
