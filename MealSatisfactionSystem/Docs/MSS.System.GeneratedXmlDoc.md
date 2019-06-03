# MSS.System #

## Type MSSSystem.BLL.AnswerController

 AnswerController allows the webpage to access the Answer entity. 



---
#### Method MSSSystem.BLL.AnswerController.AnswerList(System.Int32)

 Lists all the answers associated to the supplied Question ID 

|Name | Description |
|-----|------|
|questionId: |Contains the question's unique identifier|
**Returns**: A list of answers associated to the question ID



---
#### Method MSSSystem.BLL.AnswerController.AnswersByQuestion_List(System.String)

 Lists answers by question text 

|Name | Description |
|-----|------|
|questionText: |Contains the question text of the question that is associated to the answers|
**Returns**: A list of answers by question text



---
#### Method MSSSystem.BLL.AnswerController.Answer_Update(System.String,System.Int32)

 Updates the description of the answer 

|Name | Description |
|-----|------|
|description: |Contains the new answer description that replaces the existing answer description in the database|
|answerId: |Contains the unique identifier of the answer to be updated|


---
## Type MSSSystem.BLL.ChartingResponseController

 Controls the request and return of the charting data 



---
#### Method MSSSystem.BLL.ChartingResponseController.GetChartingResponseData(System.String,System.DateTime,System.DateTime,System.Collections.Generic.List{System.String},System.Collections.Generic.List{System.String},System.Collections.Generic.List{System.String},System.Int32)

 For getting data from the webservice. Uses dynamic Linq to create query strings. Allows for custom building. 

|Name | Description |
|-----|------|
|Parameter: |Contains a single Question Parameter to filter by|
|FromDate: |Contains the start date to filter by|
|ToDate: |Contains the date to filter to|
|Units: |Contains an array of units to filter by|
|Genders: |Contains an array of genders to filter by|
|Ages: |Contains an array of ages(ints) to filter by|
|Site: |Contains the SiteId of the user generating the reports|
**Returns**: ChartingResponse with gender,age,and data(value, desc, date)



---
## Type MSSSystem.BLL.DatabaseCreation

 Handles the actions related to creating the database from the script file. 



---
#### Method MSSSystem.BLL.DatabaseCreation.runSqlScriptFile(System.String)

 Fills the DB with the script data by splitting the script on GO commands. Prints out to db_log.txt with error messages 

|Name | Description |
|-----|------|
|pathStoreProceduresFile: |Path to db script file|


---
#### Method MSSSystem.BLL.DatabaseCreation.isDBFilled

 Using the questions table to query if the database has been loaded with script data yet. 

**Returns**: bool based on if the db is filled or not



---
## Type MSSSystem.BLL.HomePageController

 Holds all business layer classes associated with the homepage. 



---
#### Method MSSSystem.BLL.HomePageController.getSite(System.String)

 Queries and returns the site ID the current user is affiliated with. 

|Name | Description |
|-----|------|
|userId: |The ID of the user logged in|
**Returns**: The site ID



---
#### Method MSSSystem.BLL.HomePageController.getPass(System.String)

 Queries and returns the site's daily passcode based on site which the user is part of. 

|Name | Description |
|-----|------|
|userId: |The ID of the user logged in|
**Returns**: The site's Passcode



---
#### Method MSSSystem.BLL.HomePageController.grabData(System.Int32)

 Queries and returns all QuestionResponses and thier associated values that are dated within the current quarter. 

|Name | Description |
|-----|------|
|siteId: |This is the site ID that the current user is affiliated with.|
**Returns**: A list of QuestionResponses and thier relevatn associated data.



---
#### Method MSSSystem.BLL.HomePageController.grabData(System.Int32,System.DateTime,System.DateTime)

 Overloaded grabData() for filtering based on date. 

|Name | Description |
|-----|------|
|siteId: |Id of site to query|
|fromDate: |From date to query|
|toDate: |To date to query|
**Returns**: 



---
## Type MSSSystem.BLL.LookupController

 Handles the return of lookup table values 



---
#### Method MSSSystem.BLL.LookupController.getValues(System.String)

 Based on the passed in type, gets the required information and stores in LookupValues object 

|Name | Description |
|-----|------|
|type: |Contains the type of lookup table needed|
**Returns**: List of LookupValues, or an empty list



---
## Type MSSSystem.BLL.QuestionController

 Allows the webpage to access the Question entity. 



---
#### Method MSSSystem.BLL.QuestionController.GetQuestionParameterList

 Lists all questions 

**Returns**: A list of all questions



---
#### Method MSSSystem.BLL.QuestionController.Question_Count

 Counts number of questions 

**Returns**: An integer representing the total number of questions



---
#### Method MSSSystem.BLL.QuestionController.GetQuestionText(System.Int32)

 Gets the QuestionText and SubQuestionText of a question with the supplied Question ID 

|Name | Description |
|-----|------|
|questionId: |Contains the Question ID for the question text|
**Returns**: The QuestionText and SubQuestionText of the supplied Question ID



---
#### Method MSSSystem.BLL.QuestionController.Questions_List

 Lists questions by distinct question text 

**Returns**: A list of distinct question text



---
#### Method MSSSystem.BLL.QuestionController.QuestionsWithSubQuestions_List

 Lists only questions in the survey that have subquestions 

**Returns**: A list of questions with subquestions



---
#### Method MSSSystem.BLL.QuestionController.QuestionsWithAnswers_List

 Lists only questions in the survey that have associated answers, excluding questions that prompt respondents for comments 

**Returns**: A list of questions that have associated answers



---
#### Method MSSSystem.BLL.QuestionController.Question_Update(System.String,System.Int32)

 Updates the question in the database 

|Name | Description |
|-----|------|
|questionText: |Contains the new question text that replaces the existing question text in the database|
|questionId: |Contains the ID of the question to be updated|


---
#### Method MSSSystem.BLL.QuestionController.SubQuestionsByQuestion_List(System.String)

 Lists subquestions by question text 

|Name | Description |
|-----|------|
|questionText: |Contains the question text of the question that is checked for any associated subquestions|
**Returns**: A list of subquestions associated to the supplied question text



---
#### Method MSSSystem.BLL.QuestionController.SubQuestion_Update(System.Int32,System.String)

 Updates a subquestion in the database 

|Name | Description |
|-----|------|
|questionId: |Contains the ID of the question associated the subquestion to be updated|
|subQuestionText: |Contains the new Subquestion text that replaces the existing subquestion text in the database|


---
## Type MSSSystem.BLL.QuestionResponseController

 QuestionResponseController allows the webpage to access the QuestionResponse entity. 



---
#### Method MSSSystem.BLL.QuestionResponseController.Add_QuestionResponse(System.Int32,System.Int32,System.Int32)

 Method for adding question response to database 

|Name | Description |
|-----|------|
|responseId: |Contians the response unique identifier|
|questionId: |Contains the question unique identifier|
|answerId: |Contains the answer unique identifier|


---
## Type MSSSystem.BLL.ResponseController

 Allows the website to access the response entity in the database 



---
#### Method MSSSystem.BLL.ResponseController.Add_NewResponse(MSS.Data.Entities.Response)

 Adds a new instance of a response in the database 

|Name | Description |
|-----|------|
|response: |Contains the response item that is being added to the database. Item obtained from Survey|


---
#### Method MSSSystem.BLL.ResponseController.Get_NewestResponseID

 Finds the newest ResponseId 

**Returns**: A integer representing the newest ResponseId



---
## Type MSSSystem.BLL.Security.RoleManager

 Role manager is the class that is responsible for methods involving the role such as inserting the default roles on 



---
#### Method MSSSystem.BLL.Security.RoleManager.#ctor

 Gets called when the class is constructed 



---
#### Method MSSSystem.BLL.Security.RoleManager.AddDefaultRoles

 Adds all 3 of the default roles(AdministratorView, AdministratorEdit, SuperUser) to the database 



---
#### Method MSSSystem.BLL.Security.RoleManager.ListAllRoleNames

 Lists the names of all of the roles in the database 

**Returns**: A list of strings containing the names of the roles in the database



---
#### Method MSSSystem.BLL.Security.RoleManager.ListAllRoles

 Lists all roles as classes from the database 

**Returns**: A list of Role profile classes which contains all information about all of the roles in the database



---
#### Method MSSSystem.BLL.Security.RoleManager.ListAllAdminRoles

 Lists the names of all of the roles in the database except for the SuperUser role 

**Returns**: A list of strings containing the names of the roles in the database except for the SuperUser role



---
## Type MSSSystem.BLL.Security.UserManager

 User manager class is the class that is responsible for methods involving the user such as inserting a user in the database, updating a user and deactivating a user 



---
#### Field MSSSystem.BLL.Security.UserManager.STR_DEFAULT_PASSWORD

 STR_DEFAULT_PASSWORD is the default password for the webmaster 



---
#### Field MSSSystem.BLL.Security.UserManager.STR_USERNAME_FORMAT

STR_USERNAME_FORMAT is the format of the username for a given user



---
#### Field MSSSystem.BLL.Security.UserManager.STR_WEBMASTER_USERNAME

 STR_WEBMASTER_USERNAME is the default webmaster username and it cannot be changed on our website 



---
#### Method MSSSystem.BLL.Security.UserManager.#ctor

 Gets called when the class is constructed 



---
#### Method MSSSystem.BLL.Security.UserManager.AddWebMaster

 Adds a default user with Superuser privileges to the database 



---
#### Method MSSSystem.BLL.Security.UserManager.VerifyNewUserName(System.String)

 Queries the database for usernames that contains the provided username and then adds the number of usernames found to the end of the supplied username making the username unique 

|Name | Description |
|-----|------|
|suggestedUserName: |The username to be verified|
**Returns**: Returns a unique username



---
#### Method MSSSystem.BLL.Security.UserManager.ListAllUsers

 Returns a list of all of the users in our system 

**Returns**: A list of userprofile classes



---
#### Method MSSSystem.BLL.Security.UserManager.AddUser(MSS.Data.Entities.Security.UserProfile)

 Adds a user to the database 

|Name | Description |
|-----|------|
|userinfo: |The user to be added to the database|


---
#### Method MSSSystem.BLL.Security.UserManager.AddUserToRole(MSS.Data.Entities.Security.ApplicationUser,System.String)

 Adds the user to the role with the name provided 

|Name | Description |
|-----|------|
|userAccount: |The applicationUser class with information about the user|
|roleName: |The role name the user should be added to|


---
#### Method MSSSystem.BLL.Security.UserManager.UpdateUser(MSS.Data.Entities.Security.UserProfile)

 Updates the user with the same Id as the Id in the userProfile passed into the method 

|Name | Description |
|-----|------|
|userinfo: |The information about which user should be updated as well as the new information for the updated user|


---
#### Method MSSSystem.BLL.Security.UserManager.GetUserSiteId(System.String)

 Gets the user's site Id based on the user's username 

|Name | Description |
|-----|------|
|userName: |The user's username|
**Returns**: The user's siteId



---
#### Method MSSSystem.BLL.Security.UserManager.ListUser_BySearchParams(System.String,System.Collections.Generic.List{System.Int32},System.Collections.Generic.List{System.String},System.Int32)

 Looks up all users and filters by the search parameters provided 

|Name | Description |
|-----|------|
|partialName: |Partial username, firstname or last name of the users that will be displayed|
|sites: |The site(s) of the users that will be displayed|
|roleName: |The role(s) of the users that will be displayed|
|status: |The status of the users that will be displayed. 1 is active, 2 is inactive 3 is either active or inactive users.|
**Returns**: List of users stored in a list of UserProfile



---
#### Method MSSSystem.BLL.Security.UserManager.getUserRole(System.String)

 Gets the role of the user with the provided username 

|Name | Description |
|-----|------|
|username: |Username of the user whose role to look for|
**Returns**: 



---
## Type MSSSystem.BLL.SiteController

 Allows the webpage to access the Site entity. 



---
#### Method MSSSystem.BLL.SiteController.Site_AddDefault

 Adds a default site to the database on initial project deployment, the default SuperUser account will be attached to this site. 



---
#### Method MSSSystem.BLL.SiteController.Site_List(System.Boolean)

 Searches for all the Sites based on the deactivated parameter. 

|Name | Description |
|-----|------|
|deactivated: |Contains the status of the desired sites; true for deactivated sites, false for active sites.|
**Returns**: A list of Sites.



---
#### Method MSSSystem.BLL.SiteController.Site_FindById(System.Int32)

 Looks for the site whoose ID matches the ID provided. 

|Name | Description |
|-----|------|
|siteId: |Contains the site ID of the site that the user is looking for|
**Returns**: A Site object matching the siteId



---
#### Method MSSSystem.BLL.SiteController.Site_List(System.Boolean,System.String,System.String)

 Searches for all the Sites based on the deactivated parameter as well as a user defined search parameter and search fields. 

|Name | Description |
|-----|------|
|deactivated: |Contains the status of the desired sites; true for deactivated sites, false for active sites.|
|searchArg: |Contains the term the user is searching for.|
|searchBy: |Contains the value of the fields the user wishes to search against.|
**Returns**: A list of Sites.



---
#### Method MSSSystem.BLL.SiteController.Site_List_All

 Searches for all sites, regardless of if they are deactivated. 

**Returns**: A list of all Sites.



---
#### Method MSSSystem.BLL.SiteController.Site_Add(System.String,System.String,System.String)

 Adds a new site to the system. 

|Name | Description |
|-----|------|
|siteName: |Contains the user entered name for the new site.|
|description: |Contains the user entered description of the new site|
|passcode: |Contains the user entered passcode of the new site.|
**Returns**: A SiteId of the newly added Site.



---
#### Method MSSSystem.BLL.SiteController.Site_Update(System.Int32,System.String,System.String,System.String)

 Updates a site in the database. 

|Name | Description |
|-----|------|
|siteId: |Contains the siteId of the unit that was selected for the update.|
|siteName: |Contains the user updated name for the site.|
|description: |Contains the user updated description for the site.|
|passcode: |Contains the user updated passcode for the site.|


---
#### Method MSSSystem.BLL.SiteController.Site_Deactivate(System.Int32)

 Sets the desired site's Disabled field to 1(True). 

|Name | Description |
|-----|------|
|siteId: |Contains the siteId of the site being deactivated.|


---
#### Method MSSSystem.BLL.SiteController.getPasscode

 Gets a randomly generated word from the file nounlist.txt in the website root folder for use as a passcode. 

**Returns**: A randomly generated passcode.



---
#### Method MSSSystem.BLL.SiteController.getSinglePasscode(System.Int32)

 Gets a single passcode. 

**Returns**: A single passcode string.



---
#### Method MSSSystem.BLL.SiteController.Site_ChangePasscode

 Changes the passcode at every enabled site. This method is run automatically at midngiht by HangFire. (See startup.cs in MSS.Website/App_code) 



---
#### Method MSSSystem.BLL.SiteController.Site_ChangeSinglePasscode(System.Int32,System.String)

 Updates a single site in the DB with a new passcode, checks for unique 



---
#### Method MSSSystem.BLL.SiteController.Site_PasscodeList

 Obtains a list of current passcodes for existing sites. 

**Returns**: A list of passcodes for existing sites.



---
#### Method MSSSystem.BLL.SiteController.GetIdFromPasscode(System.String)

 Obtains the Siteid that matches a user entered passcode. 

|Name | Description |
|-----|------|
|passcode: |Contains the site passcode|
**Returns**: A SiteId from the site matching the passcode.



---
## Type MSSSystem.BLL.SurveyController

 Allows the webpage to access the Response entity. 



---
#### Method MSSSystem.BLL.SurveyController.Response_List_All

 Response_List_All() will search for all responses. This method is used to populate the ViewResponses page before filters have been selected. 

**Returns**: List of all responses



---
## Type MSSSystem.BLL.SurveyResponseController

 Controls the "ViewResponses" webpage, including its access to the SurveyOverview and IndividualSurvey POCOs. 



---
#### Method MSSSystem.BLL.SurveyResponseController.Response_List_All

 Searches for all the Responses that are associated with an active Site and Unit and stores them in a SurveyOverview list. 



> This method is used to populate the "ViewResponses" page before any filters have been selected by the user, if the user has a SuperUser role. 

**Returns**: A list of SurveyOverview POCOs with an active Site and active Unit.



---
#### Method MSSSystem.BLL.SurveyResponseController.Response_List_Filters(System.Collections.Generic.List{System.String},System.Collections.Generic.List{System.String},System.Collections.Generic.List{System.String},System.Nullable{System.DateTime},System.Nullable{System.DateTime},System.Collections.Generic.List{System.String})

 Searches for all the Responses that match the filters inputted by the user and stores them in a SurveyOverview list. 



> This method is used to narrow down the displayed Responses based on inputted criteria. In addition, this method is used to populate the "ViewResponses" page before any filters have been selected by the user, if the user does not have a SuperUser role. 

|Name | Description |
|-----|------|
|sites: |Contains a list of SiteName strings.|
|units: |Contains a list of UnitName strings.|
|gender: |Contains a list of Gender strings. |
|startDate: |Contains a DateTime? value of the text input in the FromDate control.|
|endDate: |Contains a DateTime? value of the text input in the ToDate control.|
|age: |Contains a list of Age strings.|
**Returns**: A list of SurveyOverview POCOs that match filters.



---
#### Method MSSSystem.BLL.SurveyResponseController.Active_Site_List

 Finds all active Sites and stores the active SiteName in a string list. 

**Returns**: A list of SiteName strings.



---
#### Method MSSSystem.BLL.SurveyResponseController.Active_Unit_List

 Finds all active Units and stores the active UnitNames in a string list. 

**Returns**: A list of UnitName strings.



---
#### Method MSSSystem.BLL.SurveyResponseController.Get_SiteName(System.Int32)

 Finds a SiteName, based on the SiteId that is associated with the logged in user's account. 



> This method is used when the logged in user does not have a SuperUser role and, therefore, does not have access to the survey data from all sites. The user does not input the siteId for this method. Instead, the siteId is determined in the code behind, based on the user's account information. 

|Name | Description |
|-----|------|
|siteId: |Contains an integer equal to the SiteId of the logged in UserProfile.|
**Returns**: A string of the Site's SiteName.



---
#### Method MSSSystem.BLL.SurveyResponseController.Get_SiteStatus(System.Int32)

 Determines if a Site has been deactivated. 

|Name | Description |
|-----|------|
|siteId: |Contains an integer equal to the SiteId of the logged in UserProfile.|
**Returns**: A true/false value indicating whether the Site is disabled.



---
#### Method MSSSystem.BLL.SurveyResponseController.Get_Survey_Question(System.Int32)

 Searches for the QuestionText of the Question, based on the inputted questionId. 

|Name | Description |
|-----|------|
|questionId: |Contains an integer equal to the QuestionId of the requested Question.|
**Returns**: A string with the Question's QuestionText.



---
#### Method MSSSystem.BLL.SurveyResponseController.Get_Survey_SubQuestion(System.Int32)

 Searches for the SubQuestionText of the Question, based on the inputted questionId. 

|Name | Description |
|-----|------|
|questionId: |Contains an integer equal to the QuestionId of the requested Question.|
**Returns**: A string with the Question's SubQuestionText.



---
#### Method MSSSystem.BLL.SurveyResponseController.Get_Survey_Answer(System.Int32,System.Int32)

 Searches for the Answer Description of the Answer, based on the inputted responseId and questionId. 

|Name | Description |
|-----|------|
|responseId: |Contains an integer equal to the ResponseId of the requested Resposne.|
|questionId: |Contains an integer equal to the QuestionId of the requested Question.|
**Returns**: A string with the Answer's Description.



---
#### Method MSSSystem.BLL.SurveyResponseController.Response_List_Individual(System.Int32)

 Searches for the Response and stores the result in a SurveyOverview instance. 

|Name | Description |
|-----|------|
|responseId: |Contains an integer equal to the ResponseId of the selected Response.|
**Returns**: A single SurveyOverview instance.



---
## Type MSSSystem.BLL.UnitController

 This class will control all the methods that lookup or modify units 



---
#### Method MSSSystem.BLL.UnitController.Unit_List(System.Boolean)

 Unit_List will list all the units fitting the parameters entered. 

|Name | Description |
|-----|------|
|deactivated: |True to show deactivated sites, False to show Active sites|
**Returns**: Returns a list of units



---
#### Method MSSSystem.BLL.UnitController.Unit_Search(System.Boolean,System.String,System.String)

 Unit_Search will search for any unit fitting the parameters entered. 

|Name | Description |
|-----|------|
|deactivated: |True to show deactivated sites, False to show Active sites|
|searchArg: |The search arguments being passed in to look for|
|searchBy: |What to search for, IE: Description, SiteName, UnitName|
**Returns**: Returns a list of units



---
#### Method MSSSystem.BLL.UnitController.Unit_Search(System.Boolean,System.Int32,System.String,System.String)

 Unit_Search will search for any unit fitting the parameters entered. 

|Name | Description |
|-----|------|
|deactivated: |True to show deactivated sites, False to show Active sites|
|siteId: |SiteId of the current user looking for a list of units|
|searchArg: |The search arguments being passed in to look for|
|searchBy: |What to search for, IE: Description, SiteName, UnitName|
**Returns**: Returns a list of units



---
#### Method MSSSystem.BLL.UnitController.SiteUnitList(System.Int32)

 Method to look for active units with the same siteId 

|Name | Description |
|-----|------|
|siteId: |Unique identifer for specific site|
**Returns**: A list of units matching the siteId



---
#### Method MSSSystem.BLL.UnitController.Unit_Add(MSS.Data.Entities.Unit,System.Int32)

 Unit_Add will add a new unit to the database 

|Name | Description |
|-----|------|
|item: |Item is a Unit Entity. The fields not required are UnitId and deactivated.|
|userSiteId: |The siteId of the user attempting to add a new unit. This much match the Unit's siteId|
**Returns**: Returns the UnitId of the newly added Unit.



---
#### Method MSSSystem.BLL.UnitController.Unit_Add(System.Int32,System.String,System.String,System.Int32)

 Unit_Add will add a new unit to the database 

|Name | Description |
|-----|------|
|siteId: |The siteId of the new Unit|
|unitName: |The name of the new Unit|
|description: |The description of the new unit|
|userSiteId: |The siteId of the user attempting to add a new unit. This much match the Unit's siteId|
**Returns**: Returns the UnitId of the newly added Unit.



---
#### Method MSSSystem.BLL.UnitController.Unit_Update(MSS.Data.Entities.Unit)

 Unit_Update will update a unit in the database 

|Name | Description |
|-----|------|
|item: |Item is a completed Unit Entity that will be replacing the one in the database. The UnitId should not be changed to prevent errors.|


---
#### Method MSSSystem.BLL.UnitController.Unit_Update(System.Int32,System.Int32,System.String,System.String)

 Unit_Update will update a unit in the database 

|Name | Description |
|-----|------|
|unitId: |The UnitId of the unit you wish to update|
|siteId: |The new or old SiteId|
|unitName: |The new or old Unit Name|
|description: |The new or old Description|


---
#### Method MSSSystem.BLL.UnitController.Unit_Deactivate(MSS.Data.Entities.Unit)

 Unit_Inactive will set the desired unit's deactivated field to 1(True) 

|Name | Description |
|-----|------|
|item: |Item is the Unit that you want to set as inactive|
**Returns**: Returns the UnitId of the now inactive unit



---
#### Method MSSSystem.BLL.UnitController.Unit_Deactivate(System.Int32)

 Unit_Inactive will set the desired unit's deactivated field to 1(True) 

|Name | Description |
|-----|------|
|unitId: |UnitId of the Unit you want to be set as inactive|
**Returns**: Returns the UnitId of the now inactive unit



---
## Type MSSSystem.BLL.Utility

 Used to store utility methods that are used throughout the project. 



---
#### Method MSSSystem.BLL.Utility.checkValidString(System.String)

 Checks a string to see if it contains any invalid characters and throws an exception if it does. 

|Name | Description |
|-----|------|
|check: |Contains the string to be validated.|


---
## Type MSSSystem.DAL.MSSContext

 Maps our Entites to the SQL database. 



---
#### Method MSSSystem.DAL.MSSContext.#ctor

 Creates the database from our entities if it doesn't exist. 



---
#### Property MSSSystem.DAL.MSSContext.Answers

 Answers maps the Answer table and entity to one another. 



---
#### Property MSSSystem.DAL.MSSContext.Questions

 Questions maps the Question table and entity to one another. 



---
#### Property MSSSystem.DAL.MSSContext.QuestionResponses

 QuestionResponses maps the QuestionResponse table and entity to one another. 



---
#### Property MSSSystem.DAL.MSSContext.Responses

 Responses maps the Response table and entity to one another. 



---
#### Property MSSSystem.DAL.MSSContext.Sites

 Sites maps the Site table and entity to one another. 



---
#### Property MSSSystem.DAL.MSSContext.Units

 Units maps the Unit table and entity to one another. 



---
#### Method MSSSystem.DAL.MSSContext.OnModelCreating(System.Data.Entity.DbModelBuilder)

 Builds the schema of the database for us, it maps the properties of the relationships between the tables to the entites. 

|Name | Description |
|-----|------|
|modelBuilder: ||


---


