using System;
using System.Text;
using BLL.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace BLL.Services
{
    public class EmailService
    {
        IConfiguration config;
        ILogger<EmailService> logger;
        string emailFolder;

        public EmailService(IConfiguration config, ILogger<EmailService> logger, IHostEnvironment env)
        {
            this.config = config;
            this.logger = logger;
            emailFolder = System.IO.Path.Combine(env.ContentRootPath, "EmailOutbox");
        }

        public void SendBookedBooking(string userEmail, BookingDTO booking)
        {
            string subject = "Aabaas BD - Booking Confirmed (Booked)";
            string body = BuildBody("Thank you for your payment. Your hotel is booked.", booking);
            Send(userEmail, subject, body);
        }

        public void SendCancelledBooking(string userEmail, BookingDTO booking)
        {
            string subject = "Aabaas BD - Booking Cancelled";
            string body = BuildBody("Your booking has been cancelled.", booking);
            Send(userEmail, subject, body);
        }

        private string BuildBody(string message, BookingDTO booking)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Dear Guest,");
            sb.AppendLine();
            sb.AppendLine(message);
            sb.AppendLine();
            sb.AppendLine("Hotel: " + booking.HotelName);
            sb.AppendLine("City: " + booking.HotelCity);
            sb.AppendLine("Check-in: " + booking.CheckIn.ToString("dd MMM yyyy"));
            sb.AppendLine("Check-out: " + booking.CheckOut.ToString("dd MMM yyyy"));
            sb.AppendLine("Total Cost: BDT " + booking.TotalCost.ToString("N2"));
            sb.AppendLine("Status: " + booking.Status);
            sb.AppendLine();
            sb.AppendLine("Thank you for choosing Aabaas BD.");
            return sb.ToString();
        }

        private void Send(string userEmail, string subject, string body)
        {
            SaveToFile(userEmail, subject, body);

            string enabledText = config["Smtp:Enabled"];
            bool enabled = enabledText != null && enabledText.ToLower() == "true";

            string host = config["Smtp:Host"] ?? "";
            string user = config["Smtp:User"] ?? "";
            string password = config["Smtp:Password"] ?? "";
            string from = config["Smtp:From"] ?? user;

            password = password.Replace(" ", "");

            if(password.Contains("PUT_YOUR", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning("SMTP password not set. Email saved to file only for {Email}", userEmail);
                return;
            }

            if(!enabled || host.Length == 0 || user.Length == 0 || password.Length == 0)
            {
                logger.LogWarning("SMTP is off or not configured. Email saved to file for {Email}", userEmail);
                return;
            }

            int port = 587;
            string portText = config["Smtp:Port"];
            if (portText != null && portText.Length > 0)
            {
                int.TryParse(portText, out port);
            }

            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress("Aabaas BD", from));
                message.To.Add(MailboxAddress.Parse(userEmail));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using SmtpClient client = new SmtpClient();
                client.Connect(host, port, SecureSocketOptions.StartTls);
                client.Authenticate(user, password);
                client.Send(message);
                client.Disconnect(true);

                logger.LogInformation("Email sent to {Email}", userEmail);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "SMTP failed for {Email}. Copy saved in EmailOutbox.", userEmail);
            }
        }

        private void SaveToFile(string userEmail, string subject, string body)
        {
            try
            {
                if (!System.IO.Directory.Exists(emailFolder))
                {
                    System.IO.Directory.CreateDirectory(emailFolder);
                }

                string safeEmail = userEmail.Replace("@", "_at_").Replace(".", "_");
                string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + safeEmail + ".txt";
                string path = System.IO.Path.Combine(emailFolder, fileName);

                StringBuilder file = new StringBuilder();
                file.AppendLine("To: " + userEmail);
                file.AppendLine("Subject: " + subject);
                file.AppendLine();
                file.AppendLine(body);

                System.IO.File.WriteAllText(path, file.ToString());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Could not save email file.");
            }
        }
    }
}
