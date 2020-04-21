<%@ Page Language="C#" MasterPageFile="Global.Master" AutoEventWireup="true" CodeBehind="~/CurriculumVitae.aspx.cs" Inherits="Website.CurriculumVitae" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="conCurriculumnVitaeHead" ContentPlaceHolderID="cphHead" Runat="Server">
    <link href="CSS/CurriculumVitae.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="conCurriculumnVitaeBody" ContentPlaceHolderID="cphBody" Runat="Server">
    
    <h3>Curriculum Vitae</h3>
    <p>The following lists my academic and professional achievements to date. I will be updating this throughout the course of my career.</p>

    <div class="horizontal_seperator_light"></div>
                    
    <h3>Qualifications</h3>
    <p>Click any <img src="Images/pdf_icon.gif" height="15px" /> icon to view the certificate as a PDF or any <img src="Images/jpeg_icon.gif" height="15px" /> icon to view the certificate as a JPEG.</p>
    
    <div class="qualifications">
        <ul class="qualification">
            <li>
                <h3>Degree</h3>
                <p>Bachelor of Science degree with Second Class Honours in Software Engineering</p>
                <p>
                    <a href="Images/Certificates/degree.pdf" target="_blank" style="margin-right: 12px;"><img src="Images/pdf_icon.gif" height="36px" /></a>
                    <a href="Images/Certificates/degree.jpg" target="_blank"><img src="Images/jpeg_icon.gif" height="36px" /></a>
                </p>
                <p class="institution"><a href="http://www.ntu.ac.uk/" target="_blank">Nottingham Trent University</a></p>
            </li>
            <li>
                <h3>A Levels</h3>
                <p>Computing - Grade A<br />Electronics - Grade A<br />ICT - Grade B</p>
                <p class="institution"><a href="http://www.sjd.ac.uk/" target="_blank">Sir John Deane's 6th Form College</a></p>
            </li>
            <li style="margin-right: 0px;">
                <h3>GCSEs</h3>
                <p>4 x Grade A<br />6 x Grade B<br />1 x Grade C</p>
                <p class="institution"><a href="http://www.agsb.co.uk/" target="_blank">Altrincham Grammar School for Boys</a></p>
            </li>
        </ul>
    </div>

    <div class="horizontal_seperator_light"></div>
    
    <h3>Work Experience</h3>
    
    <div class="work_experience">
        <div class="work_experience_title"><h3>Application Developer for The Plumtree Group Ltd <i><a style="color: inherit;" href="http://www.plumtreegroup.com/" target="_blank">(website)</a></i></h3></div>
        <table width="100%" cellspacing="0" cellpadding="6">
            <tr>
                <td class="work_experience_label_row">Period:</td>
                <td class="work_experience_content_row" style="border-top: solid 1px #9dca5d;">September 2008 - Present</td>
            </tr>
            <tr>
                <td class="work_experience_label_row">Technologies:</td>
                <td class="work_experience_content_row"><span class="highlight">C#.NET, ASP.NET, SQL Server, WCF, WPF, Silverlight</span></td>
            </tr>
            <tr>
                <td class="work_experience_label_row">Roles:</td>
                <td class="work_experience_content_row">
                    <p>The Plumtree Group is one of the UK and Europe’s leading specialists in information management solutions for the healthcare sector. They also provide major corporate and public sector organisations with powerful, web-enabled, document management, workflow and electronic form software.</p>
                    <asp:Panel ID="pnlWorkExperiencePlumtree" runat="server">
                        <p>I joined the Plumtree Group as an industrial-placement student as part of my degree course. However, it was not long before I was being treated as a full-time member of staff. Within the first few months of my year-long placement I was being given real responsibilities and projects to develop that would be sold by the company and was offered a permanent position to return to after completing my degree.</p>
                        <p>My initial role was to develop OCR solutions using the <span class="highlight">INDICIUS Neurascript</span> package. This involved configuring the package to scan hand-written documents, verify and validate the data read from them and export it to the <i>dartEDM</i> document management system. Verification, validation and data export was done using <span class="highlight">VBScript.</span></p>
                        <p>I was then asked to take control of development of the company's flag-ship product, scanning software called <i>QScan</i>. This product was a <span class="highlight">C#.NET Windows Forms</span> application that interfaced with scanning hardware to create digital images of documents, read the values of any barcodes and export the documents to various applications.</p>
                        <p>The first project I undertook by myself was to create  a <i>PDF Append</i> application. This project was a <span class="highlight">C#.NET Class Library</span> which was used to merge serval documents into a single, bookmarked PDF file. The application also needed to be intelligent enough to know at which page within the destination the PDF file the new document would need to be inserted. When combined with the <i>QScan</i> and <i>dartEDM</i> products, <i>PDF Append</i> allowed doctors to create a single, electronic record for each patient that was easy to find and navigate through and could be updated with every patient episode.</p>
                        <p>Since returning to the Plumtree Group after completing my degree I have been part of the <i>DartV3</i> team. This is a huge project span over several years during which every product that the Plumtree Group sells will be rebranded and rewritten as a <i>DartV3 Module</i>. All modules will be able to interact with each other enabling systems to be upgraded by simply replacing single DLL files.</p>
                        <p>During my time at the Plumtree Group I have been directly involved with and spent some time developing every product that the company sells. The majority of these are <span class="highlight">ASP.NET web applications</span> with <span class="highlight">C#.NET code-behind</span> that interact with <span class="highlight">SQL Server 2000, 2008 and 2010 databases</span>, as well as <span class="highlight">Windows Services</span>, <span class="highlight">WCF and WPF projects</span> and <span class="highlight">Silverlight applications</span>.</p>
                    </asp:Panel>
                    <div style="float: right; padding: 0px 12px; font-style: italic; font-weight: normal;"><asp:LinkButton ID="lblWorkExperiencePlumtree" runat="server" /></div>
                </td>
            </tr>
        </table>
    </div>
    
    <ajax:CollapsiblePanelExtender ID="cpeWorkExperiencePlumtree" runat="server"
        ExpandControlID="lblWorkExperiencePlumtree"
        ExpandedText="...less"
        CollapseControlID="lblWorkExperiencePlumtree"
        CollapsedText="more..."
        TextLabelID="lblWorkExperiencePlumtree"
        TargetControlID="pnlWorkExperiencePlumtree"
        SuppressPostBack="true"
        Collapsed="true" />
    
</asp:Content>

