<%@ Page Language="C#" MasterPageFile="Public.Master" AutoEventWireup="true" CodeBehind="PublicAlbum.aspx.cs" Inherits="NisbetPhotography.Website.Public.PublicAlbum" %>

<asp:Content ID="conPublicAlbumHead" ContentPlaceHolderID="cphPublicHead" runat="server">
    <!-- galleria slideshow -->
    <script src="../Scripts/js/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/highlight.pack.js" type="text/javascript"></script>
    <script src="../Scripts/js/galleria.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="conPublicAlbumBody" ContentPlaceHolderID="cphPublicBody" runat="server">
    <h1 style="text-align: center; font-style: italic;"><asp:Label ID="lblAlbumTitle" runat="server" /></h1>
                <div id="gallery">
        <asp:Repeater ID="rptGallery" runat="server">
            <ItemTemplate>
                <img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" alt="<%# DataBinder.Eval(Container.DataItem, "Caption") %>" />
            </ItemTemplate>
        </asp:Repeater>
    </div>
        
    <% if (rptGallery.Items.Count == 0)
       { %>
       <p>No pictures have been added to this album.</p>
    <% } %>
    
    <script>
        Galleria.loadTheme('../Scripts/js/galleria.classic.js');
        $('#gallery').galleria({
           height: 500
       });
    </script>
</asp:Content>
