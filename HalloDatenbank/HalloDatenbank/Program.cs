using System;
using System.Data.SqlClient;

namespace HalloDatenbank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hallo Datenbank");

            var conString = "Server=(localdb)\\MSSQLLOCALDB;Database=Firma;Trusted_Connection=true;";

            try
            {
                var con = new SqlConnection(conString);
                con.Open();
                Console.WriteLine("Datenbankverbindung wurde hergestellt");

                var countCmd = new SqlCommand();
                countCmd.Connection = con;
                countCmd.CommandText = "SELECT COUNT(*) FROM Mitarbeiter";
                object obj = countCmd.ExecuteScalar();
                if (obj is int c)
                    Console.WriteLine($"Es wurden {c} Mitarbeiter in der Datenbank gefunden");


                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM Mitarbeiter";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var nachname = reader.GetString(1);
                    Console.WriteLine(nachname);

                    if (reader["Vorname"] != null)
                        Console.WriteLine($"Vorname: {reader["Vorname"]}");

                    if (reader["GebDatum"] != null)
                    {
                        var gebDatum = reader.GetDateTime(reader.GetOrdinal("GebDatum"));
                        Console.WriteLine($"GebDatum: {gebDatum:d}");
                    }
                    
                }

                con.Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Datenbankfehler: {ex.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Programmfehler: {e.Message}");
            }

            Console.WriteLine("Ende");
            Console.ReadLine();
        }
    }
}
