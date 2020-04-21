<%@ Page Language="C#" MasterPageFile="Public.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NisbetPhotography.Website.Public.Default" %>

<asp:Content ID="conDefaultHead" ContentPlaceHolderID="cphPublicHead" runat="server">
    <link href="CSS/Default.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conDefaultBody" ContentPlaceHolderID="cphPublicBody" runat="server">
    <asp:GridView ID="dgPublicAlbums" runat="server"
        AllowPaging="true"
        AutoGenerateColumns="false"
        CellPadding="6"
        OnPageIndexChanging="dgPublicAlbums_PageIndexChanging"
        PageSize="10"
        CssClass="grid_view"
        OnRowCommand="dgPublicAlbums_RowCommand"
        >
        <HeaderStyle HorizontalAlign="Center" BackColor="#3d3d3d" ForeColor="#ffffff" BorderColor="#000000" />
        <RowStyle BackColor="#BBBBBB" VerticalAlign="Top" ForeColor="#000000" BorderColor="#000000" />
        <AlternatingRowStyle BackColor="#d5d5d5" ForeColor="#000000" BorderColor="#000000" />
        <PagerStyle HorizontalAlign="Center" BackColor="#3d3d3d" ForeColor="#ffffff" BorderColor="#000000" />              
        <Columns>            
            <asp:ButtonField ButtonType="Link" DataTextField="Name" HeaderText="Name" CommandName="dgPublicAlbums_RowClicked" />
            <asp:ButtonField ButtonType="Link" DataTextField="Description" HeaderText="Description" CommandName="dgPublicAlbums_RowClicked" />
            <asp:BoundField DataField="ImageCount" HeaderText="Pictures">    
                <ItemStyle HorizontalAlign="Right" Width="75px" />
            </asp:BoundField>
            <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:ddd dd/MM/yyyy HH:mm}">
                <ItemStyle HorizontalAlign="Center" Width="200px" />
            </asp:BoundField>
        </Columns>            
    </asp:GridView>
    
    <% if (dgPublicAlbums.Rows.Count == 0)
       { %>
        <p>No public albums have been created.</p>
    <% } %>
</asp:Content>
