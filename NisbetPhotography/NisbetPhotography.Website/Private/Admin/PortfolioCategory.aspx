<%@ Page Language="C#" MasterPageFile="Admin.Master" AutoEventWireup="true" CodeBehind="PortfolioCategory.aspx.cs" Inherits="NisbetPhotography.Website.Private.Admin.PortfolioCategory" %>
<%@ MasterType VirtualPath="Admin.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="conPortfolioCategoryHead" ContentPlaceHolderID="cphAdminHead" runat="server">
    <link href="CSS/PortfolioCategory.css" rel="stylesheet" type="text/css" />       

    <script type="text/javascript" src="../Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</asp:Content>

<asp:Content ID="conPortfolioCategoryBody" ContentPlaceHolderID="cphAdminBody" runat="server">
    <asp:Panel ID="pnlPortfolioCategoryOptions" runat="server" DefaultButton="btnSaveChanges">
        <div class="name">
            <div class="name_label">Name:</div>
            <div class="name_textbox"><asp:TextBox ID="txtName" runat="server" CssClass="textbox" /></div>
        </div>
        
        <div class="options">
            <ul class="option_buttons">
                <li><input type="button" value="Cancel" class="option_button" onclick="window.location = 'Portfolio.aspx';" /></li>
                <% if (SelectedPortfolioCategory.IsInDatabase)
                    { %>
                    <li><asp:Button ID="btnAddPictures" runat="server" Text="Add Pictures" CssClass="option_button" /></li>
                    <li><asp:Button ID="btnDeleteCategory" runat="server" Text="Delete Category" CssClass="option_button" OnClick="btnDeleteCategory_Click" /></li>
                <% } %>
                <li><asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" CssClass="option_button" OnClick="btnSaveChanges_Click" /></li>
            </ul>
        </div>
    </asp:Panel>
    
    <% if (SelectedPortfolioCategory.IsInDatabase)
       { %>
        <asp:Panel ID="pnlImages" runat="server" CssClass="images">
            <asp:Repeater ID="rptImages" runat="server" OnItemDataBound="rptImages_ItemDataBound">
                <ItemTemplate>
                    <div class="image">
                        <table class="image_wrapper" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><img src="<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>" class="image_actual" /></td>
                            </tr>
                        </table>
                        <div class="image_thumbnail"><asp:RadioButton ID="rdoThumbnail" runat="server" Text="Thumbnail" GroupName="Category" Checked='<%# (Boolean)DataBinder.Eval(Container.DataItem, "Thumbnail") %>' portfolioImageId='<%# DataBinder.Eval(Container.DataItem, "Id").ToString() %>' /></div>
                        <div class="image_delete_button"><asp:Button ID="btnDeleteImage" runat="server" Text="Delete Image" CssClass="option_button" OnClick="btnDeleteImage_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id").ToString() %>' /></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        
            <% if (rptImages.Items.Count == 0)
               { %>
                <div class="no_pictures">No pictures have been added to this category.</div>
            <% } %>
        </asp:Panel>
    <% } %>

    <asp:Panel ID="pnlUploadPicture" runat="server" style="display: none; background-color: transparent;">
        <div id="silverlightControlHost">
            <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="350px" height="200px">
		      <param name="source" value="../../ClientBin/NisbetPhotography.MultipleImageUploader.xap"/>
		      <param name="onError" value="onSilverlightError" />
		      <param name="background" value="white" />
		      <param name="minRuntimeVersion" value="4.0.50826.0" />
		      <param name="autoUpgrade" value="true" />
              <asp:Literal ID="ltrInitParams" runat="server" />
		      <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0" style="text-decoration:none">
 			      <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		      </a>
	        </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlErrorPopup" runat="server" CssClass="popup" style="display: none;">
        <p style="text-align: center;"><asp:Label ID="lblError" runat="server" /></p>
        <p class="error_label"><asp:Button ID="btnErrorOk" runat="server" Text="OK" CssClass="option_button" /></p>
    </asp:Panel>
    
    <!-- invisible link button for error popup -->
    <asp:LinkButton ID="lnkErrorPopup" runat="server" style="visibility: hidden;" />

    <% if (SelectedPortfolioCategory.IsInDatabase)
       { %>
        <ajax:ModalPopupExtender ID="mpeUploadPicture" runat="server" TargetControlID="btnAddPictures" PopupControlID="pnlUploadPicture" />
    <% } %>

    <ajax:ModalPopupExtender ID="mpeError" runat="server" TargetControlID="lnkErrorPopup" PopupControlID="pnlErrorPopup" />
</asp:Content>
