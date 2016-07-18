using System;
using System.Net;
using System.Net.Mail;

namespace MailClient
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

            var cmdArgs = new CmdLineArgs(args);

            //Get FROM address
            if(cmdArgs.Exists(0)) { mailAddressFrom = cmdArgs.Get(0); }
            else { Console.WriteLine("No FROM address"); return; }

            //Get TO address
            if (cmdArgs.Exists(1)) { mailAddressTo = cmdArgs.Get(1); }
            else { Console.WriteLine("No TO address"); return; }

            //Get BODY
            if (cmdArgs.Exists(2)) { mailBody = cmdArgs.Get(2); }
            else { Console.WriteLine("No BODY"); return; }

            if(mailAddressFrom.Length >= 3 
                && mailAddressTo.Length >= 3 
                && mailBody.Length >= 1)
            {
                Console.WriteLine("Enter password: ");
                //Hide inputted password chars
                password = PasswordReader.ReadPassword();
            }

            var mail = new MailMessage(mailAddressFrom, mailAddressTo);
            mail.IsBodyHtml = true;
            mail.Subject = "An email from Office365";
            mail.Body = "<html><body><p>" + mailBody + "</p></body></html>";
            
            var client = new SmtpClient {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true
            };
            
            // Important: This line of code must be executed before setting the NetworkCredentials 
            // object, otherwise the setting will be reset (a bug in .NET)
            client.UseDefaultCredentials = false; 

            var cred = new NetworkCredential(mailAddressFrom, password);
            client.Credentials = cred;

            Console.WriteLine("Sending mail...");
            client.Send(mail);
            Console.WriteLine("Mail sent.");
        }
    }
}
