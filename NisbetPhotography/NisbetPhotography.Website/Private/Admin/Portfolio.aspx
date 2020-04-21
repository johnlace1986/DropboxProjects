<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="Portfolio.aspx.cs" Inherits="NisbetPhotography.Website.Private.Admin.Portfolio" %>
<%@ MasterType VirtualPath="Admin.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="conPortfolioHead" ContentPlaceHolderID="cphAdminHead" runat="server">
    <link href="CSS/Portfolio.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conPortfolioBody" ContentPlaceHolderID="cphAdminBody" runat="server">
    
    <script type="text/javascript" language="javascript">
        function open_category(portfolioCategoryId) {
            PageMethods.SetSelectedPortfolioCategoryFromJS(portfolioCategoryId);
            window.location = 'PortfolioCategory.aspx';
        }
    </script>

    <asp:Panel ID="pnlCategories" runat="server" CssClass="categories">
        <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
            <ItemTemplate>
                <div class="category_link_button" onclick="open_category(<%# DataBinder.Eval(Container.DataItem, "Id").ToString() %>);">
                    <div class="category">
                        <table class="category_image_wrapper" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="vertical-align: middle;"><div class="category_image_frame"><asp:Image ID="imgCategory" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Thumbnail.ImageUrl") %>' class="category_image" /></div></td>
                            </tr>
                        </table>
                        <div class="category_title"><%# DataBinder.Eval(Container.DataItem, "Name") %></div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <% if (rptCategories.Items.Count == 0)
           { %>
            <p>No portfolio categories have been added to the website.</p>
        <% } %>
    </asp:Panel>
    <div class="options"><asp:Button ID="btnAddCategory" runat="server" Text="Add Cagegory" CssClass="option_button" OnClick="btnAddCategory_Click" /></div>
</asp:Content>
