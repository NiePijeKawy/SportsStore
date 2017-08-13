using SportsStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;
using System.Net;
using System.Net.Mail;


namespace SportsStore.Domain.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor (EmailSettings settings)
	    {
            emailSettings = settings;
	    }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            //using (var smtpClient = new SmtpClient())
            //{
            //    smtpClient.EnableSsl = emailSettings.UseSsl;
            //    smtpClient.Host = emailSettings.ServerName;
            //    smtpClient.Port = emailSettings.ServerPort;
            //    smtpClient.UseDefaultCredentials = false;
            //    smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);

            //    if (emailSettings.WriteAsFile)
            //    {
            //        smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            //        smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
            //        smtpClient.EnableSsl = false;
            //    }

            //    StringBuilder body = new StringBuilder()
            //        .AppendLine("Nowe zamówienie")
            //        .AppendLine("----")
            //        .AppendLine("Produkty");

            //    foreach (var line in cart.Lines)
            //    {
            //        var subtotal = line.Product.Price * line.Quantity;

            //        body.AppendFormat("{0} x {1} (wartość: {2:c}",
            //            line.Quantity,
            //            line.Product.Name,
            //            subtotal);

            //        //  body.AppendFormat("{0} x {1} (wartość: {2:c}", line.Quantity,
            //        //line.Product.Name,
            //        //subtotal);
            //    }

            //    body.AppendFormat("Wartość całkowita: {0:c}", cart.ComputeTotalValue())
            //        .AppendLine("---")
            //        .AppendLine("Wysyłka dla:")
            //        .AppendLine(shippingDetails.Name)
            //        .AppendLine(shippingDetails.Line1)
            //        .AppendLine(shippingDetails.Line2 ?? "")
            //        .AppendLine(shippingDetails.Line3 ?? "")
            //        .AppendLine(shippingDetails.City)
            //        .AppendLine(shippingDetails.State ?? "")
            //        .AppendLine(shippingDetails.Country)
            //        .AppendLine(shippingDetails.Zip)
            //        .AppendLine("---")
            //        .AppendFormat("Pakowanie prezentu : {0}", shippingDetails.GiftWrap ? "Tak" : "Nie");

            //    MailMessage mailMessage = new MailMessage(
            //        emailSettings.MailFromAddress,
            //        emailSettings.MailToAddress,
            //        "Otrzymano nowe zamówienie",
            //        body.ToString());

            //    if (emailSettings.WriteAsFile)
            //    {
            //        mailMessage.BodyEncoding = Encoding.ASCII;
            //    }

            //    smtpClient.Send(mailMessage);


            //}



            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.wp.pl";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("karol9891@wp.pl", "karol89");

            //MailMessage mm = new MailMessage("karol9891@wp.pl", shippingDetails.Email, "test", "test");//"petrykowskikarol@gmail.com"
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            StringBuilder body = new StringBuilder()
                .AppendLine("Nowe zamówienie")
                .AppendLine("----")
                .AppendLine("Produkty");

            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quantity;

                body.AppendFormat("{0} x {1} (wartość: {2:c}",
                    line.Quantity,
                    line.Product.Name,
                    subtotal);                
            }

            body.AppendFormat("Wartość całkowita: {0:c}", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine("Wysyłka dla:")
                .AppendLine(shippingDetails.Name)
                .AppendLine(shippingDetails.Line1)
                .AppendLine(shippingDetails.Line2 ?? "")
                .AppendLine(shippingDetails.Line3 ?? "")
                .AppendLine(shippingDetails.City)
                .AppendLine(shippingDetails.State ?? "")
                .AppendLine(shippingDetails.Country)
                .AppendLine(shippingDetails.Zip)
                .AppendLine("---")
                .AppendFormat("Pakowanie prezentu : {0}", shippingDetails.GiftWrap ? "Tak" : "Nie");

            MailMessage mailMessage = new MailMessage(
                emailSettings.MailFromAddress,
                shippingDetails.Email,
                "Otrzymano nowe zamówienie",
                body.ToString());

            client.Send(mailMessage);//mm
        }
    }
}
