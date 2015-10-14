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
