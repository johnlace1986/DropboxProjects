using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;

namespace NisbetPhotography.Website
{
    public partial class ContactUs : Business.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorPageUrl = "ContentErrorPage.aspx";
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            lblSendError.Text = "";

            if (String.IsNullOrEmpty(txtName.Text))
            {
                lblSendError.Text = "Please enter your name";
                return;
            }

            if (String.IsNullOrEmpty(txtContactDetails.Text))
            {
                lblSendError.Text = "Please enter your contact details";
                return;
            }

            if (String.IsNullOrEmpty(txtEnquiry.Text))
            {
                lblSendError.Text = "Please enter your enquiry";
                return;
            }

            try
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(EnquiryEmailAddress);
                msg.From = new MailAddress(EnquiryEmailAddress);
                msg.Subject = txtName.Text + " has made an enquiry on NisbetPhotography.co.uk";
                msg.Body = GetMailMessage(txtName.Text, txtContactDetails.Text, txtEnquiry.Text);
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = SmtpSettings;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(EmailUserName, EmailPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(msg);

                lblSendError.Text = "Thank you for your enquiry. We will try and contact you as soon as possible.";
            }
            catch(Exception ex)
            {
                lblSendError.Text = "An error occured while sending the enquiry. Please try again later";
                DbObjects.Business.Error.SaveFromExceptionIncludingInnerErrors(ex);
            }
        }

        private String GetMailMessage(String name, String contactDetails, String enquiry)
        {
            String strMessage = "<html>";
            strMessage += "    <body>";
            strMessage += "        <p>Enquiry from NisbetPhotography.co.uk:</p>";
            strMessage += "        <table cellspacing=\"0\" cellpadding=\"6\" style=\"width: 100%; border: 1px solid #000000;\">";
            strMessage += "            <tr style=\"vertical-align: top;\">";
            strMessage += "                <td style=\"border: 1px solid #000000; width: 20%;\">Name:</td>";
            strMessage += "                <td style=\"border: 1px solid #000000;\">" + DbObjects.Business.Global.RemoveHtmlTags(name) + "</td>";
            strMessage += "            </tr>";
            strMessage += "            <tr style=\"vertical-align: top;\">";
            strMessage += "                <td style=\"border: 1px solid #000000; width: 10%;\">Contact Details:</td>";
            strMessage += "                <td style=\"border: 1px solid #000000;\">" + DbObjects.Business.Global.RemoveHtmlTags(contactDetails) + "</td>";
            strMessage += "            </tr>";
            strMessage += "            <tr style=\"vertical-align: top;\">";
            strMessage += "                <td style=\"border: 1px solid #000000;\">Enquiry:</td>";
            strMessage += "                <td style=\"border: 1px solid #000000;\">" + DbObjects.Business.Global.RemoveHtmlTags(enquiry) + "</td>";
            strMessage += "            </tr>";
            strMessage += "        </table>";
            strMessage += "    </body>";
            strMessage += "</html>";

            return strMessage;
        }
    }
}
