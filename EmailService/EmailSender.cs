﻿using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message message)
        {
            Console.WriteLine("Email Sender break 1");
            var mailMessage = CreateEmailMessage(message);

            Console.WriteLine("Email Sender break 6");
            await SendAsync(mailMessage);

            Console.WriteLine("Email Sender break 14");
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            Console.WriteLine("Email Sender break 2");
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            Console.WriteLine("Email Sender break 3");
            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<div><h2 style='text-align:center;'>Forgot your password?</h2><p style='text-align:center;color:gray;'>That's fine, it happens! Click on the link below to reset your password</p><p style='text-align:center;'>{0}</p></div>", message.Content) };

            Console.WriteLine("Email Sender break 4");
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            Console.WriteLine("Email Sender break 5");
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    Console.WriteLine("Email Sender break 7");
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    Console.WriteLine("Email Sender break 8");
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    Console.WriteLine("Email Sender break 9");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    Console.WriteLine("Email Sender break 10");

                    await client.SendAsync(mailMessage);
                    Console.WriteLine("Email Sender break 11");
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    Console.WriteLine("Email Sender break 12");
                    throw;
                }
                finally
                {
                    Console.WriteLine("Email Sender break 13");
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}