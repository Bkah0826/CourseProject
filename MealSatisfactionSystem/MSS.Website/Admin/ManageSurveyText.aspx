<%@ Page Title="Manage Survey Text" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageSurveyText.aspx.cs" Inherits="Admin_ManageSurveyText" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="ManageSurveyText" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" type="text/css" href="../Content/ManageSites.css">
    <h1>Manage Survey Text</h1>
    <p>Update the survey form's questions, subquestions, and answers</p>
    <hr />
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <div class="row">
        <!-- Navigation Tabs -->
        <ul class="nav nav-tabs">
            <li class="active"><a href="#questions" data-toggle="tab">Update Questions</a></li>
            <li><a href="#subquestions" data-toggle="tab">Update Subquestions</a></li>
            <li><a href="#answers" data-toggle="tab">Update Answers</a></li>
        </ul>
        <!-- Tab Content Area -->
        <div class="tab-content clearfix" style="padding:30px 10px">
            <%-- Update Questions Tab --%>
            <div class="tab-pane fade in active" id="questions">
                <div class="ListView rounded_corners">
                    <asp:ListView ID="UpdateQuestionsListView" runat="server"
                        DataSourceID="QuestionsListODS"
                        OnItemCommand="UpdateQuestionsListView_ItemCommand"
                        OnItemEditing="UpdateQuestionsListView_ItemEditing"
                        OnItemDataBound="UpdateQuestionsListView_ItemDataBound">
                        <EditItemTemplate>
                            <tr>
                                <td>
                                    <asp:TextBox Text='<%# Bind("questionText") %>' runat="server" ID="questionTextTextBox" CssClass="listViewBox question edit" MaxLength="300"/></td>
                                <td>
                                    <asp:Repeater ID="QuestionIdListRepeater" runat="server"
                                        DataSource='<%# Eval("questionIds") %>'
                                        ItemType="MSS.Data.POCOs.QuestionPOCO"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label Text="<%# Item.questionId %>" runat="server" ID="questionIdLabel" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Button runat="server" CommandName="Change" Text="Update" CssClass="btn-success" ID="UpdateButton"/>
                                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn-danger" ID="CancelButton"/>
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
                            <tr style="background-color: #FFF; color: #333333;">
                                <td>
                                    <asp:TextBox Text='<%# Eval("questionText") %>' runat="server" ID="questionTextLabel" Enabled="false" CssClass="listViewBox question" /></td>
                                <td>
                                    <asp:Repeater ID="QuestionIdListRepeater" runat="server"
                                        DataSource='<%# Eval("questionIds") %>'
                                        ItemType="MSS.Data.POCOs.QuestionPOCO"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label Text="<%# Item.questionId %>" runat="server" ID="questionIdLabel" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" CssClass="btn-primary dbl-wide-btn" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                            <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                <th runat="server">Question</th>
                                                <th runat="server"></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                    <tr runat="server">
                                    <td runat="server">
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


            <%-- Update Subquestions Tab --%>
            <div class="tab-pane fade in" id="subquestions">
                    
                <%-- Question Selection and Subquestion Fetching Panel --%>
                <div class="col-md-12" style="border-bottom:1px solid #ddd">
                    <asp:Panel ID="SelectQuestionsForSubquestionsPanel" runat="server" DefaultButton="FetchSubQuestionsButton">
                        <div class="form-horizontal">
                            <div class="fieldset">
                                <div class="form-group">
                                    <asp:Label ID="QuestionsWithSubQuestionsLabel" runat="server"
                                        Text="Question" CssClass="control-label col-md-1 text-left"
                                        AssociatedControlID="QuestionsWithSubQuestionsDropDownList"/>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="QuestionsWithSubQuestionsDropDownList" runat="server"
                                            DataSourceID="QuestionsWithSubQuestionsListODS"
                                            DataTextField="questionText"
                                            DataValueField="questionText"
                                            AppendDataBoundItems="true"
                                            CssClass="form-control col-md-11"
                                            Style="min-width:100%; margin-bottom: 20px;">
                                            <asp:ListItem Value="0" Selected="True" Text="Select a question..."></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-12 text-center">
                                        <asp:Button ID="FetchSubQuestionsButton" runat="server" Text="Fetch Subquestions" OnClick="FetchSubQuestionsButton_Click" CssClass="btn btn-primary"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="ListView rounded_corners">
                    <asp:ListView ID="UpdateSubQuestionsListView" runat="server"
                        DataSourceID="SubQuestionListODS"
                        OnItemCommand="UpdateSubQuestionsListView_ItemCommand"
                        OnItemEditing="UpdateSubQuestionsListView_ItemEditing"
                        OnItemDataBound="UpdateSubQuestionsListView_ItemDataBound">
                        <EditItemTemplate>
                            <tr>
                                <td>
                                    <asp:TextBox Text='<%# Bind("SubQuestionText") %>' runat="server" ID="SubQuestionTextTextBox" CssClass="listViewBox subquestion edit" MaxLength="150"/></td>
                                <td>
                                    <asp:Button runat="server" CommandName="Change" Text="Update" ID="UpdateButton" CssClass="btn-success" CommandArgument='<%# Eval("QuestionId") %>' />
                                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" CssClass="btn-danger" />
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
                            <tr style="background-color: #FFF; color: #333333;">
                                <td>
                                    <asp:TextBox Text='<%# Eval("SubQuestionText") %>' runat="server" ID="SubQuestionTextLabel" Enabled="false" CssClass="listViewBox subquestion" /></td>
                                <td>
                                    <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" CssClass="btn-primary dbl-wide-btn" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                            <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                <th runat="server">Subquestion</th>
                                                <th runat="server"></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                    <tr runat="server">
                                    <td runat="server">
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

            <%-- Update Answers Tab --%>
            <div class="tab-pane fade in" id="answers">
                <%-- Question Selection and Answer Fetching Panel --%>
                <div class="col-md-12" style="border-bottom:1px solid #ddd">
                    <asp:Panel ID="SelectQuestionsForAnswersPanel" runat="server" DefaultButton="FetchAnswersButton">
                        <div class="form-horizontal">
                            <div class="fieldset">
                                <div class="form-group">
                                    <asp:Label ID="QuestionWithAnswersLabel" runat="server"
                                        Text="Question" CssClass="control-label col-md-1 text-left"
                                        AssociatedControlID="QuestionsWithAnswersDropDownList"/>
                                    <div class="col-md-10">
                                        <asp:DropDownList ID="QuestionsWithAnswersDropDownList" runat="server"
                                            DataSourceID="QuestionsWithAnswersListODS"
                                            DataTextField="questionText"
                                            DataValueField="questionText"
                                            AppendDataBoundItems="true"
                                            CssClass="form-control col-md-11"
                                            Style="min-width:100%; margin-bottom: 20px;">
                                            <asp:ListItem Value="0" Selected="True" Text="Select a question..."></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-12 text-center">
                                        <asp:Button ID="FetchAnswersButton" runat="server" Text="Fetch Answers" OnClick="FetchAnswersButton_Click" CssClass="btn btn-primary"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="ListView rounded_corners">
                    <asp:ListView ID="UpdateAnswersListView" runat="server"
                        DataSourceID="AnswersListODS"
                        OnItemCommand="UpdateAnswersListView_ItemCommand"
                        OnItemEditing="UpdateAnswersListView_ItemEditing"
                        OnItemDataBound="UpdateAnswersListView_ItemDataBound">
                        <EditItemTemplate>
                            <tr>
                                <td>
                                    <asp:TextBox Text='<%# Bind("description") %>' runat="server" ID="descriptionTextBox" CssClass="listViewBox answer edit" MaxLength="50" />
                                </td>
                                <td>
                                    <asp:Button runat="server" CommandName="Change" Text="Update" ID="UpdateButton" CssClass="btn-success" CommandArgument='<%# Eval("answerId") %>'/>
                                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" CssClass="btn-danger"/>
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
                            <tr style="background-color: #FFF; color: #333333;">
                                <td>
                                    <asp:TextBox Text='<%# Eval("description") %>' runat="server" ID="descriptionLabel" Enabled="false" CssClass="listViewBox answer" /></td>
                                <td>
                                    <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" CssClass="btn-primary dbl-wide-btn" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                            <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                                <th runat="server">Answers</th>
                                                <th runat="server"></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server">
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
        </div>
    </div>

    <asp:ObjectDataSource runat="server" ID="QuestionsListODS"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="Questions_List"
        TypeName="MSSSystem.BLL.QuestionController"/>

    <asp:ObjectDataSource ID="QuestionsWithSubQuestionsListODS" runat="server"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="QuestionsWithSubQuestions_List"
        TypeName="MSSSystem.BLL.QuestionController"/>

    <asp:ObjectDataSource ID="QuestionsWithAnswersListODS" runat="server"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="QuestionsWithAnswers_List"
        TypeName="MSSSystem.BLL.QuestionController"/>

    <asp:ObjectDataSource ID="SubQuestionListODS" runat="server"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="SubQuestionsByQuestion_List"
        TypeName="MSSSystem.BLL.QuestionController">
        <SelectParameters>
            <asp:ControlParameter ControlID="QuestionsWithSubQuestionsDropDownList" PropertyName="SelectedValue" DefaultValue="0" Name="questionText" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="AnswersListODS" runat="server"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="AnswersByQuestion_List"
        TypeName="MSSSystem.BLL.AnswerController"
        OnUpdated="CheckForException">
        <SelectParameters>
            <asp:ControlParameter ControlID="QuestionsWithAnswersDropDownList" PropertyName="SelectedValue" DefaultValue="0" Name="questionText" Type="String"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>

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
