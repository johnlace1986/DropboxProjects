using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace JohnLace
{
    public partial class Global : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSendEnquiry_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";

                string strFrom = txtEmailAddress.Text;
                string strTo = ConfigurationManager.AppSettings["EmailTo"];
                string strSubject = "Your JohnLace.co.uk enquiry";
                string strMessage = GetMailMessage(txtName.Text, txtEnquiry.Text);
                string strHost = ConfigurationManager.AppSettings["EmailSmtpSetting"];
                string strUser = ConfigurationManager.AppSettings["EmailUserName"];
                string strPassword = DecryptEmailPassword(ConfigurationManager.AppSettings["EmailPassword"]);

                MailMessage msg = new MailMessage();
                msg.To.Add(strTo);
                msg.From = new MailAddress(strFrom);
                msg.Subject = strSubject;
                msg.Body = strMessage;
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = strHost;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(strUser, strPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                client.Send(msg);

                lblError.Text = "Thank you for your enquiry. I will try and contact you as soon as I can";
            }
            catch
            {
                lblError.Text = "An error occured while sending the enquiry. Please try again later";
            }

            SetPanelVisibility(pnlEnquiryErrorPopup, true);
        }

        private string GetMailMessage(string Name, string Enquiry)
        {
            string strMessage = "<html>";
            strMessage += "    <body>";
            strMessage += "        <p>Enquiry from JohnLace.co.uk:</p>";
            strMessage += "        <table cellspacing=\"0\" cellpadding=\"6\" style=\"width: 100%; border: 1px solid #000000;\">";
            strMessage += "            <tr style=\"vertical-align: top;\">";
            strMessage += "                <td style=\"border: 1px solid #000000; width: 10%;\">Name:</td>";
            strMessage += "                <td style=\"border: 1px solid #000000;\">" + Name + "</td>";
            strMessage += "            </tr>";
            strMessage += "            <tr style=\"vertical-align: top;\">";
            strMessage += "                <td style=\"border: 1px solid #000000;\">Enquiry:</td>";
            strMessage += "                <td style=\"border: 1px solid #000000;\">" + Enquiry.Replace("\n", "<br />") + "</td>";
            strMessage += "            </tr>";
            strMessage += "        </table>";
            strMessage += "    </body>";
            strMessage += "</html>";

            return strMessage;
        }

        private string DecryptEmailPassword(string encryptedPassword)
        {
            byte[] toEncryptArray = Convert.FromBase64String(encryptedPassword);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("Manchester City FC"));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private void SetPanelVisibility(Panel panel, bool visible)
        {
            if (visible)
                panel.Style["visibility"] = "visible";
            else
                panel.Style["visibility"] = "hidden";
        }
    }
}