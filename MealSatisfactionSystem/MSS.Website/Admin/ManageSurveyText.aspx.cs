using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region AdditionalNamespaces
using MSS.Data.Entities.Security;
using MSSSystem.BLL;
#endregion

/// <summary>
/// Represents the ManageSurveyText Web Forms page.
/// </summary>
public partial class Admin_ManageSurveyText : System.Web.UI.Page
{
    /// <summary>
    /// Redirects any non-superuser users to login and also hides the UserControl message once the page loads.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Represents the base class for classes that contain event data, and provides a value to use for events that do not include event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Check and make sure the user is a SuperUser
        if (!User.IsInRole(SecurityRoles.SuperUser))
        {
            Response.Redirect("~/Account/Login.aspx");
        }
        //hide message user control
        MessageUserControl.Visible = false;
    }

    /// <summary>
    /// Allows the MessageUserControl to handle any exceptions thrown from violating the constraints within the entity classes.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the Selected, Inserted, Updated, and Deleted events of the ObjectDataSource control.</param>
    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.Visible = true;
        MessageUserControl.HandleDataBoundException(e);
    }

    #region Update Questions Methods
    /// <summary>
    /// Handles events that occur when a button in the Questions list view is clicked.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemCommand event, which occurs when a button in a ListView is clicked.</param>
    protected void UpdateQuestionsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        //hide message user control
        MessageUserControl.Visible = false;

        //If the Edit button is clicked, show informational message
        if (e.CommandName.Equals("Edit"))
        {
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfo("Edit Mode Active", "The question text in the selected row can now be edited.");
        }
        //If the Change button is clicked, do the following actions:
        else if (e.CommandName.Equals("Change"))
        {
            int i = e.Item.DisplayIndex;

            //Capture and store the question text on the selected index
            TextBox questionTextBox = UpdateQuestionsListView.Items[i].FindControl("questionTextTextBox") as TextBox;
            string questionText = questionTextBox.Text;

            //handle null values and white-space-only values
            if (string.IsNullOrEmpty(questionText) || string.IsNullOrWhiteSpace(questionText))
            {
                //show error message
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfoError("Processing Error", "Question is required.");

                //highlight the question textbox that handled the error
                questionTextBox.Focus();
            }
            //if user-entered value is not null or just a white space, do the the following actions:
            else
            {
                //find the question Id repeater that stores the question Ids associated with the edited question text
                Repeater questionIdRepeater = UpdateQuestionsListView.Items[i].FindControl("QuestionIdListRepeater") as Repeater;

                //loop through the question Id repeater
                foreach (RepeaterItem item in questionIdRepeater.Items)
                {
                    //capture the question Id
                    int questionId = int.Parse(((Label)item.FindControl("questionIdLabel")).Text);

                    MessageUserControl.TryRun(() =>
                    {   //check if user entered invalid values
                        Utility utility = new Utility();
                        utility.checkValidString(questionText);

                        //Update the selected question(s)
                        QuestionController sysmgr = new QuestionController();
                        sysmgr.Question_Update(questionText, questionId);

                        //turn off edit mode and refresh the listview
                        UpdateQuestionsListView.EditIndex = -1;
                        UpdateQuestionsListView.DataBind();
                    }, "Success", "Question has been updated.");
                }

                //show success/error message captured by message user control's try run method
                MessageUserControl.Visible = true;

                //reset update subquestions tab
                QuestionsWithSubQuestionsDropDownList.Items.Clear();
                QuestionsWithSubQuestionsDropDownList.Items.Add(new ListItem("Select a question...", "0"));
                QuestionsWithSubQuestionsDropDownList.DataBind();
                QuestionsWithSubQuestionsDropDownList.Enabled = true;
                FetchSubQuestionsButton.Enabled = true;

                //reset update answers tab
                QuestionsWithAnswersDropDownList.Items.Clear();
                QuestionsWithAnswersDropDownList.Items.Add(new ListItem("Select a question...", "0"));
                QuestionsWithAnswersDropDownList.DataBind();
                QuestionsWithAnswersDropDownList.Enabled = true;
                FetchAnswersButton.Enabled = true;

                //show datapager
                DataPager pager = UpdateQuestionsListView.FindControl("ActiveDataPager") as DataPager;
                pager.Visible = true;
            }
        }
        //If the Cancel button is clicked, show informational message
        else if (e.CommandName.Equals("Cancel"))
        {
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfo("Update Cancelled", "No changes to the selected question were saved.");

            //show datapager
            DataPager pager = UpdateQuestionsListView.FindControl("ActiveDataPager") as DataPager;
            pager.Visible = true;
        }
    }

    /// <summary>
    /// Handles events that occur when an edit operation is requested, but before the Update Questions ListView item is put in edit mode.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemEditing event, which occurs when an edit operation is requested but before the ListView item is put in edit mode.</param>
    protected void UpdateQuestionsListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        //hide message user control
        MessageUserControl.Visible = false;

        //hide datapager
        DataPager pager = UpdateQuestionsListView.FindControl("ActiveDataPager") as DataPager;
        pager.Visible = false;
    }

    /// <summary>
    /// Hides all other rows when the user enters edit mode to make editing questions easier.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemCreated and ItemDataBound events, which occur when an item is created and is bound to data in a ListView control.</param>
    protected void UpdateQuestionsListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        //hide message user control
        MessageUserControl.Visible = false;

        if (UpdateQuestionsListView.EditIndex != -1)
        {
            if (e.Item.DisplayIndex != UpdateQuestionsListView.EditIndex)
            {
                e.Item.Visible = false;
            }
        }
    }
    #endregion

    #region Update SubQuestions Methods
    /// <summary>
    /// Populates and updates the subquestions list view based on the selected question
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Represents the base class for classes that contain event data, and provides a value to use for events that do not include event data.</param>
    protected void FetchSubQuestionsButton_Click(object sender, EventArgs e)
    {
        //if user selects the "Select a question..." option in the dropdown list, do the following actions:
        if (QuestionsWithSubQuestionsDropDownList.SelectedValue == "0")
        {
            //show error message
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfoError("No Question Selected", "Please select a question.");

            //highlight the dropdown list, and refresh the Update Subquestions listview
            QuestionsWithSubQuestionsDropDownList.Focus();
            UpdateSubQuestionsListView.DataBind();
        }
        //if user selects a question from the dropdown list, show informational message
        else
        {
            MessageUserControl.ShowInfo("Subquestions Displayed", "Subquestions under the selected question are now displayed.");
            UpdateSubQuestionsListView.DataBind();
        }
    }

    /// <summary>
    /// Handles events that occur when a button in the Subquestions list view is clicked
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemCommand event, which occurs when a button in a ListView is clicked.</param>
    protected void UpdateSubQuestionsListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        //If the Edit button is clicked, do the following actions:
        if (e.CommandName.Equals("Edit"))
        {
            //show informational message
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfo("Edit Mode Active", "The subquestion text in the selected row can now be edited.");

            //disable drop-down list and fetch button to prevent editing of subquestions of other questions
            QuestionsWithSubQuestionsDropDownList.Enabled = false;
            FetchSubQuestionsButton.Enabled = false;
        }
        //If the Change button is clicked, do the following actions:
        else if (e.CommandName.Equals("Change"))
        {
            //capture the questionId associated to the selected subquestion
            int questionId = int.Parse(e.CommandArgument.ToString());

            //capture the display index
            int i = e.Item.DisplayIndex;

            //find the selected subquestion text box
            TextBox subQuestionTextBox = UpdateSubQuestionsListView.Items[i].FindControl("subQuestionTextTextBox") as TextBox;

            //capture the question text in the selected subquestion text box
            string subQuestionText = subQuestionTextBox.Text;

            //handle null values and white-space-only values
            if (string.IsNullOrEmpty(subQuestionText) || string.IsNullOrWhiteSpace(subQuestionText))
            {
                //show error message
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfoError("Processing Error", "Subquestion is required.");

                //highlight the subquestion row that caused the error
                subQuestionTextBox.Focus();
            }
            //if user-entered value is not null or just a white space, do the the following actions:
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    //check if user entered invalid values
                    Utility utility = new Utility();
                    utility.checkValidString(subQuestionText);

                    //update the subquestion text of the selected row
                    QuestionController sysmgr = new QuestionController();
                    sysmgr.SubQuestion_Update(questionId, subQuestionText);
                    UpdateSubQuestionsListView.DataBind();
                    UpdateSubQuestionsListView.EditIndex = -1;
                }, "Success", "Subquestion has been updated.");
            }

            //show success/error message
            MessageUserControl.Visible = true;

            //show datapager
            DataPager pager = UpdateSubQuestionsListView.FindControl("ActiveDataPager") as DataPager;
            pager.Visible = true;

            //enable drop-down list and fetch button to allow editing of other subquestions of other questions
            QuestionsWithSubQuestionsDropDownList.Enabled = true;
            FetchSubQuestionsButton.Enabled = true;
        }
        //If the Cancel button is clicked, do the following actions:
        else if (e.CommandName.Equals("Cancel"))
        {
            //show informational message
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfo("Update Cancelled", "No changes to the selected subquestion were saved.");

            //show datapager
            DataPager pager = UpdateSubQuestionsListView.FindControl("ActiveDataPager") as DataPager;
            pager.Visible = true;

            //enable drop-down list and fetch button to allow editing of other subquestions of other questions
            QuestionsWithSubQuestionsDropDownList.Enabled = true;
            FetchSubQuestionsButton.Enabled = true;
        }
    }

    /// <summary>
    /// Handles events that occur when an edit operation is requested, but before the Update Subquestions ListView item is put in edit mode.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemEditing event, which occurs when an edit operation is requested but before the ListView item is put in edit mode.</param>
    protected void UpdateSubQuestionsListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        //hide message user control
        MessageUserControl.Visible = false;

        //hide datapager
        DataPager pager = UpdateSubQuestionsListView.FindControl("ActiveDataPager") as DataPager;
        pager.Visible = false;
    }

    /// <summary>
    /// Hides all other rows when the user enters edit mode to make editing subquestions easier.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemCreated and ItemDataBound events, which occur when an item is created and is bound to data in a ListView control.</param>
    protected void UpdateSubQuestionsListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        //hide message user control
        MessageUserControl.Visible = false;

        if (UpdateSubQuestionsListView.EditIndex != -1)
        {
            if (e.Item.DisplayIndex != UpdateSubQuestionsListView.EditIndex)
            {
                e.Item.Visible = false;
            }
        }
    }
    #endregion

    #region Update Answers methods
    /// <summary>
    /// Populates and updates the subquestions list view based on the selected question
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Represents the base class for classes that contain event data, and provides a value to use for events that do not include event data.</param>
    protected void FetchAnswersButton_Click(object sender, EventArgs e)
    {   
        //if user selects the "Select a question..." option in the dropdown list, do the following actions:
        if (QuestionsWithAnswersDropDownList.SelectedValue == "0")
        {
            //show error message
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfoError("No Question Selected", "Please select a question.");

            //highlight the dropdown list and refresh the answers listview
            QuestionsWithAnswersDropDownList.Focus();
            UpdateAnswersListView.DataBind();
        }
        else
        {
            //show informational message
            MessageUserControl.ShowInfo("Answers Displayed", "Answers associated under the selected question are now displayed.");
            UpdateAnswersListView.DataBind();
        }
    }

    /// <summary>
    /// Handles the actions executed by command buttons found in the Answers list view
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemCommand event, which occurs when a button in a ListView is clicked.</param>
    protected void UpdateAnswersListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        //If the Edit button is clicked, do the following actions:
        if (e.CommandName.Equals("Edit"))
        {
            //show informational message
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfo("Edit Mode Active", "The answer text in the selected row can now be edited.");

            //disable drop-down list and fetch button to prevent editing of answers to other questions
            QuestionsWithAnswersDropDownList.Enabled = false;
            FetchAnswersButton.Enabled = false;
        }
        //If the Change button is clicked, do the following actions:
        else if (e.CommandName.Equals("Change"))
        {

            //capture the answer Id of the selected row
            int answerId = int.Parse(e.CommandArgument.ToString());
            //capture the row index of the selected row
            int i = e.Item.DisplayIndex;

            //find the answer textbox in the selected row
            TextBox answerTextBox = UpdateAnswersListView.Items[i].FindControl("descriptionTextBox") as TextBox;

            //capture the answer text from the textbox
            string answerText = answerTextBox.Text;

            //handle null values and white-space-only values
            if (string.IsNullOrEmpty(answerText) || string.IsNullOrWhiteSpace(answerText))
            {
                //show error message
                MessageUserControl.Visible = true;
                MessageUserControl.ShowInfoError("Processing Error", "Answer is required.");

                //highlight the answer textbox in the row that caused the error
                answerTextBox.Focus();
            }
            //if user-entered value is not null or just a white space, do the the following actions:
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    //check if user entered invalid values
                    Utility utility = new Utility();
                    utility.checkValidString(answerText);

                    //update the answer text of the selected row
                    AnswerController sysmgr = new AnswerController();
                    sysmgr.Answer_Update(answerText, answerId);
                    UpdateAnswersListView.DataBind();
                    UpdateAnswersListView.EditIndex = -1;
                }, "Success", "Answer has been updated.");
            }
            
            //show success/error message
            MessageUserControl.Visible = true;

            //show datapager
            DataPager pager = UpdateAnswersListView.FindControl("ActiveDataPager") as DataPager;
            pager.Visible = true;

            //enable drop-down list and fetch button to allow editing of other answers to other questions
            QuestionsWithAnswersDropDownList.Enabled = true;
            FetchAnswersButton.Enabled = true;
        }
        //If the Cancel button is clicked, do the following actions:
        else if (e.CommandName.Equals("Cancel"))
        {
            //show informational message
            MessageUserControl.Visible = true;
            MessageUserControl.ShowInfo("Update canceled", "No changes to the selected answer were saved.");

            //show datapager
            DataPager pager = UpdateAnswersListView.FindControl("ActiveDataPager") as DataPager;
            pager.Visible = true;

            //enable drop-down list and fetch button to allow editing of other answers to other questions
            QuestionsWithAnswersDropDownList.Enabled = true;
            FetchAnswersButton.Enabled = true;
        }
    }

    /// <summary>
    /// Handles events that occur when an edit operation is requested, but before the Update Answers ListView item is put in edit mode.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemEditing event, which occurs when an edit operation is requested but before the ListView item is put in edit mode.</param>
    protected void UpdateAnswersListView_ItemEditing(object sender, ListViewEditEventArgs e)
    {
        //hide message user control
        MessageUserControl.Visible = false;

        //hide datapager
        DataPager pager = UpdateAnswersListView.FindControl("ActiveDataPager") as DataPager;
        pager.Visible = false;
    }

    /// <summary>
    /// Hides all other rows when the user enters edit mode to make editing answers easier.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Provides data for the ItemCreated and ItemDataBound events, which occur when an item is created and is bound to data in a ListView control.</param>
    protected void UpdateAnswersListView_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        //hide message user control
        MessageUserControl.Visible = false;

        if (UpdateAnswersListView.EditIndex != -1)
        {
            if (e.Item.DisplayIndex != UpdateAnswersListView.EditIndex)
            {
                e.Item.Visible = false;
            }
        }
    }
    #endregion
}