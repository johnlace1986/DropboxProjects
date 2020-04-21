<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NisbetPhotography.Website.Private.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <link href="../CSS/Global.css" rel="stylesheet" type="text/css" />
        <link href="CSS/Default.css" rel="stylesheet" type="text/css" />
        <title>Nisbet Photography - Private Domain</title>

        <script src="../Scripts/js/Global.js" type="text/javascript"></script>
        
        <script language="javascript" type="text/javascript">
            function btnLogIn_Click() {

                var emailAddress = document.getElementById('<%= txtEmailAddress.ClientID %>').value;
                var password = document.getElementById('<%= txtPassword.ClientID %>').value;
                var lblError = document.getElementById('<%= lblError.ClientID %>');

                lblError.innerHTML = '';

                if (emailAddress == '') {
                    lblError.innerHTML = 'Please enter your email address';
                    return false;
                }

                if (!(ValidateEmailAddress(emailAddress))) {
                    lblError.innerHTML = 'Please enter a vaild email address';
                    return false;
                }

                if (password == '') {
                    lblError.innerHTML = 'Please enter your password';
                    return false;
                }

                return true;
            }
        </script>
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <div class="gradient"></div>
            <asp:UpdatePanel ID="upnLogIn" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnLogIn" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Panel ID="pnlLogIn" runat="server" CssClass="log_in" DefaultButton="btnLogIn">
                        <h1><img src="../Images/logo.png" alt="Nisbet Photography" /></h1>
                        <p>Welcome to the Nisbet Photography customer domain. Here you can view all of the photographs we have taken for you and order copies in various sizes. To log in please enter your details into the boxes below.</p>
                        <div class="log_in_text_box_holder">
                            <div class="log_in_text_box_label">Email Address:</div>
                            <div class="log_in_text_box"><asp:TextBox ID="txtEmailAddress" runat="server" CssClass="textbox" Width="100%" /></div>
                        </div>
                        <div class="log_in_text_box_holder">
                            <div class="log_in_text_box_label">Password:</div>
                            <div class="log_in_text_box"><asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" TextMode="Password" Width="100%" /></div>
                        </div>
                        <div class="log_in_error">
                            <div class="log_in_error_label"><asp:Label ID="lblError" runat="server" /></div>
                            <div class="log_in_error_button"><asp:Button ID="btnLogIn" runat="server" Text="Log in" CssClass="button" OnClientClick="if (!(btnLogIn_Click())) return false;" OnClick="btnLogIn_Click" /></div>
                        </div>
                        <div class="return_to_main_site"><a href="../">Return to Nisbet Photogrpahy</a></div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </body>
</html>