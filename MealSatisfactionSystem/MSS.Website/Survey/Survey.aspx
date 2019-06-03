<%@ Page Title="Meal Service Satisfaction Survey" Language="C#" MasterPageFile="~/SurveyPortal.master" AutoEventWireup="true" CodeFile="Survey.aspx.cs" Inherits="Survey" ValidateRequest="false"%>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Survey" ContentPlaceHolderID="Survey" Runat="Server">
    <%-- Object datasources start here --%>
    <%-- Unit ODS --%>
    <asp:ObjectDataSource ID="UnitListODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="SiteUnitList" TypeName="MSSSystem.BLL.UnitController">
      <SelectParameters>
          <asp:Parameter Name="siteId" Type="Int32" />
      </SelectParameters>
    </asp:ObjectDataSource>
    <%-- Q1A ODS --%>
    <asp:ObjectDataSource ID="Q1AAnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="1" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <%-- Q1B ODS --%>
       <asp:ObjectDataSource ID="Q1BAnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="2" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <%-- Q1C ODS --%>
       <asp:ObjectDataSource ID="Q1CAnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="3" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <%-- Q1D ODS --%>
       <asp:ObjectDataSource ID="Q1DAnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="4" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <%-- Q1E ODS --%>
       <asp:ObjectDataSource ID="Q1EAnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="5" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <%-- Q2 ODS --%>
    <asp:ObjectDataSource ID="Q2AnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
        SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="6" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <%-- Q3 ODS --%>
    <asp:ObjectDataSource ID="Q3AnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
            SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="7" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
      <%-- Q4 ODS --%>
    <asp:ObjectDataSource ID="Q4AnswerODS" runat="server" OldValuesParameterFormatString="original_{0}" 
            SelectMethod="AnswerList" TypeName="MSSSystem.BLL.AnswerController">
        <SelectParameters>
            <asp:Parameter DefaultValue="8" Name="questionId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
<%-- Object Datasource ends here --%>
    
    <div class="TakeSurvey"> <%--this div defines class for entire page useful for styling--%>
    <div class="container" >
        <div class="jumbotron">   
            <asp:Image ID="Logo" runat="server" ImageUrl="~/Images/Covenant-Health-Logo.png" CssClass="img-responsive center-block" AlternateText="Covenant Health Logo"/>
            <div class="text">
            <h1>Meal Service Satisfaction Survey</h1>
                </div>
        </div>
  

    <%--Actual survey starts here --%>
    <div class="row">  
        <div class="col-xs-12 col-sm-12 col-md-12">
              <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
            <asp:Panel ID="SurveyPanel" runat="server" CssClass="panel panel-default center-block">
                <div class="panel-heading">
                    <h2>Please Provide the Following:</h2>
                    <p>*All questions are optional</p>
                </div>
               
                <div class="panel-body">
                    <div class="SurveyContent col-md-6 col-md-push-3">
                    <div class="form-group">
                        <%--Required Field --%>
                        <asp:Label ID="UnitLabel" runat="server" Text="Unit (Required)" AssociatedControlID="CPUnitDDL"/>
                        <asp:DropDownList ID="CPUnitDDL" runat="server" DataSourceID="UnitListODS" DataTextField="Description" DataValueField="UnitID" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Selected="True" Value="0">Select...</asp:ListItem>
                        </asp:DropDownList> 
                    </div>
                        <hr />
                    <ol style="list-style-type:none;">
                        <li>
                            <asp:Label ID="Q1Label" runat="server" AssociatedControlID="Q1ADDL" />
                            <ol style="list-style-type:none;">
                                <li>
                                    <div class="form-group">
                                        <asp:Label ID="Q1ALabel" runat="server" AssociatedControlID="Q1ADDL"/>
                                        <asp:DropDownList ID="Q1ADDL" runat="server" DataSourceID="Q1AAnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <asp:Label ID="Q1BLabel" runat="server" AssociatedControlID="Q1BDDL"/>
                                        <asp:DropDownList ID="Q1BDDL" runat="server" DataSourceID="Q1BAnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <asp:Label ID="Q1CLabel" runat="server"  AssociatedControlID="Q1CDDL"/>
                                        <asp:DropDownList ID="Q1CDDL" runat="server" DataSourceID="Q1CAnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <asp:Label ID="Q1DLabel" runat="server"  AssociatedControlID="Q1DDDL"/>
                                        <asp:DropDownList ID="Q1DDDL" runat="server" DataSourceID="Q1DAnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <asp:Label ID="Q1ELabel" runat="server" AssociatedControlID="Q1EDDL"/>
                                        <asp:DropDownList ID="Q1EDDL" runat="server" DataSourceID="Q1EAnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </li>
                            </ol>
                        </li>
                        <li>
                            <div class="form-group">
                                <asp:Label ID="Q2Label" runat="server" AssociatedControlID="Q2DDL"/>
                                <asp:DropDownList ID="Q2DDL" runat="server" DataSourceID="Q2AnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <asp:Label ID="Q3Label" runat="server"  AssociatedControlID="Q3DDL"/>
                                <asp:DropDownList ID="Q3DDL" runat="server" DataSourceID="Q3AnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <asp:Label ID="Q4Label" runat="server"  AssociatedControlID="Q4DDL"/>
                                <asp:DropDownList ID="Q4DDL" runat="server" DataSourceID="Q4AnswerODS" DataTextField="Description" DataValueField="AnswerId" CssClass="form-control" AppendDataBoundItems="true">
                                            <asp:ListItem Selected="True" Value="19">Select...</asp:ListItem>
                                        </asp:DropDownList>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <asp:Label ID="Q5Label" runat="server" AssociatedControlID="Q5TextBox"/>
                                <asp:TextBox ID="Q5TextBox" runat="server" CssClass="form-control" Rows="5" TextMode="MultiLine" Style="min-width:100%; min-height:100px; margin:0 auto;"/>
                            </div>
                        </li>
                    </ol>
                     </div>
                 </div>
            </asp:Panel>
        </div>
          </div>
  

    <%--Customer Profile section --%>
    <div class="row">
        <div class="col-md-12 ">
            <asp:Panel ID="CustomerProfilePanel" runat="server" CssClass="panel panel-default">
                <div class="panel-heading">
                    <h2>Customer Profile:</h2>
                    <p>*The following information will remain anonymous</p>
                </div>
                <div class="panel-body">
                    <div class="ProfileContent col-md-6 col-md-push-3">
                    <div class="form-group">
                        <asp:Label ID="AgeLabel" runat="server" Text="Age" AssociatedControlID="AgeDDL" />
                        <asp:DropDownList ID="AgeDDL" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Prefer not to provide"  Selected="True"/>
                            <asp:ListItem Text="Under 18"/>
                            <asp:ListItem Text="18-34" />
                            <asp:ListItem Text="35-54" />
                            <asp:ListItem Text="55-74"/>
                            <asp:ListItem Text="75+"/>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Gender" ID="GenderLabel" AssociatedControlID="GenderDDL"/>
                        <asp:DropDownList ID="GenderDDL" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Other/ Prefer not to provide" Value="O" Selected="True"/>
                            <asp:ListItem Text="Male" Value="M"/>
                            <asp:ListItem Text="Female" Value="F"/>
                        </asp:DropDownList>
                    </div>
                    </div>
                </div>

            </asp:Panel>
        </div>
    </div>
        <%-- Popup panel starts here --%>
                                    <AjaxToolKit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="Continue"                CancelControlID="Cancelbtn"  BackgroundCssClass="shadow">
                                    </AjaxToolKit:ModalPopupExtender>

                                    <!-- Popup that appears when Needhelp is clicked. -->
                                    <asp:Panel ID="pnlPopup" runat="server" CssClass="survey-popup" Style="display: none">
                                        <div class="survey-popup-header panel-heading">
                                            <h3>Privacy Statement Agreement</h3>
                                        </div>
                                        <div class="survey-popup-body panel-body">
                                            <p>By pressing submit you are agreeing to Covenant Health's Privacy statement</p>
                                            <p><a href="https://www.covenanthealth.ca/privacy-statement" class="btn btn-link" target="_blank">Privacy Statement</a></p>
                                           <%-- Submit Survey Buttons --%>
                                            <asp:Button ID="Submitbtn" runat="server" Text="Submit" CssClass="btn btn-success" OnClick="Submit_Click"  />
                                             <asp:Button ID="Cancelbtn" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="Back_Click"/>
                                        </div>
                                    </asp:Panel>
        <%-- Survey buttons --%> 
    <div class="row">
        <div class="col-md-6 col-md-push-3">            
            <asp:Button ID="Back" runat="server" Text="Back" CssClass="btn btn-danger btn-lg col-md-4" OnClick="Back_Click"/>
            <asp:Button ID="Continue" runat="server" Text="Continue" CssClass="btn btn-success btn-lg col-md-4" style="float:right"/>
        </div>
    </div>
          </div>
    </div>

</asp:Content>