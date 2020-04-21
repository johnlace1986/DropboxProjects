<%@ Page Title="Nisbet Photography - Contact Us" Language="C#" MasterPageFile="NisbetPhotographyMaster.Master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="NisbetPhotography.Website.ContactUs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="conContactUsHead" ContentPlaceHolderID="cphNisbetPhotographyMasterHead" Runat="Server">

    <link href="CSS/ContactUs.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript">

        function btnSend_Click() {

            var name = document.getElementById('<%= txtName.ClientID %>').value;
            var contactDetails = document.getElementById('<%= txtContactDetails.ClientID %>').value;
            var enquiry = document.getElementById('<%= txtEnquiry.ClientID %>').value;
            var lblError = document.getElementById('<%= lblSendError.ClientID %>');

            lblError.innerHTML = '';

            if ((name == '') || (name == '<%= tweName.WatermarkText %>')) {
                lblError.innerHTML = 'Please enter your name';
                return false;
            }

            if ((contactDetails == '') || (contactDetails == '<%= tweContactDetails.WatermarkText %>')) {
                lblError.innerHTML = 'Please enter your contact details';
                return false;
            }

            if ((enquiry == '') || (enquiry == '<%= tweEnquiry.WatermarkText %>')) {
                lblError.innerHTML = 'Please enter your enquiry';
                return false;
            }

            return true;
        }
    
    </script>
    
</asp:Content>

<asp:Content ID="conContactUsBody" ContentPlaceHolderID="cphNisbetPhotographyMasterBody" Runat="Server">

    <div class="contact_us">
        <asp:UpdatePanel ID="udpContactUs" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                    <h1>Contact Us</h1>
                
                    <div class="description">
                        <p>If you would like to get in contact with us regarding any details of the site or would like to discuss any photography requirements you may have, then please fill in this form and click "Send".  We will get back to you as soon as we can.</p>
                        <p>Alternatively you can email us at:</p>
                        <p style="text-align: center; font-style: italic;"><a href="mailto:<%= EnquiryEmailAddress %>"><%= EnquiryEmailAddress %></a></p>
                        <p>or call us on: 07543 854 331</p>
                    </div>
                
                    <div class="user_form">
                        <p><asp:TextBox ID="txtName" runat="server" CssClass="textbox" /></p>
                        <p><asp:TextBox ID="txtContactDetails" runat="server" CssClass="textbox" /></p>
                        <p><asp:TextBox ID="txtEnquiry" runat="server" TextMode="MultiLine" CssClass="textbox_multiline" /></p>
                    </div>
            
                    <div class="send">
                        <div class="error_label"><asp:Label ID="lblSendError" runat="server" /></div>
                        <div class="send_button"><asp:Button ID="btnSend" runat="server" Text="Send" CssClass="button" OnClientClick="if (!(btnSend_Click())) return false;" OnClick="btnSend_Click" /></div>
                    </div>

                <ajax:TextBoxWatermarkExtender ID="tweName" runat="server" TargetControlID="txtName" WatermarkText="Name" WatermarkCssClass="watermark" />
                <ajax:TextBoxWatermarkExtender ID="tweContactDetails" runat="server" TargetControlID="txtContactDetails" WatermarkText="Contact Details" WatermarkCssClass="watermark" />
                <ajax:TextBoxWatermarkExtender ID="tweEnquiry" runat="server" TargetControlID="txtEnquiry" WatermarkText="Enquiry" WatermarkCssClass="watermark_multiline" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        
</asp:Content>

