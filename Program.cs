//A simple example mailing service that checks if it should send mail from an SQL database every X seconds based on a delay
//specified in Program.cs.

//It contains detailed step-by-step comments on what the program actually does, and how it works.

//What was it used for?<

//I used this simple little code to send mail from a website I made. 
//This website let's you register with an e-mail address and then lets you create entries into a database. 
//The fields you can submit are a Message and a Send Time.

//So the website functions like a basic little reminder's website, 
//you write a message and tell the website when to send it to you. 
//This mailservice, running seperately, 
//connects to the database at repeating intervals to check for messages and checks if the message should be sent. 
//If these conditions are met, it sends the message and deletes the entry from the database.

using OnlineReminders.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MailService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Mail service...");
            //Run the method that starts the timer.
            RunService();
            Console.WriteLine("Press ENTER to stop the mailservice.");
            //Stop the program from shutting down, so that the mailservice can run uninterrupted.
            Console.ReadLine();
        }
        static void RunService()
        {
            //Delay between each database check (MS)
            Timer timer = new Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Run the Method SendMail, which takes care of connection to the database, getting the DbSet from the rows.
            SendMail();
            Console.WriteLine("Checking Reminders...");
            Console.WriteLine("Press ENTER to stop the mailservice.");

        }

        private static void SendMail()
        {
            //Connect to DB and populate Msg.Messages with the entries that should be mailed.
            DatabaseConnector.ConnectToDB();
            //Check if mail should be sent.
            MailServiceSend.StartService();
        }
    }
}
