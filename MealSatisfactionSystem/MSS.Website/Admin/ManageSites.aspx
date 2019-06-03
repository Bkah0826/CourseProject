<%@ Page Title="Manage Sites" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageSites.aspx.cs" Inherits="AdminPortal_ManageSites" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="ManageSites" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" type="text/css" href="../Content/ManageSites.css">    
        <h1>Manage Sites</h1>
        <p>Add, update or deactivate a site or lookup deactivated sites.</p>       
    <hr />
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <div class="row">
        <div>
            <!-- Navigation Tabs -->
            <ul class="nav nav-tabs">
                <li class="active"><a href="#active" data-toggle="tab">Update Site</a></li>                
                <li><a href="#add" data-toggle="tab">Add Site</a></li>
                <li><a href="#deactivated" data-toggle="tab">View Inactive Sites</a></li>
            </ul>
            <!-- Tab Content Area -->
            <div class="tab-content clearfix">
                <!-- This is the tab holding the listview showing all the Active units in the database.-->
                <div class="tab-pane fade in active" id="active">
                    <!-- Search Bar -->
                    <div class="searchBar">
                         <asp:Panel ID="ActiveSearchPanel" runat="server" DefaultButton="ActiveSearchButton">
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
                                            <asp:Label runat="server" ID="ActiveSearchTextBox" Text="Search by:" CssClass="title-label"></asp:Label><br />
                                            <asp:RadioButtonList runat="server" ID="ActiveSearchBy" CssClass="radioButtonList">
                                                <asp:ListItem ID="ActiveSearchAllCheckbox" runat="server" Text="All" Selected="True"></asp:ListItem>
                                                <asp:ListItem ID="ActiveSiteNameCheckbox" runat="server" Text="Site Name"></asp:ListItem>
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
                    <div class="ListView rounded_corners">
                        <asp:ListView ID="ActiveSiteListView" runat="server" DataKeyNames="SiteId" OnItemCommand="SiteListView_ItemCommand"
                             OnItemCanceling="ActiveSiteListView_ItemCanceling" OnItemDataBound="ActiveSiteListView_ItemDataBound" OnItemEditing="ActiveSiteListView_ItemEditing" 
                            DataSourceID="ActiveSiteListODS">
                            <EditItemTemplate>
                                <tr style="margin-bottom:58px">
                                    <td>
                                        <asp:TextBox Text='<%# Bind("SiteName") %>' runat="server" ID="SiteNameTextBox" CssClass="listViewBox siteName edit" MaxLength="50"/></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" CssClass="listViewBox description edit" MaxLength="100" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Passcode") %>' runat="server" ID="PasscodeTextBox" CssClass="listViewBox passcode edit" MaxLength="15" AutoComplete="off"/></td>
                                    <td>
                                        <asp:Button runat="server" CommandName="Change" Text="Update" ID="UpdateButton" CommandArgument='<%# Eval("SiteId") %>' CssClass="btn-success" UseSubmitBehavior="False" />
                                        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" CssClass="btn-danger" UseSubmitBehavior="False" />
                                    </td>
                                </tr>
                            </EditItemTemplate>
                            <EmptyDataTemplate>
                                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                    <tr>
                                        <td>No data was returned.</td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #FFF; color: #333333; border-bottom: 1px solid #dddddd">
                                    <td>
                                        <asp:TextBox Text='<%# Bind("SiteName") %>' runat="server" ID="SiteNameTextBox" Enabled="False" ToolTip='<%# Eval("SiteName") %>' CssClass="listViewBox siteName" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" Enabled="False" ToolTip='<%# Eval("Description") %>' CssClass="listViewBox description" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Passcode") %>' runat="server" ID="PasscodeTextBox" Enabled="False" ToolTip='<%# Eval("Passcode") %>' CssClass="listViewBox passcode" /></td>
                                    <td>
                                        <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" CommandArgument='<%# Eval("SiteId") %>' CssClass="btn-primary" UseSubmitBehavior="False" />
                                        <asp:Button runat="server" CommandName="Deactivate" Text="Deactivate" ID="DeactivateButton" CommandArgument='<%# Eval("SiteId") %>' CssClass="btn-danger" />                                        
                                    </td>
                                    <!-- Controls for the confirm popup that appears when "Deactivate" button is pressed. -->
                                    <AjaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="DeactivateButton">
                                    </AjaxToolkit:ConfirmButtonExtender>

                                    <AjaxToolKit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="DeactivateButton" OkControlID="btnYes"
                                        CancelControlID="btnNo" BackgroundCssClass="shadow">
                                    </AjaxToolKit:ModalPopupExtender>

                                    <!-- Popup that appears when deactivate is clicked. -->
                                    <asp:Panel ID="pnlPopup" runat="server" CssClass="deactivate-popup" Style="display: none">
                                        <div class="deactivate-popup-header panel-heading">
                                            <h3>Confirm Deactivation</h3>
                                        </div>
                                        <div class="deactivate-popup-body panel-body">
                                            <p>Are you sure you want to deactivate the <%# Eval("SiteName") %> site?</p>
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
                                                    <th runat="server">Description</th>
                                                    <th runat="server">Passcode</th>
                                                    <th runat="server"></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar" style="text-align: center; background-color: #468CC8; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF" >
                                            <asp:DataPager runat="server" ID="ActiveDataPager" >
                                                <Fields>                                                   
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous" NextPreviousButtonCssClass="btn-primary nextprev"  NumericButtonCssClass="btn-primary footer-buttons" />                                                    
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>
                    </div>
                    <!-- Enables a button that calls the passcode generation method that is used by the site to automatically change passcodes 
                        <asp:Button runat="server" ID="PasscodeButton" OnClick="PasscodeButton_Click" Text="Get Passcode" />-->
                </div>

                <!-- This is the tab holding the listview showing all the Deactivated units in the database. -->
                <div class="tab-pane fade" id="deactivated">

                    <!-- Search Bar -->
                    <div class="searchBar">
                        <asp:Panel ID="DeactivatedSearchPanel" runat="server" DefaultButton="DeactivatedSearchButton">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="DeactivatedSearchBox" runat="server" placeholder="Search" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="">
                                            <asp:Label runat="server" ID="DeactivatedSearchTextBox" Text="Search by:" CssClass="title-label"></asp:Label><br />
                                            <asp:RadioButtonList runat="server" ID="DeactivatedSearchBy" CssClass="radioButtonList">
                                                <asp:ListItem ID="DeactivatedSearchAllCheckbox" runat="server" Text="All" Selected="True"></asp:ListItem>
                                                <asp:ListItem ID="DeactivatedSiteNameCheckbox" runat="server" Text="Site Name"></asp:ListItem>
                                                <asp:ListItem ID="DeactivatedDescriptionCheckbox" runat="server" Text="Description"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr />
                                        <asp:Button ID="DeactivatedSearchButton" runat="server" OnClick="DeactivatedSearchButton_Click" Text="Search" class="btn btn-primary" UseSubmitBehavior="False" />
                                        <asp:Button ID="DeactivatedClearButton" runat="server" OnClick="DeactivatedClearButton_Click" Text="Clear" class="btn btn-danger" UseSubmitBehavior="False" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>

                    <div class="ListView rounded_corners">
                        <asp:ListView ID="DeactivatedSiteListView" runat="server" DataSourceID="DeactivatedSiteListODS">
                            <EmptyDataTemplate>
                                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                    <tr>
                                        <td><asp:Label ID="noDataLabel" runat="server" Text="No data was returned"></asp:Label></td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #FFF; color: #333333; border-bottom: 1px solid #dddddd"">
                                    <td>
                                        <asp:TextBox Text='<%# Bind("SiteName") %>' runat="server" ID="SiteNameTextBox" Enabled="False" ToolTip='<%# Eval("SiteName") %>' CssClass="listViewBox inactiveSiteName" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" Enabled="False" ToolTip='<%# Eval("Description") %>' CssClass="listViewBox inactiveDescription" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Passcode") %>' runat="server" ID="PasscodeTextBox" Enabled="False" ToolTip='<%# Eval("Passcode") %>' CssClass="listViewBox inactivePasscode" /></td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                    <th runat="server">Site Name</th>
                                                    <th runat="server">Description</th>
                                                    <th runat="server">Passcode</th>
                                                    <th runat="server"></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar" style="text-align: center; background-color: #468CC8; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                            <asp:DataPager runat="server" ID="DeactivatedDataPager">
                                                <Fields>                                                  
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous" NextPreviousButtonCssClass="btn-primary nextprev"  NumericButtonCssClass="btn-primary footer-buttons" />  
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                        </asp:ListView>

                    </div>
                </div>

                <!-- This is the tab holding the add new site functionality.-->
                <div class="tab-pane" id="add">                   
                    <div class="addBar">
                        <asp:Panel ID="AddPanel" runat="server" DefaultButton="AddButton">
                            <table class="add-table">
                                <tr>
                                    <td>
                                        <asp:Label ID="AddSiteNameLabel" runat="server" Text="Site Name" CssClass="add-label"></asp:Label>
                                
                                
                                        <asp:TextBox ID="AddSiteNameTextBox" runat="server" CssClass="form-control" placeholder="Site Name" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="vertical-align:top;">
                                
                                    <td>
                                        <asp:Label ID="AddDescriptionLabel" runat="server" Text="Description" CssClass="add-label"></asp:Label>
                                
                                        <asp:TextBox ID="AddDescriptionTextBox" runat="server" CssClass="form-control multi" placeholder="Description"  TextMode="MultiLine" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="AddPasscodeLabel" runat="server" Text="Passcode" CssClass="add-label"></asp:Label>
                                
                                        <asp:TextBox ID="AddPasscodeTextBox" runat="server" CssClass="form-control" placeholder="Passcode" MaxLength="15"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td>
                                        <asp:Button runat="server" Text="Add" ID="AddButton" CssClass="btn btn-success" UseSubmitBehavior="False" OnClick="AddButton_Click" />
                                        <asp:Button runat="server" Text="Clear" ID="CancelButton" CssClass="btn btn-danger" UseSubmitBehavior="False" OnClick="CancelButton_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div class="ListView rounded_corners">
                      <asp:ListView ID="AddSiteListView" runat="server" DataSourceID="ActiveSiteListODS">
                            <EmptyDataTemplate>
                                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                                    <tr>
                                        <td><asp:Label ID="noDataLabel" runat="server" Text="No data was returned"></asp:Label></td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #FFF; color: #333333; border-bottom: 1px solid #dddddd">
                                    <td>
                                        <asp:TextBox Text='<%# Bind("SiteName") %>' runat="server" ID="SiteNameTextBox" Enabled="False" ToolTip='<%# Eval("SiteName") %>' CssClass="listViewBox addTabSiteName" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" Enabled="False" ToolTip='<%# Eval("Description") %>' CssClass="listViewBox addTabDescription" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("Passcode") %>' runat="server" ID="PasscodeTextBox" Enabled="False" ToolTip='<%# Eval("Passcode") %>' CssClass="listViewBox addTabPasscode" /></td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                    <th runat="server">Site Name</th>
                                                    <th runat="server">Description</th>
                                                    <th runat="server">Passcode</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar" style="text-align: center; background-color: #468CC8; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                                            <asp:DataPager runat="server" ID="AddDataPager">
                                                <Fields>
                                                    <%--<asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" ButtonCssClass="btn-primary footer-buttons"></asp:NextPreviousPagerField>--%>
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous" NextPreviousButtonCssClass="btn-primary footer-buttons"  NumericButtonCssClass="btn-primary footer-buttons" />  
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

    <!--ODS's-->
    <div id="ods-div">
        <asp:ObjectDataSource ID="ActiveSiteListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
            <SelectParameters>
                <asp:Parameter DefaultValue="false" Name="deactivated" Type="Boolean"></asp:Parameter>
            </SelectParameters>
        </asp:ObjectDataSource>

        <asp:ObjectDataSource ID="DeactivatedSiteListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
            <SelectParameters>
                <asp:Parameter DefaultValue="true" Name="deactivated" Type="Boolean"></asp:Parameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <%-- List view used for searching on the ActiveListView to enable edit to single out records --%>
        <asp:ObjectDataSource ID="SearchListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
            <SelectParameters>
                <asp:ControlParameter ControlID="deactivatedSearch" PropertyName="Value" DefaultValue="false" Name="deactivated" Type="Boolean"></asp:ControlParameter>
                <asp:ControlParameter ControlID="searchArg" PropertyName="Value" DefaultValue="" Name="searchArg" Type="String"></asp:ControlParameter>
                <asp:ControlParameter ControlID="searchBy" PropertyName="Value" DefaultValue="Site Name" Name="searchBy" Type="String"></asp:ControlParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="searchArg" runat="server" Value=""/>
        <asp:HiddenField ID="searchBy" runat="server" Value=""/>
        <asp:HiddenField ID="deactivatedSearch" runat="server" Value=""/>
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

         <%-- This script will keep the selected tab panel the same on hitting a search or clear button --%>
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

