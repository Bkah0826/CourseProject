<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" Async="true" ValidateRequest="false" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="AdminLogin">

    <link rel="stylesheet" type="text/css" href="../Content/ManageSites.css">
    <div class="row">
        <div class="col-md-12">
            <section id="loginForm">
                <div class="form-horizontal">
                    <asp:Panel runat="server" ID="EnterKeyPanel" DefaultButton="LogInActionButton">
                        <asp:Image ID="Logo" runat="server" ImageUrl="../Images/Covenant-Health-Logo.png" CssClass="loginImage img-responsive" />
                        <h1 class="text-center">Meal Satisfaction System</h1>
                        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                        <div class="form-group col-sm-12">
                            <asp:Label runat="server" AssociatedControlID="UserName" CssClass="col-sm-5 control-label">Username</asp:Label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                <asp:TextBox runat="server" ID="UserName" CssClass="form-control" MaxLength="256" />
                            </div>
                        </div>
                        <div class="form-group col-sm-12">
                            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-sm-5 control-label">Password</asp:Label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" MaxLength="40" CssClass="form-control" CausesValidation="true" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-7 control-label">
                                <asp:LinkButton ID="loginButton" runat="server" CssClass="btn btn-link">Need help logging in?</asp:LinkButton>
                                <ajaxToolkit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="loginButton" OkControlID="btnYes"
                                    BackgroundCssClass="shadow">
                                </ajaxToolkit:ModalPopupExtender>

                                <!-- Popup that appears when deactivate is clicked. -->
                                <asp:Panel ID="pnlPopup" runat="server" CssClass="deactivate-popup" Style="display: none">
                                    <div class="deactivate-popup-header panel-heading  text-center" style="text-align: center">
                                        <h3>Login Help</h3>
                                    </div>
                                    <div class="deactivate-popup-body panel-body text-center">
                                        <p>Need help logging in?
                                            <br />
                                            <br />
                                            Please contact your employer to help you recover your account username or reset your password.</p>

                                        <asp:Button ID="btnYes" runat="server" Text="Back" CssClass="btn btn-danger" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="form-group col-md-12">
                            <div class="col-sm-7 control-label">
                                <asp:Button runat="server" OnClick="LogIn" ID="LogInActionButton" Text="Log in" CssClass="btn btn-success" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
