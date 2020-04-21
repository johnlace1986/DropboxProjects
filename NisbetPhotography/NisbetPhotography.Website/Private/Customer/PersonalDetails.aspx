<%@ Page Language="C#" MasterPageFile="Customer.Master" AutoEventWireup="true" CodeBehind="PersonalDetails.aspx.cs" Inherits="NisbetPhotography.Website.Private.Customer.PersonalDetails" %>
<%@ MasterType VirtualPath="Customer.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="conPersonalDetailsHead" ContentPlaceHolderID="cphCustomerHead" runat="server">
    <link href="CSS/PersonalDetails.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conPersonalDetailsBody" ContentPlaceHolderID="cphCustomerBody" runat="server">
    <asp:Panel ID="pnlPersonalDetails" runat="server" DefaultButton="btnSaveChanges">
        <div class="fields">
            <div class="field">
                <div class="field_title">Account Number:</div>
                <div class="field_label"><asp:Label ID="lblUserId" runat="server" /></div>
            </div>
            <div class="field">
                <div class="field_title">Account Holder Since:</div>
                <div class="field_label"><asp:Label ID="lblDateCreated" runat="server" /></div>
            </div>
            <div class="field">
                <div class="field_title">First Name:</div>
                <div class="field_textbox"><asp:TextBox ID="txtFirstName" runat="server" CssClass="textbox" /></div>
            </div>
            <div class="field">
                <div class="field_title">Surname:</div>
                <div class="field_textbox"><asp:TextBox ID="txtSurname" runat="server" CssClass="textbox" /></div>
            </div>
            <div class="field">
                <div class="field_title">Email Address:</div>
                <div class="field_textbox"><asp:TextBox ID="txtEmailAddress" runat="server" CssClass="textbox" /></div>
            </div>
        </div>
        
        <div class="options">
            <ul class="option_buttons">
                <li><input type="button" value="Cancel" class="option_button" onclick="window.location = 'Default.aspx';" /></li>
                <li><asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="option_button" /></li>
                <li><asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CssClass="option_button" OnClick="btnSaveChanges_Click" /></li>
                <li><asp:Label ID="lblConfirmation" runat="server" CssClass="confirmation_label" /></li>
            </ul>
        </div>
    </asp:Panel>
    
    <asp:Panel ID="pnlErrorPopup" runat="server" CssClass="popup" style="display: none;">
        <p style="text-align: center;"><asp:Label ID="lblError" runat="server" /></p>
        <p style="text-align: center;"><asp:Button ID="btnErrorOk" runat="server" Text="OK" CssClass="option_button" /></p>
    </asp:Panel>

    <asp:Panel ID="pnlChangePasswordPopup" runat="server" CssClass="popup" style="display: none;" DefaultButton="btnChangePasswordOk">
        <div class="fields">
            <div class="field">
                <div class="field_title">Current Password:</div>
                <div class="field_textbox"><asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="textbox" TextMode="Password" /></div>
            </div>
            <div class="field">
                <div class="field_title">New Password:</div>
                <div class="field_textbox"><asp:TextBox ID="txtNewPassword" runat="server" CssClass="textbox" TextMode="Password" /></div>
            </div>
            <div class="field">
                <div class="field_title">Confirm Password:</div>
                <div class="field_textbox"><asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="textbox" TextMode="Password" /></div>
            </div>
        </div>
        <p style="text-align: center;"><asp:Label ID="lblChangePasswordError" runat="server" /></p>
        <ul class="option_buttons">
            <li><asp:Button ID="btnChangePasswordCanel" runat="server" Text="Cancel" CssClass="option_button" /></li>
            <li><asp:Button ID="btnChangePasswordOk" runat="server" Text="OK" CssClass="option_button" OnClick="btnChangePasswordOk_Click" /></li>
        </ul>
    </asp:Panel>

    <!-- invisible link button for error popup -->
    <asp:LinkButton ID="lnkErrorPopup" runat="server" style="visibility: hidden;" />

    <ajax:ModalPopupExtender ID="mpeError" runat="server" TargetControlID="lnkErrorPopup" PopupControlID="pnlErrorPopup" />
    <ajax:ModalPopupExtender ID="mpeChangePassword" runat="server" TargetControlID="btnChangePassword" PopupControlID="pnlChangePasswordPopup" />
</asp:Content>
