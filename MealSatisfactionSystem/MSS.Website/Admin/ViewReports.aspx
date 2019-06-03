<%@ Page Title="View Reports" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewReports.aspx.cs" Inherits="Admin_ViewReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SiteAccess" runat="Server">
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="../Scripts/moment.js"></script>
    <script src="../Scripts/reimg.js"></script>
    <script src="//cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.5/jspdf.min.js"></script>

    <%-- Allows use of E6 in IE11 --%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bluebird/3.3.4/bluebird.min.js"></script>

    <script src="https://cdn.jsdelivr.net/npm/moment@latest/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.0/Chart.min.js"></script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="../Content/charting.css">

    <style>
        .navbar {
            z-index: 10000;
        }

        #modalContainer {
            background-color: rgba(0, 0, 0, 0.3);
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            z-index: 10000;
            background-image: url(tp.png); /* required by MSIE to prevent actions on lower z-index elements */
            border-left: 1px solid #ddd;
            border-right: 1px solid #ddd;
            border-bottom: 2px solid #ddd;
        }

        #alertBox {
            position: relative;
            width: 500px;
            min-height: 100px;
            margin-top: 50px;
            background-color: #fff;
            background-repeat: no-repeat;
            background-position: 20px 30px;
            border-bottom-left-radius: 8px;
            border-bottom-right-radius: 8px;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }

            #alertBox > p {
                padding: 10px;
            }

            #alertBox > a {
                margin: 15px;
                width: 100%;
                background-color: #286090;
                color: white;
                padding: 5px 20px;
                border-radius: 4px;
                position: relative;
                bottom: 10px;
                left: 205px;
            }

        #alert > a:hover {
            text-decoration: none;
        }

        #modalContainer > #alertBox {
            position: fixed;
        }

        #alertBox h1 {
            padding: 10px 15px;
            margin: 0;
            font: bold 0.9em Raleway;
            background-color: #468CC8;
            color: #FFF;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            border-left: 1px solid #468CC8;
            border-right: 1px solid #468CC8;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="card heading">
        <h1>Reports</h1>
        <small>Build custom reports to view the data you need.</small>
    </div>
    <div id="tabs" style="font-family:Raleway">
        <ul>
            <li><a href="#mainCard" class="noselect">Reports</a></li>
            <li><a href="#percentileTab" class="noselect">Weighted Percentile</a></li>

        </ul>
        <div id="mainCard" class="card card-body" style="display: flex;">
            <div id="leftParameterContainer" class="well">
                <div id="dateSection">
                    <p>
                        From:
                    <input id="fromDatePicker" autocomplete="off" readonly="readonly"/><i style="color: #d9534f" class="glyphicon glyphicon-remove"></i>
                    </p>
                    <p>
                        To:
                    <input id="toDatePicker" autocomplete="off" readonly="readonly"/><i style="color: #d9534f" class="glyphicon glyphicon-remove"></i>
                    </p>
                </div>
                <div id="datePresets">
                    <a href="#" id="prevWeek">Previous Week</a> |
                <a href="#" id="prevMonth">Previous Month</a> |
                <a href="#" id="ytd">Year To Date</a>
                </div>

                <h4>Filters</h4>
                <button id="clearParameterFilters" class="btn btn-danger btn-xs">Clear All</button>

                <% if (User.IsInRole("SuperUser"))
                    { %>
                <div>
                    <h5>Site</h5>

                    <asp:DropDownList runat="server" AutoPostBack="true" ID="SiteDD" DataSourceID="SiteDDDS" DataTextField="SiteName" DataValueField="SiteId" OnDataBound="SiteDD_DataBound">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                    <asp:ObjectDataSource runat="server" ID="SiteDDDS" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="false" Name="deactivated" Type="Boolean"></asp:Parameter>
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <% } %>
                <div>
                    <div style="display: flex;">
                        <div id="genderParameterSection">
                            <h5>Gender</h5>
                            <asp:CheckBoxList ID="genderRadioList" runat="server">
                                <asp:ListItem Value="-1" Selected="true">Not Specified</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="O">Other/Prefer not to say</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                        <div id="ageParameterSection">
                            <h5>Age</h5>
                            <asp:CheckBoxList ID="ageRadioList" runat="server">
                                <asp:ListItem Value="-1" Selected="true">Not Specified</asp:ListItem>
                                <asp:ListItem Value="1">Under 18</asp:ListItem>
                                <asp:ListItem Value="2">18-34</asp:ListItem>
                                <asp:ListItem Value="3">35-54</asp:ListItem>
                                <asp:ListItem Value="4">55-74</asp:ListItem>
                                <asp:ListItem Value="5">75+</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div id="unitsSection">
                        <h5>Units</h5>
                        <div id="unitDBParameters">
                            <input id="defaultUnitCheck" type="checkbox" value="All" checked="true" /><label id="defaultUnitCheckLabel" for="defaultUnitCheck">All Units</label>
                            <asp:PlaceHolder runat="server" ID="unitPlaceholder"></asp:PlaceHolder>
                            <%-- Auto generated at load from db --%>
                        </div>

                    </div>
                    <div id="parameterSection">
                        <h5>Parameters</h5>
                        <small>Must select a minimum of one paramater from the below options, maximum of three.</small>
                        <div id="dbParameters">
                            <asp:PlaceHolder runat="server" ID="questionParameterPlaceHolder"></asp:PlaceHolder>
                            <%-- Auto generated at load from db --%>
                        </div>

                    </div>
                    <button id="generateChart" type="button" class="btn btn-success">Generate Chart</button>
                    <label id="params-errormsg" class="hidden error-msg">Must select between 1 - 3 Parameters.</label>
                </div>
            </div>

            <div id="rightChartContainer" class="well rightParameterContainer">
                <div id="chartHeaderGroup">
                    <div id="pieChartPagingGroup" class="hidden">
                        <i class="glyphicon glyphicon-arrow-left noselect"></i>
                        <p><span id="fraction">1</span> / <span id="demoninator">3</span></p>
                        <i class="glyphicon glyphicon-arrow-right noselect"></i>
                    </div>
                </div>
                <div id="chartBody">
                    <div class="loader hidden"></div>
                    <div>
                        <img src="../Images/graph_placeholder.png" />
                        <%-- <canvas id="dynamicChart" class="dynChart" style="width: 512px; height: 356px; padding: 0;margin: auto; "> </canvas>--%>
                    </div>
                    <p id="filters" class="small"></p>

                </div>

                <div id="chartFooterGroup">
                    <div>
                        <div class="print-btn">
                            <button id="printGraph" type="button" class="btn btn-default">Download</button>
                            <div class="loader hidden"></div>
                            <p class="hidden saved">Saved!</p>
                            <p class="hidden" style="color: #d9534f">Save failed.</p>

                        </div>
                        <div class="btn-group" role="group" aria-label="...">
                            <button type="button" class="btn btn-default active">Pie</button>
                            <button type="button" class="btn btn-default">Line</button>
                            <button type="button" class="btn btn-default">Bar</button>
                        </div>
                    </div>
                    <label id="btngroup-errormsg" class="hidden error-msg">Must select a chart type.</label>
                </div>

            </div>

        </div>

        <div id="percentileTab" class="card card-body">
            <div class="well">
                <div class="weightedDateContainer">
                    <div>
                        <div id="dateSection_p">
                            <p>
                                From:
                        <input id="fromDatePicker_p" autocomplete="off" /><i style="color: #d9534f" class="glyphicon glyphicon-remove"></i>
                            </p>
                            <p>
                                To:
                        <input id="toDatePicker_p" autocomplete="off"/><i style="color: #d9534f" class="glyphicon glyphicon-remove"></i>
                            </p>
                        </div>
                        <div id="datePresets_p">
                            <a href="#" id="prevWeek_p">Previous Week</a> |
                    <a href="#" id="prevMonth_p">Previous Month</a> |
                    <a href="#" id="ytd_p">Year To Date</a>
                        </div>
                    </div>
                    <% if (User.Identity.Name == "Webmaster")
                        { %>
                    <div>
                        <h5>Site:</h5>

                        <asp:DropDownList runat="server" AutoPostBack="false" ID="SiteDD_p" DataSourceID="SiteDDDS_p" DataTextField="SiteName" DataValueField="SiteId">
                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                        </asp:DropDownList>
                        <asp:ObjectDataSource runat="server" ID="SiteDDDS_p" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="false" Name="deactivated" Type="Boolean"></asp:Parameter>
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </div>
                    <% } %>
                </div>
                
                <div class=".download_p" style="display: flex;">
                    <button id="generate_p" class="btn btn-success">Generate</button>
                    <button id="download_p" class="btn btn-default hidden">Download</button>
                    <div id="loader_p" class="loader hidden"></div>
                    <p id="saved_p" class="hidden saved">Saved!</p>
                    <p id="failure_p" class="hidden" style="color: #d9534f">Save failed.</p>
                </div>
                <div id="weightedContainer" class="containers">
                    <div class="loader hidden" style="margin: 0 auto;"></div>
                    <div id="weightedHead">
                    </div>
                    <div id="weightedBody">
                       
                    </div>                    
                </div>
            </div>
        </div>
    </div>

    <script src="../Scripts/custom/charting.js"></script>
    <script src="../Scripts/custom/charting_percentile.js"></script>
    <script>
        $("document").ready(function () {
            Charting()
            Charting_Percentile()
        });
    </script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AdminLogin" runat="Server">
</asp:Content>

