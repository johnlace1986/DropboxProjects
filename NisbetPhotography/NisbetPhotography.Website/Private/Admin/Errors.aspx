<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="Errors.aspx.cs" Inherits="NisbetPhotography.Website.Private.Admin.Errors" %>
<%@ MasterType VirtualPath="Admin.Master" %>

<asp:Content ID="conErrorsHead" ContentPlaceHolderID="cphAdminHead" runat="server">
    <link href="CSS/Errors.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conErrorsBody" ContentPlaceHolderID="cphAdminBody" runat="server">
        
    <div style="padding-right: 12px;">
        <asp:GridView ID="dgError" runat="server"
            AllowPaging="true"
            AutoGenerateColumns="false"
            CellPadding="6"
            OnPageIndexChanging="dgError_PageIndexChanging"
            PageSize="10"
            CssClass="grid_view"
            OnRowCommand="dgError_RowCommand"
            >
            <HeaderStyle HorizontalAlign="Center" BackColor="#3d3d3d" ForeColor="#ffffff" BorderColor="#000000" />
            <RowStyle BackColor="#BBBBBB" VerticalAlign="Top" ForeColor="#000000" BorderColor="#000000" />
            <AlternatingRowStyle BackColor="#d5d5d5" ForeColor="#000000" BorderColor="#000000" />
            <PagerStyle HorizontalAlign="Center" BackColor="#3d3d3d" ForeColor="#ffffff" BorderColor="#000000" />        
            <Columns>            
                <asp:ButtonField ButtonType="Link" DataTextField="Name" HeaderText="Name" CommandName="dgError_RowClicked" />
                <asp:ButtonField ButtonType="Link" DataTextField="MessageSubstring" HeaderText="Message" CommandName="dgError_RowClicked">
                    <ItemStyle Wrap="false" />
                </asp:ButtonField>
                <asp:BoundField DataField="DateThrown" HeaderText="Date Thrown" DataFormatString="{0:ddd dd/MM/yyyy HH:mm}">
                    <ItemStyle Width="132px" HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>            
        </asp:GridView>
        
        <% if (dgError.Rows.Count == 0)
           { %>
            <p>No new errors have occured in the website.</p>
        <% } %>
    </div>
    
</asp:Content>
