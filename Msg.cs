using System;
using System.Collections.Generic;

namespace MailService
{
    public class ReminderModel
    {
        public int ID { get; set; }

        public string Email { get; set; }
        public string Message { get; set; }

        //DateTime in the format of DateTime.Date, contains information on when the mail should be sent.
        public DateTime SendTime { get; set; }
    }
    public class Msg
    {
        //Contains a list of all the messages in the database, messages in my case are DB entries with ID, Email, Message and SendTime fields.
        //ReminderModel simply replicates the object I am storing in my SQL database.
        public static List<ReminderModel> Messages = new List<ReminderModel>();
    }
}
