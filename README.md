# MailService

A simple example mailing service that checks if it should send mail from an SQL database every X seconds based on a delay
specified in Program.cs.

It contains detailed step-by-step comments on what the program actually does, and how it works.

<b>What was it used for?</b>

I used this simple little code to send mail from a website I made. This website let's you register with an e-mail address and then lets you create entries into a database. The fields you can submit are a Message and a Send Time.

So the website functions like a basic little reminder's website, you write a message and tell the website when to send it to you. This mailservice, running seperately, connects to the database at repeating intervals to check for messages and checks if the message should be sent. If these conditions are met, it sends the message and deletes the entry from the database.
