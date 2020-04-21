<%@ Page Title="Nisbet Photography - Portfolio" Language="C#" MasterPageFile="NisbetPhotographyMaster.Master" AutoEventWireup="true" CodeBehind="Portfolio.aspx.cs" Inherits="NisbetPhotography.Website.Portfolio" %>

<asp:Content ID="conPortfolioHead" ContentPlaceHolderID="cphNisbetPhotographyMasterHead" Runat="Server">
    <link href="CSS/Portfolio.css" rel="stylesheet" type="text/css" />
    
    <!-- galleria slideshow -->
    <script src="Scripts/js/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/js/highlight.pack.js" type="text/javascript"></script>
    <script src="Scripts/js/galleria.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        function open_category(portfolioCategoryId) {
            PageMethods.SetSelectedPortfolioCategoryFromJS(portfolioCategoryId);
            window.location = 'Portfolio.aspx';
        }
    </script>
</asp:Content>

<asp:Content ID="conPortfolioBody" ContentPlaceHolderID="cphNisbetPhotographyMasterBody" Runat="Server">
        
    <table class="page_wrapper">
        <tr>
            <td class="categories" style="vertical-align: top;">
        
                <h1>Albums:</h1>
                
                <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
                    <ItemTemplate>
                        <div class="category_link_button" onclick="open_category(<%# DataBinder.Eval(Container.DataItem, "Id").ToString() %>);">
                            <div class="category">
                                <div class="category_button_inner">
                                    <div class="category_image_holder"><asp:Image ID="imgCategory" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Thumbnail.ImageUrl") %>' class="category_image" /></div>
                                    <div class="category_title"><%# DataBinder.Eval(Container.DataItem, "Name") %></div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <% if (rptCategories.Items.Count == 0)
                   { %>
                    <p>No portfolio categories have been added to the website.</p>
                <% } %>
                
            </td>
            
            <td class="vertical_seperator"></td>
            
            <td class="gallery" style="vertical-align: top;">
            
                <asp:Panel ID="pnlSelectedCategory" runat="server">
        
                    <h1 style="text-align: center; font-style: italic;"><asp:Label ID="lblAlbumTitle" runat="server" /></h1>
                            
                    <div id="gallery">
                        <asp:Repeater ID="rptGallery" runat="server">
                            <ItemTemplate>
                                <img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                        
                    <% if (rptGallery.Items.Count == 0)
                       { %>
                       <p>No pictures have been added to this category.</p>
                    <% } %>
                    
                </asp:Panel>
            </td>
        </tr>
    </table>
    
    <script>
        Galleria.loadTheme('Scripts/js/galleria.classic.js');
        $('#gallery').galleria({
           height: 400
       });
    </script>
</asp:Content>

