<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="NisbetPhotography.Website.Private.Admin.Customer" %>
<%@ MasterType VirtualPath="Admin.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="conCustomerHead" ContentPlaceHolderID="cphAdminHead" runat="server">
    <link href="CSS/Customer.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conCustomerBody" ContentPlaceHolderID="cphAdminBody" runat="server">
    
    <script type="text/javascript" language="javascript">
        function open_customer_album(customerAlbumId) {
            PageMethods.SetSelectedCustomerAlbumFromJS(customerAlbumId, '<%= SelectedCustomer.UserId.ToString() %>');
            window.location = 'CustomerAlbum.aspx';
        }
    </script>

    <asp:Panel ID="pnlCustomer" runat="server" DefaultButton="btnSaveChanges">
        <div class="fields">
            <% if (SelectedCustomer.IsInDatabase)
               { %>
                <div class="field">
                    <div class="field_title">First Name:</div>
                    <div class="field_label"><%= SelectedCustomer.FirstName %></div>
                </div>
                <div class="field">
                    <div class="field_title">Surname:</div>
                    <div class="field_label"><%= SelectedCustomer.Surname %></div>
                </div>
                <div class="field">
                    <div class="field_title">Email Address:</div>
                    <div class="field_label"><%= SelectedCustomer.EmailAddress %></div>
                </div>
            <%}
               else
               { %>
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
                <div class="field">
                    <div class="field_title">Password:</div>
                    <div class="field_textbox"><asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" TextMode="Password" /></div>
                </div>
                <div class="field">
                    <div class="field_title">Confirm Password:</div>
                    <div class="field_textbox"><asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="textbox" TextMode="Password" /></div>
                </div>
            <% } %>
        </div>
        
        <div class="options">
            <ul class="option_buttons">
                <li><input type="button" value="Cancel" class="option_button" onclick="window.location = 'Customers.aspx';" /></li>
                <% if (SelectedCustomer.IsInDatabase)
                    { %>
                    <li><asp:Button ID="btnAddAblum" runat="server" Text="Add Album" CssClass="option_button" OnClick="btnAddAlbum_Click" /></li>
                    <li><asp:Button ID="btnDeleteCustomer" runat="server" Text="Delete Customer" CssClass="option_button" OnClick="btnDeleteCustomer_Click" /></li>
                <% }
                    else
                    { %>
                    <li><asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CssClass="option_button" OnClick="btnSaveChanges_Click" /></li>
                <% } %>
            </ul>
        </div>
        
        <% if (SelectedCustomer.IsInDatabase)
           { %>
            <asp:Panel ID="pnlAlbums" runat="server" CssClass="albums">
                <asp:Repeater ID="rptAlbums" runat="server" OnItemDataBound="rptAlbums_ItemDataBound">
                    <ItemTemplate>
                        <div class="album_link_button" onclick="open_customer_album(<%# DataBinder.Eval(Container.DataItem, "Id").ToString() %>);">
                            <div class="album">
                                <table class="album_image_wrapper" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: middle;"><div class="category_image_frame"><asp:Image ID="imgAlbum" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Thumbnail.ImageUrl") %>' class="album_image" /></div></td>
                                    </tr>
                                </table>
                                <div class="album_title"><%# DataBinder.Eval(Container.DataItem, "Name") %></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <% if (rptAlbums.Items.Count == 0)
                   { %>
                    <p>No albums have been created for this customer.</p>
                <% } %>
            </asp:Panel>
        <% } %>
    </asp:Panel>
    
    <asp:Panel ID="pnlErrorPopup" runat="server" CssClass="popup" style="display: none;">
        <p style="text-align: center;"><asp:Label ID="lblError" runat="server" /></p>
        <p class="error_label"><asp:Button ID="btnErrorOk" runat="server" Text="OK" CssClass="option_button" /></p>
    </asp:Panel>

    <!-- invisible link button for error popup -->
    <asp:LinkButton ID="lnkErrorPopup" runat="server" style="visibility: hidden;" />

    <ajax:ModalPopupExtender ID="mpeError" runat="server" TargetControlID="lnkErrorPopup" PopupControlID="pnlErrorPopup" />
</asp:Content>
