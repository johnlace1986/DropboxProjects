<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="PublicAlbums.aspx.cs" Inherits="NisbetPhotography.Website.Private.Admin.PublicAlbums" %>
<%@ MasterType VirtualPath="Admin.Master" %>

<asp:Content ID="conPublicAlbumsHead" ContentPlaceHolderID="cphAdminHead" runat="server">
    <link href="CSS/PublicAlbums.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conPublicAlbumsBody" ContentPlaceHolderID="cphAdminBody" runat="server">

    <script type="text/javascript" language="javascript">
        function open_public_album(publicAlbumId) {
            PageMethods.SetSelectedPublicAlbumFromJS(publicAlbumId);
            window.location = 'PublicAlbum.aspx';
        }
    </script>

    <asp:Panel ID="pnlPublicAlbums" runat="server" CssClass="public_albums">
        <asp:Repeater ID="rptPublicAlbums" runat="server" OnItemDataBound="rptPublicAlbums_ItemDataBound">
            <ItemTemplate>
                <div class="public_album_link_button" onclick="open_public_album(<%# DataBinder.Eval(Container.DataItem, "Id").ToString() %>);">
                    <div class="public_album">
                        <table class="public_album_image_wrapper" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="vertical-align: middle;"><div class="category_image_frame"><asp:Image ID="imgPublicAlbum" runat="server" ImageUrl='<%# PublicImageFolderUrl + DataBinder.Eval(Container.DataItem, "Thumbnail.ImageUrl") %>' class="public_album_image" /></div></td>
                            </tr>
                        </table>
                        <div class="public_album_title"><%# DataBinder.Eval(Container.DataItem, "Name") %></div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <% if (rptPublicAlbums.Items.Count == 0)
           { %>
            <p>No public albums have been added to the website.</p>
        <% } %>
    </asp:Panel>
    <div class="options"><asp:Button ID="btnAddAlbum" runat="server" Text="Add Album" CssClass="option_button" OnClick="btnAddAlbum_Click" /></div>
</asp:Content>
