<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserRoleAdmin.aspx.cs" Inherits="Admin_Security_UserRoleAdmin" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" type="text/css" href="../../Content/ManageSites.css">
    <div>
        <h1>Manage Users</h1>
        <p>Add, update or deactivate a user or lookup deactivated users.</p>
        <hr />
    </div>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <div class="row">
        <div class="col-md-12">
            <!--script for tab to tab movement -->
            <ul class="nav nav-tabs ManageUsersTabs">
                <li class="active"><a href="#active" data-toggle="tab">Update User</a></li>
                <li><a href="#add" data-toggle="tab">Add User</a></li>
            </ul>
            <!--Tab panes one for each tab-->
            <div class="tab-content clearfix">
                <div class="tab-pane fade in active " id="active">
                    <br />
                    <script>
                        function nextButton(anchorRef) {
                            $('a[href="' + anchorRef + '"]').tab('show');
                        }
                    </script>
                    <div class="searchBar ManageUsersSearchBar">
                        <table>
                            <tr>
                                <td>
                                    <br /><br />
                                    <asp:TextBox ID="txtSearchMaster" runat="server" MaxLength="200" placeholder="Search" CssClass="ManageUserSearchTextbox form-control"></asp:TextBox>
                                    <hr />
                                    <h4 class="title-label">Filters</h4>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <asp:Label runat="server" ID="Label1" Text="Site" CssClass="title-label"></asp:Label><br />
                                        <div class="ManageUsersScrollDiv" id="SiteScrollDiv" runat="server">
                                            <asp:CheckBoxList ID="SiteCheckBoxList" runat="server" DataSourceID="SiteList" DataTextField="SiteName"
                                                ShowRoundedCorner="true" DataValueField="SiteId" SelectionMode="Multiple">
                                            </asp:CheckBoxList><br />
                                        </div>
                                        <br />
                                        <asp:Label runat="server" ID="Label2" Text="Role" CssClass="title-label"></asp:Label>
                                        <asp:CheckBoxList ID="RoleCheckBoxList" runat="server" DataSourceID="AllRoleNames"></asp:CheckBoxList><br />
                                        <asp:Label runat="server" ID="Label3" Text="Status" CssClass="title-label"></asp:Label><br />
                                        <asp:CheckBox ID="ActiveCheckbox" runat="server" Text="Active" Checked="false" /><br />
                                        <asp:CheckBox ID="InactiveCheckbox" runat="server" Text="Inactive" Checked="false" /><br />

                                        <asp:Button ID="btnSearch" OnClick="BtnSearch_Click" runat="server" Text="Search" class="btn btn-primary" />
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="BtnClear_Click" class="btn btn-danger" /><br>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="ListView ManageUsersListView rounded_corners">
                        <asp:ListView ID="UserListViewEdit" runat="server" OnItemEditing="Item_Edit" OnItemCanceling="Item_Cancel"
                            OnItemUpdating="Update_Command" OnPagePropertiesChanging="UserListViewEdit_PagePropertiesChanging">
                            <EditItemTemplate>
                                <tr>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("FirstName") %>' runat="server" ID="FirstNameLabel" placeholder="First Name" CssClass="listViewBox ManageUserTextbox ManageUserInsert" /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Bind("LastName") %>' runat="server" ID="LastNameLabel" placeholder="Last Name" CssClass="listViewBox ManageUserTextbox ManageUserInsert" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="SiteDropDownList" CssClass="ManageUserTextbox ManageUserInsert"
                                            runat="server" DataSourceID="SiteList" DataTextField="SiteName" DataValueField="SiteId">
                                        </asp:DropDownList>
                                    <td>
                                        <asp:Label Text='<%# Bind("UserName") %>' runat="server" ID="UserNameLabel" CssClass="ManageUserTextbox ManageUserInsert" /></td>
                                    <td>
                                        <asp:DropDownList ID="RoleMembershipsDropDown" runat="server" CssClass="ManageUserTextbox ManageUserInsert"
                                            DataSourceID="RoleNameODS">
                                        </asp:DropDownList></td>
                                    <td style="text-align:center">
                                        <asp:CheckBox CssClass="filled-in form-check-input" Checked='<%# Eval("Active") %>' runat="server" ID="disabledCheckBox" /></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="RequestedPasswordLabel" CssClass="listViewBox ManageUserTextbox ManageUserInsert" /></td>
                                    <td>
                                        <asp:Button ID="UpdateUser" runat="server" Text="Update"
                                            CommandName="update" class="btn-success" Style="border: none" Width="48%"></asp:Button>
                                        <asp:Button ID="CancelButton" runat="server" Text="Cancel"
                                            CommandName="Cancel" class="btn-danger" Style="border: none" Width="48%"></asp:Button></td>
                                </tr>
                            </EditItemTemplate>
                            <EmptyDataTemplate>
                                <table runat="server" class="ManageUsersEmptyItemTemplate">
                                    <tr>
                                        <td>No data was returned.</td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr class="ManageUsersItemTemplate">
                                    <td>
                                        <asp:TextBox Text='<%# Eval("FirstName") %>' Enabled="false" runat="server" ID="FirstNameLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("FirstName") %>' /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("LastName") %>' Enabled="false" runat="server" ID="LastNameLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("LastName") %>' />
                                    <td>
                                        <asp:TextBox Text='<%# Eval("Site") %>' Enabled="false" runat="server" ID="SiteLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("Site") %>' /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("UserName") %>' Enabled="false" runat="server" ID="UserNameLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("UserName") %>' /></td>
                                    <td>
                                        <asp:Repeater ID="RoleUserReapter" runat="server"
                                            DataSource='<%# Eval("RoleMemberships")%>'
                                            ItemType="System.String">
                                            <ItemTemplate>
                                                <asp:TextBox Enabled="false" ID="Role" CssClass="listViewBox ManageUserTextbox" runat="server" Text='<%# Item %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <SeparatorTemplate>, </SeparatorTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <td style="text-align:center">
                                        <asp:CheckBox CssClass="filled-in form-check-input  ManageUserTextbox" Checked='<%# Eval("Active") %>' runat="server" ID="disabledCheckBox" Enabled="false" Width="50%" /></td>
                                    <td>
                                        <asp:Label ID="PasswordLabel" runat="server" Text="******"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="LinkButton1" runat="server"
                                            CommandName="Edit" class="btn-primary" Style="border: none" Text="Edit" Width="100%"></asp:Button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" class="ManageUsersLayoutTemplate" border="1">
                                                <tr runat="server" class="ManageUsersLayoutTemplateTableRow">
                                                    <th runat="server">FirstName</th>
                                                    <th runat="server">LastName</th>
                                                    <th runat="server">Site</th>
                                                    <th runat="server">UserName</th>
                                                    <th runat="server">Role</th>
                                                    <th runat="server">Active</th>
                                                    <th runat="server">Password</th>
                                                    <th runat="server"></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar ManageUsersPager ManageUsersPagerButtonAlign">
                                            <asp:DataPager runat="server" ID="DataPager2">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous"
                                                        NextPreviousButtonCssClass="btn-primary ManageUsersFooterButtons nextprev" NumericButtonCssClass="btn-primary ManageUsersFooterButtons" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <SelectedItemTemplate>
                                <tr class="ManageUsersItemTemplate">
                                    <td>
                                        <asp:Label Text='<%# Eval("FirstName") + " " + Eval("LastName") %>' runat="server" ID="FirstNameLabel" CssClass="ManageUserTextbox" /></td>
                                    <td>
                                        <asp:Label Text='<%# Eval("Site") %>' runat="server" ID="SiteLabel" CssClass="ManageUserTextbox" /></td>
                                    <td>
                                        <asp:Label Text='<%# Eval("UserName") %>' runat="server" ID="UserNameLabel" CssClass="ManageUserTextbox" /></td>
                                    <td>
                                        <asp:Repeater ID="RoleUserReapter" runat="server"
                                            DataSource='<%# Eval("RoleMemberships")%>'
                                            ItemType="System.String">
                                            <ItemTemplate>
                                                <%# Item %>
                                            </ItemTemplate>
                                            <SeparatorTemplate>, </SeparatorTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <td style="text-align:center">
                                        <asp:CheckBox CssClass="filled-in form-check-input ManageUserTextbox" Checked='<%# Eval("disabled") %>' runat="server" ID="disabledCheckBox" Enabled="false" /></td>
                                    <td>
                                        <asp:Label Text='<%# Eval("RequestedPassword") %>' runat="server" ID="RequestedPasswordLabel" CssClass="ManageUserTextbox" /></td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
                <div class="tab-pane fade" id="add">
                    <br />
                    <div class="searchBar ManageUsersSearchBar">
                        <table class="add-table">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="FirstNameDescLabel" Text="First Name" CssClass="title-label"></asp:Label><br />
                                    <asp:TextBox Text='<%# Bind("FirstName") %>' MaxLength="35" runat="server" ID="FirstNameTextBox" CssClass="form-control ManageUserSearchTextbox" placeholder="First Name" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="LastNameDescLabel" Text="Last Name" CssClass="title-label"></asp:Label><br />
                                    <asp:TextBox Text='<%# Bind("LastName") %>' MaxLength="40" runat="server" ID="LastNameTextBox" CssClass="form-control ManageUserSearchTextbox" placeholder="Last Name" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Site" Text="Site" CssClass="title-label"></asp:Label><br />
                                    <asp:DropDownList ID="SiteAddDropDownList"
                                        runat="server" DataSourceID="SiteList" DataTextField="SiteName" DataValueField="SiteId" CssClass="listViewDDL form-control">
                                    </asp:DropDownList>
                                </td>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label5" Text="Username" CssClass="title-label"></asp:Label><br />
                                        <asp:TextBox Text='<%# Bind("UserName") %>' MaxLength="256" CssClass="listViewDDL form-control" runat="server" ID="UserNameLabel" placeholder="Username" />
                                    </td>
                                </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label6" Text="Role" CssClass="title-label"></asp:Label><br />
                                    <asp:DropDownList ID="RoleMemberships" runat="server" CssClass="listViewDDL form-control"
                                        DataSourceID="RoleNameODS">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="disabledCheckBox" runat="server" Text="Active" Checked="true" CssClass="filled-in form-check-input" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="Label8" Text="Password" CssClass="title-label"></asp:Label><br />
                                    <asp:TextBox CssClass="form-control ManageUserSearchTextbox" MaxLength="40" runat="server" ID="RequestedPasswordLabel" placeholder="Password" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="InsertUser" runat="server" Text="Add"
                                        CommandName="Save" OnCommand="InsertUser_Click" class="btn btn-success" Width="45%"></asp:Button>
                                    <asp:Button ID="CancelButton" runat="server" Text="Clear"
                                        CommandName="Cancel" class="btn btn-danger" Width="45%" OnCommand="CancelButton_Command"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="ListView ManageUsersListView rounded_corners">
                        <asp:ListView ID="UserListView" runat="server" OnPagePropertiesChanging="UserListView_PagePropertiesChanging" DataSourceID="UserListViewODS">
                            <EmptyDataTemplate>
                                <table runat="server" class="ManageUsersEmptyItemTemplate">
                                    <tr>
                                        <td>No data was returned.</td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <ItemTemplate>
                                <tr class="ManageUsersItemTemplate">
                                    <td>
                                        <asp:TextBox Text='<%# Eval("FirstName") %>' Enabled="false" runat="server" ID="FirstNameLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("FirstName") %>' /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("LastName") %>' Enabled="false" runat="server" ID="LastNameLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("LastName") %>' />
                                    <td>
                                        <asp:TextBox Text='<%# Eval("Site") %>' Enabled="false" runat="server" ID="SiteLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("Site") %>' /></td>
                                    <td>
                                        <asp:TextBox Text='<%# Eval("UserName") %>' Enabled="false" runat="server" ID="UserNameLabel" CssClass="listViewBox ManageUserTextbox" ToolTip='<%# Eval("UserName") %>' /></td>
                                    <td>
                                        <asp:Repeater ID="RoleUserReapter" runat="server"
                                            DataSource='<%# Eval("RoleMemberships")%>'
                                            ItemType="System.String">
                                            <ItemTemplate>
                                                <asp:TextBox Enabled="false" ID="Role" CssClass="ManageUserTextbox" runat="server" Text='<%# Item %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <SeparatorTemplate>, </SeparatorTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <td style="text-align:center">
                                        <asp:CheckBox CssClass="filled-in form-check-input  ManageUserTextbox" Checked='<%# Eval("Active") %>' runat="server" ID="disabledCheckBox" Enabled="false" Width="50%" /></td>
                                </tr>
                            </ItemTemplate>
                            <LayoutTemplate>
                                <table runat="server">
                                    <tr runat="server">
                                        <td runat="server">
                                            <table runat="server" id="itemPlaceholderContainer" class="ManageUsersLayoutTemplate"
                                                border="1">
                                                <tr runat="server" class="ManageUsersLayoutTemplateTableRow">
                                                    <th runat="server">FirstName</th>
                                                    <th runat="server">LastName</th>
                                                    <th runat="server">Site</th>
                                                    <th runat="server">UserName</th>
                                                    <th runat="server">Role</th>
                                                    <th runat="server">Active</th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder"></tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server">
                                        <td runat="server" class="footerBar ManageUsersPager ManageUsersPagerButtonAlign">
                                            <asp:DataPager runat="server" ID="DataPager2">
                                                <Fields>
                                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" NextPageText="Next" PreviousPageText="Previous"
                                                        NextPreviousButtonCssClass="btn-primary ManageUsersFooterButtons nextprev" NumericButtonCssClass="btn-primary ManageUsersFooterButtons" />
                                                </Fields>
                                            </asp:DataPager>
                                        </td>
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <SelectedItemTemplate>
                                <tr class="ManageUsersItemTemplate">
                                    <td>
                                        <asp:Label Text='<%# Eval("FirstName") + " " + Eval("LastName") %>' runat="server" ID="FirstNameLabel" CssClass="ManageUserTextbox" /></td>
                                    <td>
                                        <asp:Label Text='<%# Eval("Site") %>' runat="server" ID="SiteLabel" CssClass="ManageUserTextbox" /></td>
                                    <td>
                                        <asp:Label Text='<%# Eval("UserName") %>' runat="server" ID="UserNameLabel" CssClass="ManageUserTextbox" /></td>
                                    <td>
                                        <asp:Repeater ID="RoleUserReapter" runat="server"
                                            DataSource='<%# Eval("RoleMemberships")%>'
                                            ItemType="System.String">
                                            <ItemTemplate>
                                                <%# Item %>
                                            </ItemTemplate>
                                            <SeparatorTemplate>, </SeparatorTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <td>
                                        <asp:CheckBox CssClass="filled-in form-check-input ManageUserTextbox" Checked='<%# Eval("disabled") %>' runat="server" ID="disabledCheckBox" Enabled="false" /></td>
                                </tr>
                            </SelectedItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:ObjectDataSource ID="UserListViewODS" runat="server"
        SelectMethod="ListAllUsers"
        OldValuesParameterFormatString="original_{0}"
        TypeName="MSSSystem.BLL.Security.UserManager"
        OnSelected="CheckForException" DataObjectTypeName="MSS.Data.Entities.Security.UserProfile"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="SiteList" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Site_List" TypeName="MSSSystem.BLL.SiteController">
        <SelectParameters>
            <asp:Parameter DefaultValue="false" Name="deactivated" Type="Boolean"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="RoleNameODS" runat="server"
        SelectMethod="ListAllAdminRoles"
        OldValuesParameterFormatString="original_{0}"
        TypeName="MSSSystem.BLL.Security.RoleManager"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="AllRoleNames" runat="server"
        SelectMethod="ListAllRoleNames"
        OldValuesParameterFormatString="original_{0}"
        TypeName="MSSSystem.BLL.Security.RoleManager"></asp:ObjectDataSource>
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
