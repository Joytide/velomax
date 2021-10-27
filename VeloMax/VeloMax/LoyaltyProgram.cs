using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    class LoyaltyProgram
    {

        #region attributes 
        int programNum;
        int fee;
        int length;
        int discount;
        string programDesc;
        #endregion

        #region constructors
        public LoyaltyProgram(int programNum,int fee,int length,int discount, string programDesc)
        {
            this.programNum = programNum;
            this.fee = fee;
            this.length = length;
            this.discount = discount;
            this.programDesc = programDesc;
        }
        public LoyaltyProgram(int programNum, MySqlConnection co)
        {
            string requete = "Select * from loyaltyprogram where programnum=" + Convert.ToString(programNum) + ";";
            MySqlCommand command = co.CreateCommand();
            command.CommandText = requete;

            MySqlDataReader reader = command.ExecuteReader();
            reader.Read();
            this.programNum = programNum;
            this.fee = (int)reader["fee"];
            this.length = (int)reader["length"];
            this.discount = (int)reader["discount"];
            this.programDesc = (string)reader["programDesc"];

            reader.Close();
            command.Dispose();
        }
        #endregion

        #region properties
        public int ProgramNum { get { return this.programNum; } }
        public int Fee { get { return this.fee; } }
        public int Length { get { return this.length; } }
        public int Discount { get { return this.discount; } }
        public string ProgramDesc { get { return this.programDesc; } }

        #endregion

        #region methods
        public override string ToString()
        {
            return "Programme de fidélité :\n  ID : " + programNum + "\n    Prix : " + fee + "\n    Durée : " + length + "\n     Réduction : " + discount + "\n    Description : " + programDesc;
        }
        public void InsertIntoDB(MySqlConnection co)
        {
            string request = "INSERT INTO `VeloMax`.`loyaltyprogram` (`programnum`, `fee`, `length`, `discount`, `programdesc`) VALUES (" + this.programNum + ", " + this.fee + ", " + this.length + ", " + this.discount + ", '" + this.programDesc + "');";

            MySqlCommand command = co.CreateCommand();
            command.CommandText = request;
            MySqlDataReader reader = command.ExecuteReader();
            reader.Close();
            command.Dispose();
        }
        #endregion



    }
}
