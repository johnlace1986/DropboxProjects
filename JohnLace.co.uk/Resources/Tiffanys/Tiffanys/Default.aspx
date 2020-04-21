<%@ Page Language="C#" MasterPageFile="Tiffanys.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Tiffanys_Default" %>
<%@ MasterType VirtualPath="Tiffanys.master" %>

<asp:Content ID="conDefaultHead" ContentPlaceHolderID="cphHead" Runat="Server">
    <link href="CSS/Default.css" rel="stylesheet" type="text/css" />
    <link href="CSS/nivo-slider.css" rel="stylesheet" type="text/css" />
    <link href="CSS/custom-nivo-slider.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.nivo.slider.pack.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(window).load(function() {
            $('#slider').nivoSlider({
                effect: 'fade',
                pauseTime: 5000,
                animSpeed: 1000
            });
        });
    </script>
</asp:Content>

<asp:Content ID="conDefaultBody" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="folded_corner_boxes">
        <div class="folded_corner_box">
            <h3><a href="GreetingsCards.aspx">Greetings Cards</a></h3>
            <table width="100%" cellpadding="3">
                <tr>
                    <td rowspan="2" class="folded_corner_box_image_cell"><img src="Images/greetings_cards.png" class="folded_corner_box_image" /></td>
                    <td>Whatever the occasion, you will be sure to find the perfect greetings card at Tiffanys. We stock thousands of cards from all the major retailers inculuding Hallmark, Ling Design, Carte Blanche and many more.</td>
                </tr>
                <tr>
                    <td style="text-align: right;"><a href="GreetingsCards.aspx">Learn more...</a></td>
                </tr>
            </table>
        </div>
        
        <div class="folded_corner_box">
            <h3><a href="ThorntonsChocolates.aspx">Thorntons Chocolates</a></h3>
            <table width="100%" cellpadding="3">
                <tr>
                    <td rowspan="2" class="folded_corner_box_image_cell"><img src="Images/thorntons_chocolates.png" class="folded_corner_box_image" /></td>
                    <td>We stock a full range of Thorntons chocolates, including gift-wrapping and personalised message icing services. Perfect for treating a loved one or simply satisfying your sweet tooth.</td>
                </tr>
                <tr>
                    <td style="text-align: right;"><a href="ThorntonsChocolates.aspx">Learn more...</a></td>
                </tr>
            </table>
        </div>
        
        <div class="folded_corner_box">
            <h3><a href="HeliumBalloons.aspx">Helium Balloons</a></h3>
            <table width="100%" cellpadding="3">
                <tr>
                    <td rowspan="2" class="folded_corner_box_image_cell"><img src="Images/helium_balloons.png" class="folded_corner_box_image" /></td>
                    <td>Our helium baloons light up any room. Sold singly or grouped together to make the perfect decoration for a function room, our extensive range has every celebration covered.</td>
                </tr>
                <tr>
                    <td style="text-align: right;"><a href="HeliumBalloons.aspx">Learn more...</a></td>
                </tr>
            </table>
        </div>
        
    </div>
    
    <div class="description">
        <p>Tiffanys Card and Gift Shop is the number one private seller of greetings cards, gifts, party favours and decorations in Knutsford and the surrounding Cheshire area. We off a huge range of products at highly competitive prices. Whatever the occasion, you’ll be sure to find what you’re looking for at Tiffanys.</p>        <p>Situated on Knutsford’s busiest high street, King Street, Tiffanys has become the first point of call for thousands of shoppers from all over Cheshire and beyond. Whether you’re treating that special someone, organising a party or simply after a newspaper or magazine to read while enjoying a snack, Tiffanys has it covered.</p>
        <div id="slider">
	        <img src="Images/slider/slide1.jpg" alt="" height="250px" />
	        <img src="Images/slider/slide2.jpg" alt="" height="250px" />
	        <img src="Images/slider/slide3.jpg" alt="" height="250px" />
	        <img src="Images/slider/slide4.jpg" alt="" height="250px" />      
        </div>
        <p>Our product range is second to none. Here is just a small sample:</p>        <ul>            <li>Greetings cards</li>            <li>Soft toys and other gift ideas</li>            <li>Thorntons chocolates</li>            <li>Party favours</li>            <li>Helium balloons</li>            <li>Yankee candles</li>            <li>Newspapers and magazines</li>
        </ul>
    </div>
</asp:Content>

