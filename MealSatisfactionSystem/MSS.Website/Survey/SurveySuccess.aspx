<%@ Page Title="Survey Completion" Language="C#" MasterPageFile="~/SurveyPortal.master" AutoEventWireup="true" CodeFile="SurveySuccess.aspx.cs" Inherits="Survey_SurveySuccess" ValidateRequest="false" %>
<asp:Content ID="SurveySuccess" ContentPlaceHolderID="SurveySuccess" Runat="Server">
    <div class="SurveySuccess"><%--this div defines class for entire page useful for styling--%>
        <div class="container" >
        <div class="jumbotron text-center">   
            <asp:Image ID="Logo" runat="server" ImageUrl="~/Images/Covenant-Health-Logo.png" CssClass="img-responsive center-block" AlternateText="Covenant Health Logo"/>
            <div class="text">
            <h1>Meal Service Satisfaction Survey</h1>
            <p>Thank you for your feedback!</p>
            <p>If you would like to speak to someone regarding the meal(s) you had during your stay, contact us at (555)-555-5555.</p>
            </div>
        </div>
    </div>
    </div>
</asp:Content>

