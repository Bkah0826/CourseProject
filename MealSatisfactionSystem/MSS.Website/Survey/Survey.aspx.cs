using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#region AdditionalNamespaces
using MSSSystem.BLL;
using MSS.Data.Entities;
#endregion
/// <summary>
/// Contains all fucntionality relating to this page
/// </summary>
public partial class Survey : System.Web.UI.Page
{
    /// <summary>
    ///Loads the page. For the survey page Page_Load checks the database to see if questions exist or no active units exist on Site. If no questions or no active units exist the user will be redirected to the SurveyAccessPage.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //Counts the number of questions
        QuestionController Qsysmgr = new QuestionController();
        int NumberOfQuestions = Qsysmgr.Question_Count();
        //Checks to see if there is questions 
        if (NumberOfQuestions == 0)
        {
            string error = "nodataquestions";
            //Redirects to survey access page
            Response.Redirect("SurveyAccess.aspx?error="+ error);
        }
        else
        {
            //Obtains correct passcode from survey access page
            Utility utility = new Utility();
                        string passcode = Request.QueryString["passcode"];
            SiteController sysmgr = new SiteController();
            //uses the passcode to obtain site id
            int siteid = sysmgr.GetIdFromPasscode(passcode);
            //Checks if units exists for site
            if (UnitCheck(siteid) == 0 && passcode != null)
            {
                string error = "nounitsinsite";
                Response.Redirect("SurveyAccess.aspx?error=" + error);
            }
            //changes the units drop down according to site that enters password
            UnitListODS.SelectParameters["siteId"].DefaultValue = siteid.ToString();
            UnitListODS.Select();
            //Checks if site exists
            if (siteid == 0)
            {
                //Redirects to survey access page
                Response.Redirect("SurveyAccess.aspx");
            }
            //List of labels
            List<Label> QuestionsLabelList = new List<Label>();
            QuestionsLabelList.Add(Q1Label);
            QuestionsLabelList.Add(Q1ALabel);
            QuestionsLabelList.Add(Q1BLabel);
            QuestionsLabelList.Add(Q1CLabel);
            QuestionsLabelList.Add(Q1DLabel);
            QuestionsLabelList.Add(Q1ELabel);
            QuestionsLabelList.Add(Q2Label);
            QuestionsLabelList.Add(Q3Label);
            QuestionsLabelList.Add(Q4Label);
            QuestionsLabelList.Add(Q5Label);
            //List of Question text
            List<string> QuestionTextList = new List<string>();
            //List of Sub question starters 
            List<string> SubQuestionCharList = new List<string>();
            SubQuestionCharList.Add("a) ");
            SubQuestionCharList.Add("b) ");
            SubQuestionCharList.Add("c) ");
            SubQuestionCharList.Add("d) ");
            SubQuestionCharList.Add("e) ");
            // counter for questions
            int questioncharcounter = 1;
            //counter for subquestions
            int subquestioncharcounter = 0;
            //For loop for adding question text to labels
            for (int question = 1; question < QuestionsLabelList.Count(); question++)
            {
               //if statement which determines if label is subquestion or question 
              
                if (QuestionTextList.Count() >= 1 && QuestionTextList.Count() < 6 )
                {
                    //Subquestion text
                    if (QuestionTextList.Count == 1)
                    {
                        question = question- 1;
                    } 
                        QuestionTextList.Add(SubQuestionCharList[subquestioncharcounter] + Qsysmgr.GetQuestionText(question).SubquestionText + ":");
                        subquestioncharcounter++;     
                                  
                }
                //Question text
                else
                {
                    
                    QuestionTextList.Add(questioncharcounter.ToString() +") "+  Qsysmgr.GetQuestionText(question).QuestionText);
                    questioncharcounter++;

                }
            }
            //If the amount of question texts are equal to the amount of question labels
            if (QuestionTextList.Count == QuestionsLabelList.Count())
            {
                //adds text to labels
                int counter = 0;
                foreach (var Label in QuestionsLabelList)
                {
                    Label.Text = QuestionTextList[counter];
                    counter++;
                }
            }
     
        }
    }
    /// <summary>
    /// Allows the MessageUserControl to handle any exceptions thrown from violating the constraints within the entity classes.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
    {
        MessageUserControl.HandleDataBoundException(e);
    }
    /// <summary>
    /// Sends the user back to Survey Access page.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void Back_Click(object sender, EventArgs e)
    {
        //Check out bootstrap modals for a popup message asking for user confirmation
        //Clear_Form();
        Response.Redirect("SurveyAccess.aspx");
    }

    /// <summary>
    ///Checks to see if a unit has been selected then submit the survey information to the database.
    /// </summary>
    /// <param name="sender">Contains a reference to the control/object that raised the event.</param>
    /// <param name="e">Contains the event data of the event that triggered the method.</param>
    protected void Submit_Click(object sender, EventArgs e)
    {
        
        if (int.Parse(CPUnitDDL.SelectedValue) == 0)
        {
            MessageUserControl.ShowInfo("Must select a unit to submit a survey.");
        }
        else
        {
            MessageUserControl.TryRun(() =>
            {
              //Utility for checking for special characters
                Utility utility = new Utility();
                utility.checkValidString(Q5TextBox.Text);
            //create response
            Response newResponse = new Response();
                newResponse.UnitId = int.Parse(CPUnitDDL.SelectedValue);
                newResponse.Age = AgeDDL.SelectedItem.Value;
                newResponse.Gender = GenderDDL.SelectedValue;
                newResponse.Date = DateTime.Now;
                newResponse.Comment = Q5TextBox.Text;

            //Add the new response to the database
            ResponseController sysmgr = new ResponseController();
                sysmgr.Add_NewResponse(newResponse);
            // List of all the user's answers from all questions
                List<int> questionAnswers = new List<int>();
                questionAnswers.Add(int.Parse(Q1ADDL.SelectedValue));
                questionAnswers.Add(int.Parse(Q1BDDL.SelectedValue));
                questionAnswers.Add(int.Parse(Q1CDDL.SelectedValue));
                questionAnswers.Add(int.Parse(Q1DDDL.SelectedValue));
                questionAnswers.Add(int.Parse(Q1EDDL.SelectedValue));
                questionAnswers.Add(int.Parse(Q2DDL.SelectedValue));
                questionAnswers.Add(int.Parse(Q3DDL.SelectedValue));
                questionAnswers.Add(int.Parse(Q4DDL.SelectedValue));
                //Loops through questionAnswers list and inserts response individually
                QuestionResponseController QRsysmgr = new QuestionResponseController();
                for (int questionnumber = 1; questionnumber <= questionAnswers.Count(); questionnumber++)
                {
                    QRsysmgr.Add_QuestionResponse(newResponse.ResponseId, questionnumber, questionAnswers[questionnumber-1]);
                } 
                            Response.Redirect("SurveySuccess.aspx?rid=" + newResponse.ResponseId);

            }, "Success", "Survey has been submitted.");
        }
    }
    /// <summary>
    /// Counts the number of active units attached to Site using the siteId parameter.
    /// </summary>
    /// <param name="siteId">Contains the siteId unique identifier for site</param>
    /// <returns>A number of sites which are active</returns>
    protected int UnitCheck(int siteId)
    {
        UnitController sysmgr = new UnitController();
       return sysmgr.SiteUnitList(siteId).Count();
    }
}