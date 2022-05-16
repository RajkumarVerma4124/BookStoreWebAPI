using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    /// <summary>
    /// Created The Class For Microsoft Messaging Queueing
    /// </summary>
    public class Msmq
    {
        //Created The Object Reference For MessageQueue
        MessageQueue messageQueue = new MessageQueue();
        private string recieverEmailAddr;
        private string recieverName;

        //Method To Send Token Using MessageQueue And Delegate
        public void SendMessage(string token, string emailId, string name)
        {
            recieverEmailAddr = emailId;
            recieverName = name;
            messageQueue.Path = @".\Private$\Token";
            try
            {
                if (!MessageQueue.Exists(messageQueue.Path))
                {
                    MessageQueue.Create(messageQueue.Path);
                }
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
                messageQueue.Send(token);
                messageQueue.BeginReceive();
                messageQueue.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Delegate To Send Token As Message To The Sender EmailId Using Smtp And MailMessage
        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = messageQueue.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("iamrajvermapro@gmail.com", "QwertyKey@123"),
                };
                mailMessage.From = new MailAddress("vermar4124@gmail.com");
                mailMessage.To.Add(new MailAddress(recieverEmailAddr));
                string mailBody = $"<!DOCTYPE html>" +
                                  $"<html>" +
                                  $" <style>" +
                                  $".blink" +
                                  $"</style>" +
                                    $"<body style = \"background-color:#DBFF73;text-align:center;padding:5px;\">" +
                                    $"<h1 style = \"color:#6A8D02; border-bottom: 3px solid #84AF08; margin-top: 5px;\"> Dear <b>{recieverName}</b> </h1>\n" +
                                    $"<h3 style = \"color:#8AB411;\"> For Resetting Password The Below Link Is Issued</h3>" +
                                    $"<h3 style = \"color:#8AB411;\"> Please Click The Link Below To Reset Your Password</h3>" +
                                    $"<p style = \"color:#00802b; text-decoration: none; font-size:15px;\">{token}</p>\n" +
                                    $"<h3 style = \"color:#8AB411;margin-bottom:5px;\"><blink>This Token Will be Valid For Next 6 Hours<blink></h3>" +
                                    $"</body>" +
                                    $"</html>";

                mailMessage.Body = mailBody;
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Fundoo Notes Password Reset Link";
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
