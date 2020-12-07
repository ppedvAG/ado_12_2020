using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HalloDatenbank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hallo Datenbank");

            var conString = "Server=(localdb)\\MSSQLLOCALDB;Database=NORTHWND;Trusted_Connection=true;";

            try
            {
                using (var con = new SqlConnection(conString))
                {
                    con.Open();
                    Console.WriteLine("Datenbankverbindung wurde hergestellt");

                    var countCmd = new SqlCommand();
                    countCmd.Connection = con;
                    countCmd.CommandText = "SELECT COUNT(*) FROM Employees";
                    object obj = countCmd.ExecuteScalar();
                    if (obj is int c)
                        Console.WriteLine($"Es wurden {c} Mitarbeiter in der Datenbank gefunden");


                    Console.WriteLine("Suche [Enter für alle]:");
                    var suche = Console.ReadLine();

                    var cmd = con.CreateCommand();
                    //BÖSE !!!! cmd.CommandText = "SELECT * FROM Employees WHERE Firstname LIKE '" + suche + "%'";
                    cmd.CommandText = "SELECT * FROM Employees WHERE Firstname LIKE @search+'%'";
                    cmd.Parameters.AddWithValue("@search", suche);


                    var mitarbeiter = new List<Mitarbeiter>();

                    using (SqlDataReader reader = cmd.ExecuteReader()) //IDisoseable
                    {
                        while (reader.Read())
                        {
                            var m = new Mitarbeiter()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                                Nachname = reader.GetString(1)
                            };

                            if (reader["Firstname"] != null)
                                m.Vorname = reader["Firstname"].ToString();

                            if (reader["BirthDate"] != null)
                                m.GebDatum = reader.GetDateTime(reader.GetOrdinal("BirthDate"));

                            m.NewGebDatum = m.GebDatum.AddYears(1);

                            Console.WriteLine($"[{m.Id}] {m.Vorname} {m.Nachname} ({m.GebDatum:d} --> {m.NewGebDatum:d})");

                            mitarbeiter.Add(m);

                        }
                    }// reader.Dispose(); //--> reader.Close();

                    using (var trans = con.BeginTransaction())
                    {
                        //UPDATE command
                        foreach (var m in mitarbeiter)
                        {
                            var updateCmd = con.CreateCommand();
                            updateCmd.Transaction = trans;
                            updateCmd.CommandText = "UPDATE Employees SET BirthDate = @newBDate WHERE EmployeeID = @id";
                            updateCmd.Parameters.AddWithValue("@id", m.Id);
                            updateCmd.Parameters.AddWithValue("@newBDate", m.NewGebDatum);

                            //if (m.Nachname.StartsWith("B"))
                            //    throw new System.IO.FileNotFoundException();

                            int affectedRows = updateCmd.ExecuteNonQuery();
                            if (affectedRows == 1) Console.WriteLine("OK");
                            else if (affectedRows == 0) Console.WriteLine("Fehler, nichts wurde geändert");
                            else Console.WriteLine($"PANIK! Es wurden {affectedRows} Rows geändert");
                        }

                        trans.Commit();
                    }//default: trans.Rollback();
                }// con.Close();

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

    class Mitarbeiter
    {
        public int Id { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime GebDatum { get; set; }
        public DateTime NewGebDatum { get; set; }
    }

}
