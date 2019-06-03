<%@ Page Title="View Surveys" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewResponses.aspx.cs" Inherits="Admin_ViewResponses" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="ViewResponses" ContentPlaceHolderID="MainContent" runat="Server">
    <!-------------------------------
        LINKS FOR STYLES AND SCRIPTS   
    ---------------------------------->
    <link rel="stylesheet" type="text/css" href="../Content/ViewSurveyResponses.css">
    <link rel="stylesheet" type="text/css" href="../Content/ViewIndividualSurvey.css">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <!-------------------------------
        VIEW RESPONSES PAGE  
    ---------------------------------->
    <!--Title and Tagline (text changes based on what section the user is looking at)-->
    <div class="card">
        <h2 id="PageHeading" runat="server">Surveys</h2>
        <p id="PageTagline" runat="server">View and filter all submitted survey responses from active sites and units.</p>
    </div>
    <hr />
    <!-------------------------------
        INDIVIDUAL RESPONSE SCREEN    
    ---------------------------------->
    <div id="IndividualResponseScreen" runat="server">
        <div id="Alert" runat="server" class="info-container special-padding-bottom">
            <!--Message User Control for Individual Response Screen-->
            <div class="row">
                <div class="col-md-12">
                    <uc1:MessageUserControl ID="MessageUserControlIndividual" runat="server" />
                </div>
            </div>
        </div>
        <!--Individual Response Container-->
        <div class="info-container">
            <!--Top Back Button-->
            <div class="row">
                <div class="col-md-4">
                    <asp:Button ID="Back" runat="server" Text="Back to All Surveys" OnClick="Back_Click" CssClass="btn btn-danger btn-special" />
                </div>
            </div>
        </div>
        <div runat="server" id="SurveyInformation" class="survey-container">
            <!--Individual Response Information-->
            <div class="row">
                <div class="col-md-7">
                    <div class="panel panel-primary panel-special">
                        <!--Survey Information Panel -->
                        <div class="panel-heading">
                            Survey Information
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="SiteUnitLabel" runat="server" Text="Location:" Font-Bold="true" />
                                </div>
                                <div class="col-md-9">
                                    <asp:Label ID="Location" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="DateLabel" runat="server" Text="Submitted:" Font-Bold="true" />
                                </div>
                                <div class="col-md-9">
                                    <asp:Label ID="Date" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5">
                    <div class="panel panel-info panel-special">
                        <!-- Client Profile Panel-->
                        <div class="panel-heading">
                            Client Profile
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="GenderLabel" runat="server" Text="Gender:" Font-Bold="true" />
                                </div>
                                <div class="col-md-9">
                                    <asp:Label ID="Gender" runat="server" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:Label ID="AgeLabel" runat="server" Text="Age:" Font-Bold="true" />
                                </div>
                                <div class="col-md-9">
                                    <asp:Label ID="Age" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!--Question One-->
            <div class="text-overflow-survey">
                <div class="question">
                    <div class="row">
                        <asp:Label ID="Question1Label" runat="server" Font-Bold="True" />
                    </div>
                    <div class="sub-question">
                        <div class="row">
                            <!--Question One A-->
                            <div class="col-md-8">
                                <asp:Label ID="Question1ALabel" runat="server" />
                            </div>
                            <!--Question One A Response-->
                            <div class="col-md-4">
                                <asp:Label ID="Question1AResponse" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="sub-question-alternate">
                        <div class="row">
                            <!--Question One B-->
                            <div class="col-md-8">
                                <asp:Label ID="Question1BLabel" runat="server" />
                            </div>
                            <!--Question One B Response-->
                            <div class="col-md-4">
                                <asp:Label ID="Question1BResponse" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="sub-question">
                        <div class="row">
                            <!--Question One C-->
                            <div class="col-md-8">
                                <asp:Label ID="Question1CLabel" runat="server" />
                            </div>
                            <!--Question One C Response-->
                            <div class="col-md-4">
                                <asp:Label ID="Question1CResponse" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="sub-question-alternate">
                        <div class="row">
                            <!--Question One D-->
                            <div class="col-md-8">
                                <asp:Label ID="Question1DLabel" runat="server" />
                            </div>
                            <!--Question One D Response-->
                            <div class="col-md-4">
                                <asp:Label ID="Question1DResponse" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="sub-question">
                        <div class="row">
                            <!--Question One E-->
                            <div class="col-md-8">
                                <asp:Label ID="Question1ELabel" runat="server" />
                            </div>
                            <!--Question One E Response-->
                            <div class="col-md-4">
                                <asp:Label ID="Question1EResponse" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <!--Question Two-->
                <div class="question">
                    <div class="row">
                        <asp:Label ID="Question2Label" runat="server" Font-Bold="true" />
                    </div>
                    <!--Question Two Response-->
                    <div class="row">
                        <div class="col-md-12 response">
                            <asp:Label ID="Question2Response" runat="server" />
                        </div>
                    </div>
                </div>
                <!--Question Three-->
                <div class="question">
                    <div class="row">
                        <asp:Label ID="Question3Label" runat="server" Font-Bold="true" />
                    </div>
                    <!--Question Three Response-->
                    <div class="row">
                        <div class="col-md-12 response">
                            <asp:Label ID="Question3Response" runat="server" />
                        </div>
                    </div>
                </div>
                <!--Question Four-->
                <div class="question">
                    <div class="row">
                        <asp:Label ID="Question4Label" runat="server" Font-Bold="true" />
                    </div>
                    <!--Question Four Response-->
                    <div class="row">
                        <div class="col-md-12 response">
                            <asp:Label ID="Question4Response" runat="server" />
                        </div>
                    </div>
                </div>
                <!--Question Five-->
                <div class="question">
                    <div class="row">
                        <asp:Label ID="Question5Label" runat="server" Font-Bold="true" />
                    </div>
                    <!--Question Five Response-->
                    <div class="row">
                        <div class="col-md-12 response">
                            <asp:Label ID="Question5Response" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="info-container special-padding-top">
            <!--Bottom Back Button (included so the user doesn't have to scroll back to the top to get to the SurveyListViewScreen)-->
            <div class="row">
                <div class="col-md-4">
                    <asp:Button ID="BackBottom" runat="server" Text="Back to All Surveys" OnClick="Back_Click" CssClass="btn btn-danger btn-special" />
                </div>
            </div>
        </div>
    </div>
    <!-------------------------------
        SURVEY LISTVIEW SCREEN    
    ---------------------------------->
    <div id="SurveysListViewScreen" runat="server">
        <!--Survey Listview Tab (done to match the layout of all the management pages)-->
        <div class="row">
            <ul class="nav-tabs nav">
                <li class="active"><a href="#active" data-toggle="tab">Surveys</a></li>
            </ul>
            <div class="tab-content clearfix">
                <!--Survey Listview Pane-->
                <div class="tab-pane fade in active" id="active">
                    <div class="searchBar width-control text-overflow">
                        <asp:Panel ID="FiltersPanel" runat="server" DefaultButton="ApplyFiltersButton">
                            <table>
                                <!-- Apply Filters Button-->
                                <tr>
                                    <td>
                                        <div class="section">
                                            <asp:LinkButton ID="ApplyFiltersButton" runat="server" Text="Apply Filters" OnClick="ApplyFilters_Click" CssClass="btn btn-success" Width="180px">Apply Filters <i style="color:white" class="glyphicon glyphicon-ok"></i></asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                                <!--Clear Filters Button-->
                                <tr>
                                    <td>
                                        <div class="section">
                                            <asp:LinkButton ID="ClearFiltersButton" runat="server" Text="Clear Filters" OnClick="ClearFilters_Click" CssClass="btn btn-danger" Width="180px">Clear Filters <i style="color:white" class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                                        </div>
                                    </td>
                                </tr>
                                <!--From Date Picker (uses jQuery script to display calendar)-->
                                <tr>
                                    <td>
                                        <div class="searchBarSection">
                                            <asp:Label ID="FromDateLabel" runat="server" Text="From" Font-Bold="true"></asp:Label><br />
                                            <asp:TextBox ID="FromDate" runat="server" TextMode="DateTime" Width="180px" />
                                        </div>
                                    </td>
                                </tr>
                                <!-- To Date Picker (uses jQuery script to display calendar)-->
                                <tr>
                                    <td>
                                        <div class="searchBarDateSection">
                                            <asp:Label ID="ToDateLabel" runat="server" Text="To" Font-Bold="true"></asp:Label><br />
                                            <asp:TextBox ID="ToDate" runat="server" TextMode="DateTime" Width="180px" />
                                        </div>
                                    </td>
                                </tr>
                                <!-- Site Dropdown List (populates all the active sites in the database using an ObjectDataSource)-->
                                <tr>
                                    <td>
                                        <div id="SiteHide" runat="server" class="searchBarSection">
                                            <asp:Label ID="SiteSearchLabel" runat="server" Text="Site" Font-Bold="True"></asp:Label><br />
                                            <asp:DropDownList ID="SiteDDL" runat="server" DataSourceID="AllSitesODS" DataTextField="SiteName" AppendDataBoundItems="true" DataValueField="SiteId" AutoPostBack="true" OnSelectedIndexChanged="SiteDDL_SelectedIndexChanged" Width="180px">
                                                <asp:ListItem Value="0" Text="All Sites" runat="server" />
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <!--Unit Checkbox List (populates all the site's units in the database using an ObjectDataSource/code behind, only visible when a specific site has been selected)-->
                                <tr>
                                    <td class="checkboxItem">
                                        <div id="UnitHide" runat="server" class="checkboxSection">
                                            <asp:Label ID="UnitSearchLabel" runat="server" Text="Unit" Font-Bold="true"></asp:Label><br />
                                            <div class="checkboxList scroll">
                                                <%--<asp:PlaceHolder ID="Test" runat="server" ></asp:PlaceHolder>--%>
                                                <asp:CheckBoxList ID="UnitCheckboxList" runat="server" DataSourceID="SiteUnitsODS" DataTextField="UnitName" DataValueField="UnitName">
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="UnitAlert" runat="server" class="searchBarSection">
                                            <asp:Label ID="UnitAlertLabel" runat="server" Text="Unit" Font-Bold="true" /><br />
                                            <asp:Label ID="NoUnits" runat="server" Text="No Active Units" />
                                        </div>
                                    </td>
                                </tr>
                                <!--Gender Checkbox List (all options have been hardcoded in the list items, the list does not need to be populated via an ObjectDataSource because gender options cannot be changed within MSS)-->
                                <tr>
                                    <td class="checkboxItem">
                                        <div class="checkboxSection">
                                            <asp:Label ID="GenderSearchLabel" runat="server" Text="Gender" Font-Bold="True"></asp:Label><br />
                                            <div class="checkboxList">
                                                <asp:CheckBoxList ID="GenderCheckboxList" runat="server" TextAlign="Right" Font-Bold="false">
                                                    <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
                                                    <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
                                                    <asp:ListItem Value="Other / Not Provided" Text="Other / Not Provided"></asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <!--Age Checkbox List (all options have been hardcoded in the list items, the list does not need to be populated via an ObjectDataSource because age options cannot be changed within MSS)-->
                                <tr>
                                    <td class="checkboxItem">
                                        <div class="checkboxSection">
                                            <asp:Label ID="AgeSearchLabel" runat="server" Text="Age" Font-Bold="True"></asp:Label><br />
                                            <div class="checkboxList">
                                                <asp:CheckBoxList ID="AgeCheckboxList" runat="server" TextAlign="Right">
                                                    <asp:ListItem Value="Under 18">Under 18</asp:ListItem>
                                                    <asp:ListItem Value="18-34">18-34</asp:ListItem>
                                                    <asp:ListItem Value="35-54">35-54</asp:ListItem>
                                                    <asp:ListItem Value="55-74">55-74</asp:ListItem>
                                                    <asp:ListItem Value="75+">75+</asp:ListItem>
                                                    <asp:ListItem Value="Not Provided">Not Provided</asp:ListItem>
                                                </asp:CheckBoxList>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <!-- Survey ListView (displays all surveys that match the selected filter criteria on ApplyFilters_Click(), it displays all surveys from active sites and units on Page_Load() and ClearFilters_Click()-->
                    <div class="ListView rounded_corners">
                        <!--Message User Control for Survey ListView Screen-->
                        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
                        <div class="rounded_corners">
                            <asp:ListView ID="SurveyOverviewListView" runat="server" DataSourceID="ResponseAllODS" OnPagePropertiesChanging="SurveyOverviewListView_PagePropertiesChanging">
                                <EmptyDataTemplate>
                                    <table runat="server">
                                        <tr class="no-response">
                                            <td class="no-response">No responses matching the applied filters were found.</td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <ItemTemplate>
                                    <tr style="background-color: #FFF; color: #333333; border-bottom: 1px solid #dddddd">
                                        <td>
                                            <asp:TextBox ID="SiteNameTextBox" runat="server" Text='<%# Bind("SiteName") %>' Enabled="False" CssClass="listViewBox site" /></td>
                                        <td>
                                            <asp:TextBox ID="DescriptionTextBox" runat="server" Text='<%# Bind("UnitName") %>' Enabled="False" CssClass="listViewBox unit" /></td>
                                        <td>
                                            <asp:TextBox ID="DateTextBox" runat="server" Text='<%# Bind("Date", "{0:MMMM dd, yyyy}") %>' Enabled="False" CssClass="listViewBox date" /></td>
                                        <td>
                                            <asp:TextBox ID="GenderTextBox" runat="server" Text='<%# Bind("Gender") %>' Enabled="False" CssClass="listViewBox gender" /></td>
                                        <td>
                                            <asp:TextBox ID="AgeTextBox" runat="server" Text='<%# Bind("Age") %>' Enabled="False" CssClass="listViewBox age" /></td>
                                        <td>
                                            <asp:Button ID="View" runat="server" CommandArgument='<%#Eval("ResponseId")%>' OnClick="View_Click" Text="View" CssClass="action btn-primary" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <LayoutTemplate>
                                    <table runat="server">
                                        <tr runat="server">
                                            <td runat="server">
                                                <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif" border="1">
                                                    <tr runat="server" style="background-color: #E0FFFF; color: #333333">
                                                        <th runat="server" class="site">Site</th>
                                                        <th runat="server">Unit</th>
                                                        <th runat="server">Date</th>
                                                        <th runat="server">Gender</th>
                                                        <th runat="server">Age</th>
                                                        <th runat="server"></th>
                                                        <th></th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder"></tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr runat="server">
                                            <td runat="server" class="footerBar" style="text-align: center; background-color: #FFFFFF; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000">
                                                <asp:DataPager runat="server" ID="DataPager1" PageSize="10" PagedControlID="SurveyOverviewListView">
                                                    <Fields>
                                                        <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous" NextPreviousButtonCssClass="btn-primary nextprev footer-buttons" NumericButtonCssClass="btn-primary footer-buttons" />
                                                    </Fields>
                                                </asp:DataPager>
                                            </td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-------------------------------
        OBJECT DATA SOURCES
    ---------------------------------->
    <!--ODS for Site Dropdown List-->
    <asp:ObjectDataSource ID="AllSitesODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
        <SelectParameters>
            <asp:Parameter DefaultValue="false" Name="deactivated" Type="Boolean"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <!--ODS for Unit Checkbox List-->
    <asp:ObjectDataSource ID="SiteUnitsODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="SiteUnitList" TypeName="MSSSystem.BLL.UnitController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SiteDDL" PropertyName="SelectedValue" DefaultValue="0" Name="siteId" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <!--ODS for Response ListView-->
    <asp:ObjectDataSource ID="ResponseAllODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Response_List_All" TypeName="MSSSystem.BLL.SurveyResponseController"></asp:ObjectDataSource>
    <!-------------------------------
        SCRIPTS   
    ---------------------------------->
    <!-- Script for jQuery Datepicker-->
    <script>
        $(function () {
            $('#<%=ToDate.ClientID %>').datepicker({
                maxDate: "+0d",
                changeMonth: true,
                changeYear: true
            });
            $('#<%=FromDate.ClientID %>').datepicker({
                maxDate: "+0d",
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
</asp:Content>