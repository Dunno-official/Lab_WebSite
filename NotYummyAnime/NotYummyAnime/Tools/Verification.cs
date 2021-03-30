using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotYummyAnime.Tools
{
    public static class Verification
    {
        public static string GenerateHash()
        {
            Random random = new Random();
            string randomBase = "";

            for (int i = 0; i < 10; ++i)
                randomBase += (char)random.Next(0, 128);

            return random.GetHashCode().ToString();
        }


        public static void SendVerificationMessage(string id , string hash, string email = "denisbondarbarbar@gmail.com")
        {
            var fromAddress = new MailAddress("denisbondarbarbar@gmail.com");
           string fromPassword = "someWierdPassword";
            var toAddress = new MailAddress(email);

            string subject = "Account verification";
            string body = string.Format("<h1><a style=\"color: rgb(0, 155, 0)\" href =\"" +
                "https://localhost:44314/Account/Verification?id={0}&hash={1}\">Account verification</a></h1>", id , hash);

            SmtpClient smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

            smtp.Send(message);
        }
    }
}
