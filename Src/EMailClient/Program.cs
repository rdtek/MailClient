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
            string smtpHost = "";

            //string smtpHost = "smtp.office365.com";
            var config = AppConfigManager.Load();
            var cmdArgs = new CmdLineArgs(args);

            //Save config items if user has specified them
            if (cmdArgs.Exists(0) && cmdArgs.Get(0).ToUpper().StartsWith("CONFIG"))
            {
                if(cmdArgs.Exists("Host")) {
                    config.Host = cmdArgs.Get("Host");
                    if(config.Host.Length >= 3) AppConfigManager.Save(config);
                }

                if (cmdArgs.Exists("FromAddress")) {
                    config.FromEmailAddress = cmdArgs.Get("FromAddress");
                    if (config.FromEmailAddress.Length >= 3) AppConfigManager.Save(config);
                }
                return;
            }

            if (config.Host != null && config.Host.Length >= 3)
                smtpHost = config.Host;

            if (config.FromEmailAddress != null && config.FromEmailAddress.Length >= 3)
                mailAddressFrom = config.FromEmailAddress;

            //Get TO address
            if (cmdArgs.Exists("/to")) { mailAddressTo = cmdArgs.Get("/to"); }
            else { Console.WriteLine("No TO address"); return; }

            //Get BODY
            if (cmdArgs.Exists("/body")) { mailBody = cmdArgs.Get("/body"); }
            else { Console.WriteLine("No BODY"); return; }

            if(mailAddressFrom.Length >= 3 
                && mailAddressTo.Length >= 3 
                && mailBody.Length >= 1)
            {
                Console.WriteLine("Enter password: ");
                //Hide inputted password chars
                password = PasswordReader.ReadPassword();
            }

            SendEmail(smtpHost, mailAddressFrom, mailAddressTo, mailBody, password);
        }

        private static void SendEmail(string smtpHost, string addressFrom, 
            string addressTo, string body, string password)
        {
            var mail = new MailMessage(addressFrom, addressTo);
            mail.IsBodyHtml = true;
            mail.Subject = "An email from my mail client";
            mail.Body = "<html><body><p>" + body + "</p></body></html>";

            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Host: " + smtpHost);
            Console.WriteLine("From: " + addressFrom);
            Console.WriteLine("To: " + addressTo);
            Console.WriteLine("Body: " + body);
            Console.WriteLine("----------------------------------------------------");

            var client = new SmtpClient
            {
                Host = smtpHost,
                Port = 587,
                EnableSsl = true
            };

            // Important: This line of code must be executed before setting the NetworkCredentials 
            // object, otherwise the setting will be reset (a bug in .NET)
            client.UseDefaultCredentials = false;

            var cred = new NetworkCredential(addressFrom, password);
            client.Credentials = cred;

            Console.WriteLine("Sending mail...");

            try
            {
                client.Send(mail);
                Console.WriteLine("Mail sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
