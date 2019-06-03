<%@ Page Title="Survey Log In" Language="C#" MasterPageFile="~/SurveyPortal.master" AutoEventWireup="true" CodeFile="SurveyAccess.aspx.cs" ValidateRequest="false" Inherits="SurveyAccess" %>
<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<asp:Content ID="SurveyAccess" ContentPlaceHolderID="SurveyAccess" Runat="Server">
   <div class="SurveyAccess"><%--this div defines class for entire page useful for styling--%>
    <div class="container">
        <div class="jumbotron">
            <asp:Image ID="Logo" runat="server" ImageUrl="~/Images/Covenant-Health-Logo.png" CssClass="img-responsive center-block" AlternateText="Covenant Health Logo"/>
            <div class="text">
            <h1>Meal Service Satisfaction Survey</h1>
            <p>We would like to learn more about your meal experience while you have been in the hospital. Your responses will help us improve and ensure the quality of food services.</p>
            <p><strong>Individual responses will be kept anonymous and confidential.</strong></p>
            <a href="https://www.covenanthealth.ca/privacy-statement" class="btn btn-link" target="_blank">Privacy Statement &raquo;</a>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-md-push-3">
                <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

                <asp:Panel ID="SurveyPanel" runat="server" CssClass="panel panel-default" DefaultButton="SubmitButton">
                    <div class="panel-body">
                        <div class="form-group">
                            <asp:Label ID="SurveyPasscodeLabel" runat="server" Text="Survey Passcode" AssociatedControlID="SurveyPasscode" Font-Bold="false"/>
                            <asp:TextBox ID="SurveyPasscode" Autocomplete="off" AutoPostBack="false" runat="server" CssClass="form-control"/>
                            <asp:Button ID="NeedHelpButton" runat="server" Text="Need Help?" CssClass="btn btn-link"/>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
                                    <AjaxToolKit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="SurveyPopup" TargetControlID="NeedHelpButton"                CancelControlID="BackButton" BackgroundCssClass="shadow">
                                    </AjaxToolKit:ModalPopupExtender>

                                    <!-- Popup that appears when Needhelp is clicked. -->
                                    <asp:Panel ID="SurveyPopup" runat="server" CssClass="survey-popup" Style="display: none">
                                        <div class="survey-popup-header panel-heading">
                                            <h3>Help Info</h3>
                                        </div>
                                        <div class="survey-popup-body panel-body">
                                           <div class="popup-section">
                                            <p>Can't find where the passcode is?</p>
                                            <ul>  
                                                <li>For patients, the daily passcode is found at the bottom part of your tray ticket</li>
                                                <li>For non-patients, the passcode can be found found at the bottom part of the receipt that is received from the caffeteria.</li>
                                            </ul>
                                            <p>Did you lose your tray ticket or receipt? Contact us at (555)-555-5555 and ask what the passcode of the day is.</p>
                                               </div>
                                            <div class="popup-section">
                                                    <img src="../Images/Survey-popupimage.png" alt="Need help example" />
                                            </div>
                                          
                                              <asp:Button ID="BackButton" runat="server" Text="Back" CssClass="btn btn-danger" />
                                                
                                        </div>
                                    </asp:Panel>
        <div class="row">
            <div class="col-md-6 col-md-push-3">
                <asp:Button ID="SubmitButton" runat="server" Text="Continue" UseSubmitBehavior="true" CssClass="btn btn-success btn-lg col-md-12" OnClick="SubmitButton_Click"/>
            </div>
        </div>
    </div>
    </div>
</asp:Content>