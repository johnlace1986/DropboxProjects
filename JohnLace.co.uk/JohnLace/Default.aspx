<%@ Page Language="C#" MasterPageFile="Global.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JohnLace.Default" %>

<asp:Content ID="cphDefaultHead" ContentPlaceHolderID="cphHead" runat="server">
    <link href="CSS/Default.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="cphDefaultBody" ContentPlaceHolderID="cphBody" Runat="Server">

    <div class="default_content">
        <div class="welcome">
            <div class="left_hand_side">
                <h3>Welcome</h3>
                <p>Welcome to JohnLace.co.uk. I am a <span class="highlight">.NET software developer and website designer</span> based primarily in Nottingham; however I also commute regularly to Manchester.</p>
                <p>Here you can view my CV and browse my online portfolio to see the type of work I undertake. I have vast experience designing all kinds of solutions from Windwos Forms and WPF applications to ASP.NET websites and Silverlight Controls.</p>
                <p>If you like the look of any of the work I have done and would like to contact me regarding any software development or website design work you have then please fill in the form below. I am always interested to hear about new opportunities.</p>
            </div>

            <div class="right_hand_side">
                <div class="highlighted_box">
                    <h3>Technical Skills</h3>
                    <p>Here are some of the technologies I work with:</p>
                    <p style="padding-bottom: 0px;"><b>.NET 4.0, C#.NET, WPF, Visual Basic, C++, Silverlight, WCF, ASP.NET, HTML, JavaScript, CSS, MS SQL Server, MySQL, Oracle</b></p>
                </div>
                <br />
                <div class="highlighted_box">
                    <h3>Past Projects</h3>
                    <ul>
                        <li>Electronic Point-Of-Sale (EPOS) applications</li>
                        <li>Social networking websites</li>
                        <li>Online shops</li>
                        <li>Electronic document scanning and management</li>
                        <li>Barcode scanning</li>
                        <li>Complex image and PDF manipulation</li>
                    </ul>
                </div>
            </div>
        </div>
        <div style="padding: 12px 0px;"><div class="horizontal_seperator_light"></div></div>
    
        <div class="latest_work">
            <div class="latest_work_description">
                <h3>Latest Work</h3>
                <p>Nisbet Photgraphy are a husband and wife team based in Peterborough, UK who have recently decided to extend their photography professionally as they have, over many months of planning, come to the conclusion that if they work hard and dedicate theirselves, they could develop a business providing the fun and enjoyment of photography to others, that they themselves enjoy.</p>
                <p style="padding: 18px 0px;"><a href="http://www.nisbetphotography.co.uk" target="_blank">View website...</a></p>
            </div>
            <div class="latest_work_screenshot">
                <div id="slider">
                    <img src="Images/ProjectScreenshots/nisbet_photography001.png" />
                    <img src="Images/ProjectScreenshots/nisbet_photography002.png" />
                    <img src="Images/ProjectScreenshots/nisbet_photography003.png" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>