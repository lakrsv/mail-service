using MailService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineReminders.App_Start
{
    public class MailServiceSend
    {
        //Creating a singleton of the MailService 
        //(It's not really necessary to do so, could just have StartService not static and create an instance in Program.)
        private static MailServiceSend instance;
        public static void StartService()
        {
            if (instance == null)
            {
                instance = new MailServiceSend();
            }
            instance.SendMsg();
        }

        //This method, although poorly named, has a couple of functions, the main function is to check if the current DateTime is equal or greater than the
        //specified sendtime of the message.
        private async void SendMsg()
        {
            //Create a new list of integers to store the entries IDs so that we can delete them from the DB later.
            List<int> IDtoDelete = new List<int>();
            //Set the time to the datetime it should be sent. I am setting this to DateTime.Now.Date, not including hours, minutes and seconds.
            DateTime time = DateTime.Now.Date;
            //LINQ command to get every single message from Msg.Messages
            var messages = from m in Msg.Messages select m;
            //LINQ command to modify messages to only contain messages that have a sendTime equal to the current time, or in the past.
            messages = messages.Where(m => m.SendTime <= time);

            //Generate the MailMessage, populate the list of IDs that we will delete later on and send the message to the email of the associated db entry.
            foreach (var message in messages)
            {
                IDtoDelete.Add(message.ID);
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                MailMessage msg = new MailMessage();

                msg.To.Add(new MailAddress(message.Email));
                msg.From = new MailAddress("YOUR_MAIL_ADDRESS");

                msg.Subject = "MAIL SUBJECT";

                msg.Body = string.Format(body, "TITLE", "YOUR_MAIL_EMAIL", message.Message);
                msg.IsBodyHtml = true;

                //Wait for the Task SendMail to send the message before continuing.
                await SendMail(msg);
            }
            //Finally, when all messages have been sent, send the IDs of the entries to DatabaseConnector so that it can delete these entries from the db.
            DatabaseConnector.RemoveFromDB(IDtoDelete);
        }

        private async Task SendMail(MailMessage message)
        {
            //Using SMTP, enter credentials and send the mail. (This can be done with smtp.EnableSsl = true for better security) <- RECOMMENDED.
            //You will then have to change the port of smtp.Port to the SSL equivalent.
            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "USERNAME",
                    Password = "PASSWORD"
                };

                smtp.Credentials = credential;
                smtp.Host = "SMTPSERVER";
                //smtp.EnableSsl = true;
                smtp.Port = 587; //YOUR PORT

                //Wait for the message to be sent before continuing.
                await smtp.SendMailAsync(message);
            }
        }
    }
}
