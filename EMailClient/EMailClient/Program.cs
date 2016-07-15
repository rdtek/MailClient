using System;
using System.Net;
using System.Net.Mail;

namespace EMailClient
{
    //Code adapted from:
    //https://medium.com/developer-developers-developers/send-emails-through-office365-exchange-online-using-net-69b3a4a3b236#.t1vromdkg

    class Program
    {
        static void Main(string[] args)
        {
            string mailAddressFrom = "";
            string mailAddressTo = "";
            string mailBody = "";
            string password = "";

            //Get FROM address
            if (args != null && args.Length >= 1) {
                mailAddressFrom = args[0];
            } else {
                //No FROM address
                Console.WriteLine("No FROM address");
                return;
            }

            //Get TO address
            if (args != null && args.Length >= 2) {
                mailAddressTo = args[1];
            } else {
                //No TO address
                Console.WriteLine("No TO address");
                return;
            }

            //Get BODY
            if (args != null && args.Length >= 3) {
                mailBody = args[2];
            } else {
                //No BODY
                Console.WriteLine("No BODY");
                return;
            }

            if(mailAddressFrom.Length >= 3 && mailAddressTo.Length >= 3 && mailBody.Length >= 1)
            {
                Console.WriteLine("Enter password: ");
                //TODO: hide inputted password chars
                password = Console.ReadLine();
            }

            MailMessage mail = new MailMessage(mailAddressFrom, mailAddressTo);
            mail.IsBodyHtml = true;
            mail.Subject = "An email from Office365";
            mail.Body = "<html><body><p>" + mailBody + "</p></body></html>";
            
            SmtpClient client = new SmtpClient("smtp.office365.com");
            client.Port = 587;
            client.EnableSsl = true;

            // Important: This line of code must be executed before setting the NetworkCredentials 
            // object, otherwise the setting will be reset (a bug in .NET)
            client.UseDefaultCredentials = false; 

            NetworkCredential cred = new NetworkCredential(mailAddressFrom, password);
            client.Credentials = cred;

            Console.WriteLine("Sending mail...");
            client.Send(mail);

            Console.WriteLine("Mail sent.");
            Console.ReadLine();
        }
    }
}
