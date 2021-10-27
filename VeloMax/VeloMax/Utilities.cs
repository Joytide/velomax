using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeloMax
{   /// <summary>
    /// This class regroups many utilities used in input sanitization, tostrings for debug, converting
    /// </summary>
    public class Utilities
    {
        //==================================================================================================================================================================================================================================================
        // WRITE WITH COLORS
        //==================================================================================================================================================================================================================================================

        #region Write with colors

        /// <summary>
        /// Print the data in green for validation text
        /// </summary>
        /// <param name="data">the data to be printed</param>
        public static void GreenWriteLine(string data)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(data);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints the data in dark cyan for a nice looking console
        /// </summary>
        /// <param name="data">the data to be printed</param>
        /// <param name="dropline">boolean to know if you should drop the newline</param>
        public static void BlueWriteLine(string data,bool dropline=false)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            if (dropline)
                Console.Write(data);
            else
                Console.WriteLine(data);
            Console.ResetColor();
        }


        /// <summary>
        /// Prints the data in red for errors
        /// </summary>
        /// <param name="data">the data to be printed</param>
        public static void RedWriteLine(string data)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(data);
            Console.ResetColor();
        }




        #endregion

        //==================================================================================================================================================================================================================================================
        // CONVERT INDIAN/INT
        //==================================================================================================================================================================================================================================================

        //==================================================================================================================================================================================================================================================
        // SHOW FUNCTIONS (TO DEBUG)
        //==================================================================================================================================================================================================================================================

        #region Show functions (to debug)

        /// <summary>
        /// Shows a long string of bits with a space char at each 8 bits, for debug mainly
        /// </summary>
        /// <param name="bits">the long bit string</param>
        public static void ShowBitString(string bits)
        {
            for (int i = 0; i < bits.Length; i++)
            {
                Console.Write(bits[i]);
                if ((i + 1) % 8 == 0)
                {
                    Console.Write(" ");
                }

            }
        }



        /// <summary>
        /// Show a string tab, debug function
        /// </summary>
        /// <param name="var">the tab to be shown</param>
        public static void ShowArray(string[] var)
        {
            for (int i = 0; i < var.Length; i++)
            {
                Console.Write(var[i] + " ");
                if (i == 53) Console.Write("[HEADEREND]");
                if (i > 53 && i % 3 == 2)
                {
                    Console.Write(".");
                }
            }
        }



        /// <summary>
        /// Show a int tab, debug function
        /// </summary>
        /// <param name="tab">the tab to be shown</param>
        public static void ShowTab(int[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                Console.Write(tab[i] + " ");
            }
            Console.WriteLine("");
        }



        /// <summary>
        /// Show a byte tab, debug function
        /// </summary>
        /// <param name="tab">the tab to be shown</param>
        public static void ShowByteTab(byte[] tab)
        {
            for (int i = 0; i < tab.Length; i++)
            {
                Console.Write(tab[i] + " ");
            }
            Console.WriteLine("");
        }

        #endregion

        //==================================================================================================================================================================================================================================================
        // SANITIZE
        //==================================================================================================================================================================================================================================================

        #region Sanitize

        /// <summary>
        /// Sanitize user input over a query for an int
        /// </summary>
        /// <param name="min">the minimum of the int queried</param>
        /// <param name="max">the maximum of the int queried</param>
        /// <returns>the int queried</returns>
        public static int IntQuery(int min, int max) //Méthode qui demande et vérifie si c'est bien un chiffre dans l'intervalle
        {
            int GoodInt = 0;
            bool IntNotGood = true; // Cet Méthode peut tourner infiniment si l'on ne choisit pas un chiffre
            while (IntNotGood)
            {

                string query = Console.ReadLine();
                if (query == "*")
                {
                    return 99999;
                }
                else
                {
                    bool IsQueryInt = Int32.TryParse(query, out int INT); //Error handling
                    if (IsQueryInt == true) //Error handling
                    {

                        GoodInt = INT;
                        if (GoodInt >= min)
                        {
                            if (GoodInt <= max)
                            {
                                IntNotGood = false;
                            }
                            else Utilities.RedWriteLine("[!] Woops too much, try again");

                        }
                        else Utilities.RedWriteLine("[!] Woops that's not enough probably, try again");

                    }
                    else
                    {
                        Utilities.RedWriteLine("[!] Are you sure that's even a number?");
                    }
                }
            }
            return GoodInt;
        }



        /// <summary>
        /// Sanitize user input over a query for a double
        /// </summary>
        /// <param name="min">the minimum of the double queried</param>
        /// <param name="max">the maximum of the double queried</param>
        /// <returns>the double queried</returns>
        public static double DoubleQuery(double min, double max) //Méthode qui demande et vérifie si c'est bien un chiffre dans l'intervalle
        {
            double GoodInt = 0;
            bool IntNotGood = true; // Cet Méthode peut tourner infiniment si l'on ne choisit pas un chiffre
            while (IntNotGood)
            {

                string query = Console.ReadLine();
                if (query == "*")
                {
                    return 99999;
                }
                else
                {
                    bool IsQueryInt = double.TryParse(query, out double INT); //Error handling
                    if (IsQueryInt == true) //Error handling
                    {

                        GoodInt = INT;
                        if (GoodInt > min)
                        {
                            if (GoodInt <= max)
                            {
                                IntNotGood = false;
                            }
                            else Utilities.RedWriteLine("[!] Woops too much, try again");

                        }
                        else Utilities.RedWriteLine("[!] Woops that's not enought probably, try again");

                    }
                    else
                    {
                        Utilities.RedWriteLine("[!] Are you sure that's even a number?");
                    }
                }
            }
            return GoodInt;
        }



        /// <summary>
        /// Ask the user for a bool, sanitize his input
        /// </summary>
        /// <returns>the bool inputed</returns>
        public static bool BoolQuery()
        {
            bool HasGivenGoodBool = false;
            bool returnedbool = false;
            while (!HasGivenGoodBool)
            {
                string query = Console.ReadLine().ToLower();
                List<string> Positive = new List<string>() { "oui", "yes", "y", "o", "true", "ouais", "yep", "yop", "t", "ye", "yeh", "yup", "ou", "oi", "oiu", "ys", "yse" };
                List<string> Negative = new List<string>() { "non", "no", "n", "false", "f", "nowtf", "on", "nn", "noo", "nooo", "noon", "nno", "flase", "fl", "fla", "flas", };
                if (Positive.Contains(query))
                {
                    returnedbool = true;
                    HasGivenGoodBool = true;
                }
                else
                {
                    if (Negative.Contains(query))
                    {
                        returnedbool = false;
                        HasGivenGoodBool = true;
                    }
                    else Utilities.RedWriteLine("[!] Are you sure this is a correct boolean? I'm really making efforts to understand you :c");
                }
            }
            return returnedbool;
        }

        #endregion

    }
}
