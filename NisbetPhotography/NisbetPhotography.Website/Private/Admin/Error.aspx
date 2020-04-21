<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="NisbetPhotography.Website.Private.Admin.Error" %>
<%@ MasterType VirtualPath="Admin.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="conErrorHead" ContentPlaceHolderID="cphAdminHead" runat="server">
    <link href="CSS/Error.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conErrorBody" ContentPlaceHolderID="cphAdminBody" runat="server">
    <div class="page_wrapper">
        <h1><asp:Label ID="lblName" runat="server" /></h1>
        
        <p style="border-bottom: solid 1px #3e3d3d; padding-bottom: 12px; margin-bottom: 24px;"><asp:Label ID="lblMessage" runat="server" /></p>
        
        <h2><asp:LinkButton ID="lnkInnerErrors" runat="server" Text="Inner Errors" /></h2>
        <div class="inner_errors">
            <asp:Panel ID="pnlInnerErrors" runat="server">
                <asp:Repeater ID="rptInnerErrors" runat="server">
                    <ItemTemplate>                        
                        <div class="inner_error">
                            <h3><%# DataBinder.Eval(Container.DataItem, "Name") %></h3>
                            <p><%# DataBinder.Eval(Container.DataItem, "Message") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <% if (rptInnerErrors.Items.Count == 0)
                   { %>
                    <p>No inner errors</p>
                <% } %>
            </asp:Panel>            
        </div>
        
        <h2><asp:LinkButton ID="lnkStackTrace" runat="server" Text="Stack Trace" /></h2>
        <div class="stack_trace">
            <asp:Panel ID="pnlStackTrace" runat="server">
                <ul><li><asp:Label ID="lblStackTrace" runat="server" /></li></ul>
            </asp:Panel>
        </div>
        
        <div class="options"><asp:Button ID="btnDeleteError" runat="server" Text="Delete" CssClass="option_button" OnClick="btnDeleteError_Click" /></div>
        
    </div>
    
    <ajax:CollapsiblePanelExtender ID="cpeStackTrace" runat="server"
        TargetControlID="pnlStackTrace"
        CollapseControlID="lnkStackTrace"
        ExpandControlID="lnkStackTrace"
        Collapsed="true"
        SuppressPostBack="true"
    />
    
    <ajax:CollapsiblePanelExtender ID="cpeInnerErrors" runat="server"
        TargetControlID="pnlInnerErrors"
        CollapseControlID="lnkInnerErrors"
        ExpandControlID="lnkInnerErrors"
        Collapsed="true"
        SuppressPostBack="true"
    />
</asp:Content>
