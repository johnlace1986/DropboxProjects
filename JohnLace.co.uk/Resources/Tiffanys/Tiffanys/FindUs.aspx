<%@ Page Title="" Language="C#" MasterPageFile="Tiffanys.master" AutoEventWireup="true" CodeFile="FindUs.aspx.cs" Inherits="Tiffanys_FindUs" %>
<%@ MasterType VirtualPath="Tiffanys.master" %>

<asp:Content ID="conFindUsHead" ContentPlaceHolderID="cphHead" Runat="Server">
    <link href="CSS/FindUs.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conFindUsBody" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="map_wrapper">
        <div class="map">
            <iframe width="425" height="349" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="http://maps.google.co.uk/maps?f=q&amp;source=s_q&amp;hl=en&amp;geocode=&amp;q=wa16+6ed&amp;sll=53.330873,-4.042969&amp;sspn=21.509954,34.013672&amp;ie=UTF8&amp;hq=&amp;hnear=Knutsford+WA16+6ED,+United+Kingdom&amp;ll=53.305506,-2.374141&amp;spn=0.004488,0.00912&amp;z=16&amp;iwloc=A&amp;output=embed"></iframe><br />
        </div>
        <div class="map_view_larger">
            <a href="http://maps.google.co.uk/maps?f=q&amp;source=embed&amp;hl=en&amp;geocode=&amp;q=wa16+6ed&amp;sll=53.330873,-4.042969&amp;sspn=21.509954,34.013672&amp;ie=UTF8&amp;hq=&amp;hnear=Knutsford+WA16+6ED,+United+Kingdom&amp;ll=53.305506,-2.374141&amp;spn=0.004488,0.00912&amp;z=16&amp;iwloc=A" target="_blank">View Larger Map</a>
        </div>
    </div>
    
    <div class="description">
        <h3>Find Us</h3>        <p>Our address is:</p>        <p>70 King Street<br />        Knutsford<br />        Cheshire<br />        WA16 6ED</p>        <p>Tiffanys is located on King Street in Knutsford, Cheshire. We are the second shop after Boots, on the left-hand side of the road (heading away from the train station).</p>        <h3>By Train</h3>        <p>If you have arrived at Knutsford on a train heading towards Manchester you will be on the platform with the ticket office. Exit through the ticket office and head right through the train station car park. Once you get to the end of the car park you will be on King Street (there will be a bridge directly above your head on the right hand side). Head left down King Street.</p>        <p>If you have arrived at Knutsford on a train heading towards Chester exit through the smaller car park. Turn left and walk down Adam's Hill and follow the road round to the left, under the bridge, onto King Street.</p>
    </div>
</asp:Content>

