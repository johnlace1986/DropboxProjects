<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="NisbetPhotography.Website.Private.Admin.Customers" %>
<%@ MasterType VirtualPath="Admin.Master" %>

<asp:Content ID="conCustomersHead" ContentPlaceHolderID="cphAdminHead" runat="server">
    <link href="CSS/Customers.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conCustomersBody" ContentPlaceHolderID="cphAdminBody" runat="server">

    <div style="padding-right: 12px;">
        <asp:GridView ID="dgCustomer" runat="server"
            AllowPaging="true"
            AutoGenerateColumns="false"
            CellPadding="6"
            OnPageIndexChanging="dgCustomer_PageIndexChanging"
            PageSize="10"
            CssClass="grid_view"
            OnRowCommand="dgCustomer_RowCommand"            
            >
            <HeaderStyle HorizontalAlign="Center" BackColor="#3d3d3d" ForeColor="#ffffff" BorderColor="#000000" />
            <RowStyle BackColor="#BBBBBB" VerticalAlign="Top" ForeColor="#000000" BorderColor="#000000" />
            <AlternatingRowStyle BackColor="#d5d5d5" ForeColor="#000000" BorderColor="#000000" />
            <PagerStyle HorizontalAlign="Center" BackColor="#3d3d3d" ForeColor="#ffffff" BorderColor="#000000" />            
            <Columns>            
                <asp:ButtonField ButtonType="Link" DataTextField="FullName" HeaderText="Name" CommandName="dgCustomer_RowClicked" />
                <asp:ButtonField ButtonType="Link" DataTextField="EmailAddress" HeaderText="Email Address" CommandName="dgCustomer_RowClicked" />
                <asp:ButtonField ButtonType="Link" DataTextField="DateAdded" HeaderText="Date Added" CommandName="dgCustomer_RowClicked" DataTextFormatString="{0:ddd dd/MM/yyyy HH:mm}">
                    <ItemStyle Width="132px" HorizontalAlign="Center" />
                </asp:ButtonField>
            </Columns>
        </asp:GridView>
        
        <% if (dgCustomer.Rows.Count == 0)
           { %>
            <p>No customers have been added to the database.</p>
        <% } %>
    </div>
    <div class="options"><asp:Button ID="btnAddCustomer" runat="server" Text="Add Customer" CssClass="option_button" OnClick="btnAddCustomer_Click" /></div>
    
</asp:Content>
