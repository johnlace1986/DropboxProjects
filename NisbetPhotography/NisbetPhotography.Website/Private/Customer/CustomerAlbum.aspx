<%@ Page Language="C#" MasterPageFile="Customer.Master" AutoEventWireup="true" CodeBehind="CustomerAlbum.aspx.cs" Inherits="NisbetPhotography.Website.Private.Customer.CustomerAlbum" %>

<asp:Content ID="conCustomerAlbumHead" ContentPlaceHolderID="cphCustomerHead" runat="server">
    <link href="CSS/CustomerAlbum.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conCustomerAlbumBody" ContentPlaceHolderID="cphCustomerBody" runat="server">
    <div class="title">
        <h1><asp:Label ID="lblName" runat="server" /></h1>
        <p><i>Created on <asp:Label ID="lblDateCreated" runat="server" /></i></p>
    </div>

    <div>
        <ul class="images">
            <asp:Repeater ID="rptImages" runat="server">
                <ItemTemplate>
                    <li>
                        <table class="image_wrapper" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="text-align: center; vertical-align: middle;">
                                    <a href="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" target="_blank""><img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" class="image_actual" /></a>
                                </td>
                            </tr>
                        </table>
                        <div class="image_caption"><a href="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" target="_blank""><%# DataBinder.Eval(Container.DataItem, "Name") %></a></div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</asp:Content>
