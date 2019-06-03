<%@ Page Title="Manage Units" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageUnits.aspx.cs" Inherits="Admin_ManageUnits" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="ManageUnits" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        /* This style piece is used to keep the width of the description in Edit mode the correct size. */
        .description {
            min-width: 100%;
        }

        .addTabSiteName {
            min-width: 100%;
        }

        .addTabDescription {
            min-width: 100%;
        }

        .addTabPasscode {
            min-width: 100%;
        }
    </style>

    <link rel="stylesheet" href="../Content/ManageSites.css">
    <h1>Manage Units</h1>
    <p>Add, update or deactivate a unit or lookup deactivated units.</p>
    <hr />

    <!-- This is the MessageUserControl, if there is an error it will show here in a user friendly manner. -->
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

    <div class="row">
        <div>
            <asp:Label ID="UserSiteId" Text="" runat="server" Visible="false" />

            <!-- Navigation Tabs -->
            <ul class="nav nav-tabs" id="myTab" style="padding-left: 0px;">
                <li class="active"><a href="#active" data-toggle="tab">Update Unit</a></li>
                <li><a href="#add" data-toggle="tab">Add Unit</a></li>
                <li><a href="#deactivated" data-toggle="tab">View Inactive Units</a></li>
            </ul>

            <!-- Tab Content Area -->
            <div class="tab-content clearfix">

                <!-- This is the tab pane showing all the Active units in the database.-->
                <div class="tab-pane fade in active" id="active">

                    <!-- Active Search Bar -->
                    <div class="searchBar">
                        <asp:Panel ID="DefaultButtonPanel" runat="server" DefaultButton="ActiveSearchButton">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="ActiveSearchBox" runat="server" placeholder="Search" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Label runat="server" ID="ActiveSearchLabel" Text="Search by:" CssClass="title-label"></asp:Label><br />
                                            <asp:RadioButtonList runat="server" ID="ActiveSearchBy" CssClass="radioButtonList">
                                                <asp:ListItem ID="ActiveAllCheckbox" runat="server" Text="All" Selected="True"></asp:ListItem>
                                                <asp:ListItem ID="ActiveSiteNameCheckbox" runat="server" Text="Site Name"></asp:ListItem>
                                                <asp:ListItem ID="ActiveUnitNameCheckbox" runat="server" Text="Unit Name"></asp:ListItem>
                                                <asp:ListItem ID="ActiveDescriptionCheckbox" runat="server" Text="Description"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <hr />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ActiveSearchButton" runat="server" OnClick="ActiveSearchButton_Click" Text="Search" class="btn btn-primary" UseSubmitBehavior="False" />
                                        <asp:Button ID="ActiveClearButton" runat="server" OnClick="ActiveClearButton_Click" Text="Clear" class="btn btn-danger" UseSubmitBehavior="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>

                    <!-- Active listview -->
                    <div class="ListView rounded_corners">
                        <asp:ListView ID="ActiveUnitListView" runat="server" DataSourceID="ActiveUnitListView_ODS" DataKeyNames="UnitId" OnItemCommand="UnitListView_ItemCommand"
                            OnItemCanceling="ActiveSiteListView_ItemCanceling" OnItemDataBound="ActiveSiteListView_ItemDataBound" OnItemEditing="ActiveSiteListView_ItemEditing">
                            <EditItemTemplate>
                                <tr style="margin-bottom: 58px">
                                    <td>
                                        <asp:DropDownList ID="SiteNameDDL" runat="server" DataSourceID="ActiveSiteNameDDL_ODS" DataTextField="SiteName"
                                            DataValueField="SiteId" SelectedValue='<%# Bind("SiteId") %>' CssClass="listViewDDL siteName edit" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("UnitName") %>' runat="server" ID="UnitNameTextBox" CssClass="listViewBox passcode edit" MaxLength="8" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" CssClass="listViewBox description edit" MaxLength="100" /></td>
                                    <td>
                                        <asp:Button runat="server" CommandName="Change" Text="Update" ID="UpdateButton" CommandArgument='<%# Eval("UnitId") %>' CssClass="btn-success" UseSubmitBehavior="False" />
                                        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" CssClass="btn-danger" UseSubmitBehavior="False" />
                                    </td>
                                </tr>
                            </EditItemTemplate>
                            <EmptyDataTemplate>
                                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                    <tr>
                                        <td>
                                            <div>No data was returned.</div>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #fff; color: #333333; border-bottom: 1px solid #dddddd">
                                    <td>
                                        <asp:DropDownList ID="SiteNameDDL" runat="server" DataSourceID="ActiveSiteNameDDL_ODS" DataTextField="SiteName"
                                            DataValueField="SiteId" SelectedValue='<%# Eval("SiteId") %>' Enabled="false" CssClass="hideDDL siteName" OnDataBound="SiteNameDDL_DataBound" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("UnitName") %>' runat="server" ID="UnitNameLabel" Enabled="false" ToolTip='<%# Eval("UnitName") %>' CssClass="listViewBox passcode" MaxLength="8" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" Enabled="false" ToolTip='<%# Eval("Description") %>' CssClass="listViewBox description" MaxLength="100" /></td>
                                    <td>
                                        <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" CommandArgument='<%# Eval("UnitId") %>' CssClass="btn-primary" UseSubmitBehavior="False" />
                                        <asp:Button runat="server" CommandName="Deactivate" Text="Deactivate" ID="DeactivateButton" CommandArgument='<%# Eval("UnitId") %>' CssClass="btn-danger" />
                                    </td>
                                    <!-- Controls for the confirm popup that appears when "Deactivate" button is pressed. -->
                                    <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="DeactivateButton"></ajaxToolkit:ConfirmButtonExtender>

                                    <ajaxToolkit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="DeactivateButton" OkControlID="btnYes"
                                        CancelControlID="btnNo" BackgroundCssClass="shadow">
                                    </ajaxToolkit:ModalPopupExtender>

                                    <!-- Popup that appears when deactivate is clicked. -->
                                    <asp:Panel ID="pnlPopup" runat="server" CssClass="deactivate-popup" Style="display: none">
                                        <div class="deactivate-popup-header panel-heading">
                                            <h3>Confirm Deactivation</h3>
                                        </div>
                                        <div class="deactivate-popup-body panel-body">
                                            <p>Are you sure you want to deactivate the <%# Eval("UnitName") %> unit?</p>
                                            <p>This cannot be undone.</p>

                                            <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="btn btn-success" />
                                            <asp:Button ID="btnNo" runat="server" Text="No" CssClass="btn btn-danger" />
                                        </div>
                                    </asp:Panel>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                    <th runat="server">Site Name</th>
                                                    <th runat="server">Unit Name</th>
                                                    <th runat="server">Description</th>
                                                    <th runat="server"></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar" style="text-align: center; background-color: #468CC8; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                            <asp:DataPager runat="server" ID="ActiveDataPager">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous" NextPreviousButtonCssClass="btn-primary nextprev" NumericButtonCssClass="btn-primary footer-buttons" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                </div>

                <!-- This is the tab pane showing all the Deactivated units in the database.-->
                <div class="tab-pane fade" id="deactivated">

                    <!-- Deactivated Search Bar -->
                    <div class="searchBar">
                        <asp:Panel ID="DeactivatedDefaultButton" runat="server" DefaultButton="DeactivatedSearchButton">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="DeactivatedSearchBox" runat="server" placeholder="Search" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <asp:Label runat="server" ID="DeactivatedSearchLabel" Text="Search by:" CssClass="title-label"></asp:Label><br />
                                            <asp:RadioButtonList runat="server" ID="DeactivatedSearchBy" CssClass="radioButtonList">
                                                <asp:ListItem ID="DeactivatedAllCheckbox" runat="server" Text="All" Selected="True"></asp:ListItem>
                                                <asp:ListItem ID="DeactivatedSiteNameCheckbox" runat="server" Text="Site Name"></asp:ListItem>
                                                <asp:ListItem ID="DeactivatedUnitNameCheckbox" runat="server" Text="Unit Name"></asp:ListItem>
                                                <asp:ListItem ID="DeactivatedDescriptionCheckbox" runat="server" Text="Description"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr />
                                        <asp:Button ID="DeactivatedSearchButton" runat="server" OnClick="DeactivatedSearchButton_Click" Text="Search" class="btn btn-primary" />
                                        <asp:Button ID="DeactivatedClearButton" runat="server" OnClick="DeactivatedClearButton_Click" Text="Clear" class="btn btn-danger" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>

                    <!-- Deactivated listview -->
                    <div class="ListView rounded_corners">
                        <asp:ListView ID="DeactivatedUnitListView" runat="server" DataSourceID="DeactivatedUnitListView_ODS">
                            <EmptyDataTemplate>
                                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                    <tr>
                                        <td>
                                            <div>No data was returned.</div>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #fff; color: #333333;">
                                    <td>
                                        <asp:DropDownList ID="AllSiteNameDDL" runat="server" DataSourceID="AllSiteNameDDL_ODS" DataTextField="SiteName"
                                            DataValueField="SiteId" SelectedValue='<%# Eval("SiteId") %>' Enabled="false" CssClass="hideDDL inactiveSiteName" OnDataBound="SiteNameDDL_DataBound" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("UnitName") %>' runat="server" ID="UnitNameLabel" Enabled="false" ToolTip='<%# Eval("UnitName") %>' CssClass="listViewBox inactivePasscode" MaxLength="8" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" Enabled="false" ToolTip='<%# Eval("Description") %>' CssClass="listViewBox inactiveDescription" MaxLength="100" /></td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                    <th runat="server">SiteName</th>
                                                    <th runat="server">UnitName</th>
                                                    <th runat="server">Description</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar" style="text-align: center; background-color: #468CC8; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                            <asp:DataPager runat="server" ID="DeactivatedDataPager">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous" NextPreviousButtonCssClass="btn-primary nextprev" NumericButtonCssClass="btn-primary footer-buttons" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                </div>

                <!-- This is the tab holding the add new unit functionality.-->
                <div class="tab-pane" id="add">
                    <div class="addBar">
                        <asp:Panel ID="AddPanelDefaultButton" runat="server" DefaultButton="AddButton">
                            <table class="add-table">
                                <tr>
                                    <td>
                                        <asp:Label ID="AddUnitSiteLabel" runat="server" Text="Site Name" CssClass="add-label"></asp:Label>
                                        <br />
                                        <asp:DropDownList ID="AddSiteNameDDL" runat="server" DataSourceID="ActiveSiteNameDDL_ODS" DataTextField="SiteName"
                                            DataValueField="SiteId" CssClass="listViewDDL form-control" OnDataBound="SiteNameDDL_DataBound" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="AddUnitNameLabel" runat="server" Text="Unit Name" CssClass="add-label"></asp:Label>

                                        <asp:TextBox ID="AddUnitName" runat="server" CssClass="form-control" placeholder="Unit Name" MaxLength="8"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="vertical-align: top;">

                                    <td>
                                        <asp:Label ID="AddDescriptionLabel" runat="server" Text="Description" CssClass="add-label"></asp:Label>

                                        <asp:TextBox ID="AddUnitDescription" runat="server" CssClass="form-control multi" placeholder="Description" TextMode="MultiLine" MaxLength="8"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <asp:Button runat="server" Text="Add" ID="AddButton" CssClass="btn btn-success" UseSubmitBehavior="False" OnClick="AddUnitButton_Click" />
                                        <asp:Button runat="server" Text="Clear" ID="CancelButton" CssClass="btn btn-danger" UseSubmitBehavior="False" OnClick="AddClearButton_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div class="ListView rounded_corners">
                        <asp:ListView ID="AddUnitListView" runat="server" DataSourceID="ActiveUnitListView_ODS">
                            <EmptyDataTemplate>
                                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                    <tr>
                                        <td>
                                            <div>No data was returned.</div>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #FFF; color: #333333; border-bottom: 1px solid #dddddd">
                                    <td>
                                        <asp:DropDownList ID="SiteNameDDL" runat="server" DataSourceID="ActiveSiteNameDDL_ODS" DataTextField="SiteName"
                                            DataValueField="SiteId" SelectedValue='<%# Eval("SiteId") %>' Enabled="false" CssClass="hideDDL addTabSiteName" OnDataBound="SiteNameDDL_DataBound" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("UnitName") %>' runat="server" ID="UnitNameLabel" ToolTip='<%# Eval("UnitName") %>' Enabled="false" CssClass="listViewBox addTabPasscode" MaxLength="8" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" Enabled="False" ToolTip='<%# Eval("Description") %>' CssClass="listViewBox addTabDescription" MaxLength="100" /></td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                    <th runat="server">Site Name</th>
                                                    <th runat="server">Unit Name</th>
                                                    <th runat="server">Description</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar" style="text-align: center; background-color: #468CC8; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                            <asp:DataPager runat="server" ID="AddDataPager">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous" NextPreviousButtonCssClass="btn-primary nextprev" NumericButtonCssClass="btn-primary footer-buttons" />
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

    <div id="ods-div">
        <!-- This ODS is for the Active units -->
        <asp:ObjectDataSource ID="ActiveUnitListView_ODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Unit_Search" TypeName="MSSSystem.BLL.UnitController">
            <SelectParameters>
                <asp:Parameter DefaultValue="false" Name="deactivated" Type="Boolean"></asp:Parameter>
                <asp:ControlParameter ControlID="UserSiteId" PropertyName="Text" DefaultValue="0" Name="siteId" Type="Int32"></asp:ControlParameter>
                <asp:Parameter DefaultValue="" Name="searchArg" Type="String"></asp:Parameter>
                <asp:Parameter DefaultValue="" Name="searchBy" Type="String"></asp:Parameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <!-- This ODS is for the deactivated units -->
        <asp:ObjectDataSource ID="DeactivatedUnitListView_ODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Unit_Search" TypeName="MSSSystem.BLL.UnitController">
            <SelectParameters>
                <asp:Parameter DefaultValue="true" Name="deactivated" Type="Boolean"></asp:Parameter>
                <asp:ControlParameter ControlID="UserSiteId" PropertyName="Text" DefaultValue="0" Name="siteId" Type="Int32"></asp:ControlParameter>
                <asp:Parameter Name="searchArg" Type="String"></asp:Parameter>
                <asp:Parameter Name="searchBy" Type="String"></asp:Parameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <%-- List view used for searching on the ActiveListView to enable edit to single out records --%>
        <asp:ObjectDataSource ID="SearchListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Unit_Search" TypeName="MSSSystem.BLL.UnitController">
            <SelectParameters>
                <asp:ControlParameter ControlID="deactivatedSearch" PropertyName="Value" DefaultValue="false" Name="deactivated" Type="Boolean"></asp:ControlParameter>
                <asp:ControlParameter ControlID="searchArg" PropertyName="Value" DefaultValue="" Name="searchArg" Type="String"></asp:ControlParameter>
                <asp:ControlParameter ControlID="searchBy" PropertyName="Value" DefaultValue="Site Name" Name="searchBy" Type="String"></asp:ControlParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="searchArg" runat="server" Value="" />
        <asp:HiddenField ID="searchBy" runat="server" Value="" />
        <asp:HiddenField ID="deactivatedSearch" runat="server" Value="" />
        <!-- This ODS is for the ActiveSiteName drop down lists -->
        <asp:ObjectDataSource ID="ActiveSiteNameDDL_ODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
            <SelectParameters>
                <asp:Parameter DefaultValue="false" Name="deactivated" Type="Boolean"></asp:Parameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <!-- This ODS is for the AllSiteName drop down lists -->
        <asp:ObjectDataSource ID="AllSiteNameDDL_ODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List_All" TypeName="MSSSystem.BLL.SiteController" />
    </div>

    <script>
        <%-- This script prevents rapid clicking of buttons on the page that can cause loading errors --%>
        $(document).ready(function () {
            $("input[type=button]").click(function () {
                var el = $(this)
                el.prop('deactivated', true);
                setTimeout(function () { el.prop('deactivated', false); }, 1000);
            });
        });

        <%-- This script will keep the tab panel the same on hitting a search or clear button --%>
        if (location.hash) {
            $('a[href=\'' + location.hash + '\']').tab('show');
        }
        var activeTab = localStorage.getItem('activeTab');
        if (activeTab) {
            $('a[href="' + activeTab + '"]').tab('show');
        }

        $('body').on('click', 'a[data-toggle=\'tab\']', function (e) {
            e.preventDefault()
            var tab_name = this.getAttribute('href')
            if (history.pushState) {
                history.pushState(null, null, tab_name)
            }
            else {
                location.hash = tab_name
            }
            localStorage.setItem('activeTab', tab_name)

            $(this).tab('show');
            return false;
        });
        $(window).on('popstate', function () {
            var anchor = location.hash ||
              $('a[data-toggle=\'tab\']').first().attr('href');
            $('a[href=\'' + anchor + '\']').tab('show');
        });
    </script>
</asp:Content>
