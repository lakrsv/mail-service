using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace MailService
{
    class DatabaseConnector
    {
        public static void ConnectToDB()
        {
            //Clear the static list Msg.Messages and populate it again later with new entries.
            Msg.Messages.Clear();
            //Connect to the SQL database and open the connection.
            var connection = new SqlConnection("YOUR CONNECTION STRING");
            connection.Open();

            //Select a row(*) from your table.
            using (SqlCommand command = new SqlCommand("SELECT * FROM YOUR_TABLE", connection))
            {
                //Arbitrary
                //if(command == null) { return; }

                //Using an SqlDataReader we read each and every row from the table.
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Create a temporary list to hold an array of every single field in our row.
                        List<object> tempObj = new List<object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            //For each field in each row, add this value to our temporary object.
                            var value = reader.GetValue(i);
                            tempObj.Add(value);
                        }

                        //Create an instance of our DbSet replica and populate it with our values.
                        ReminderModel model = new ReminderModel();

                        //In my case, my first field in the row is the ID of the entry, second is email, third is message and the fourth is sendTime.
                        model.ID = (int)tempObj[0];
                        model.Email = (string)tempObj[1];
                        model.Message = (string)tempObj[2];
                        model.SendTime = (DateTime)tempObj[3];

                        //Add the DbSet replica to my List of replicas.
                        Msg.Messages.Add(model);
                    }
                }
                //Finally, close the connection.
                connection.Close();
            }
        }
        //In my case, whenever a message is sent I remove that specific message from the database. I identify the entry with it's ID.
        //This method is run from the MailServiceSend class.
        public static void RemoveFromDB(List<int> ids)
        {
            //Again, create a new connection with your connectionstring and open it.
            var connection = new SqlConnection("YOUR CONNECTION STRING");
            connection.Open();
            //Create an SqlDataReader
            SqlDataReader reader;
            foreach (int id in ids)
            {
                //And then, for every single id in ids, run the sqlcommand to delete that row from the table.
                SqlCommand command = new SqlCommand("DELETE FROM YOUR_TABLE WHERE ID=" + "'"+id+"'", connection);
                reader = command.ExecuteReader();
                //Finally, close the reader so that it can be re-used in the foreach loop.
                reader.Close();
            }

        }
    }
}
