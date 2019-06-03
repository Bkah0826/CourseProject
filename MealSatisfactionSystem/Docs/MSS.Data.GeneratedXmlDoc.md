# MSS.Data #

## Type DTOs.QuestionAnswerDTO

 Handles the question text and the collection of the associated answers of a question entity 



---
#### Property DTOs.QuestionAnswerDTO.questionAnswers

 questionAnswers is a collection of answers associated to the question entity 



---
#### Property DTOs.QuestionAnswerDTO.questionText

 questionText is the text of the question entity 



---
## Type DTOs.QuestionDTO

 Handles the question text and the collection of the question IDs of question entities under the same question text 



---
#### Property DTOs.QuestionDTO.questionText

 quetionText is the text of the of the question entities 



---
#### Property DTOs.QuestionDTO.questionIds

 questionIds is a Collection of question IDs with the same question text 



---
## Type Entities.Answer

 Answer is the Answer Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation. 



---
#### Method Entities.Answer.#ctor

 Answer() is the default constructor for the Answer class that allows the creation of empty Answer objects. 



---
#### Property Entities.Answer.AnswerId

 AnswerId is a unique identifier that identifies a specific Answer. 



---
#### Property Entities.Answer.Value

 Value is a quantifiable field that indicates the points the Answer is worth 



---
#### Property Entities.Answer.MaxValue

 MaxValue is a quantifiable field that indicates the maximum amount of points this Answer can be worth 



---
#### Property Entities.Answer.Description

 Description is a short description of the Answer. 



---
#### Property Entities.Answer.Colour

 Colour holds the colour for the Answer, this is for reporting purposes. 



---
#### Property Entities.Answer.QuestionResponses

 QuestionResponses is a collection of questionResponses linked to the Answer via its AnswerId. 



---
#### Property Entities.Answer.Questions

 Questions is a collection of questions linked to the Answer via its AnswerId. 



---
## Type Entities.Question

 Question is the Question Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation. 



---
#### Method Entities.Question.#ctor

 Question() is the default constructor for the Question class that allows the creation of empty Question objects. 



---
#### Property Entities.Question.QuestionId

 QuestionId is a unique identifier that identifies a specific Question. 



---
#### Property Entities.Question.QuestionText

 QuestionText is a string of text for a question. 



---
#### Property Entities.Question.SubQuestionText

 SubQuestionText is a string of text for the subquestion if it exist. 



---
#### Property Entities.Question.QuestionParameter

 QuestionParameter is a string of text that defines the category of the question. 



---
#### Property Entities.Question.DateAdded

 DateAdded is the date the Question was added 



---
#### Property Entities.Question.Colour

 Colour is a property used by reporting to determine the colour used to signify this object in graphs. 



---
#### Property Entities.Question.QuestionResponses

 QuestionResponses is a collection of question responses connected to the question via questionId 



---
#### Property Entities.Question.Answers

 Answers is a collection of answers linked to the question via questionId 



---
## Type Entities.QuestionResponse

 QuestionResponse is the QuestionResponse Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation. 



---
#### Property Entities.QuestionResponse.ResponseId

 ResponseId is the unique identifier for a specific response. In the QuestionResponse Entity the ResponseId along with the QuestionId create a unique identifier for this entity. 



---
#### Property Entities.QuestionResponse.QuestionId

 QuestionId is the unique identifier for a question. In the QuestionResponse Entity the QuestionId along with the ResponseId create a unique identifier for this entity. 



---
#### Property Entities.QuestionResponse.AnswerId

 AnswerId is a unique identifier that identifies the specific Answer attached to a QuestionResponse. 



---
#### Property Entities.QuestionResponse.Response

 Response is the answer connected to the QuestionResponse via it's ResponseId 



---
#### Property Entities.QuestionResponse.Answer

 Answer is the answer connected to the QuestionResponse via it's AnswerId 



---
#### Property Entities.QuestionResponse.Question

 Question is the answer connected to the QuestionResponse via it's QuestionId 



---
## Type Entities.Response

 Response is the Response Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation. 



---
#### Property Entities.Response.ResponseId

 ResponseId is a unique identifier that identifies a specific Response. 



---
#### Property Entities.Response.UnitId

 UnitId is a unique identifier that identifies the specific Unit attached to a Response 



---
#### Property Entities.Response.Age

 Age is the age of the person who completed this Response. 



---
#### Property Entities.Response.Gender

 Gender is the gender of the person who completed this Response. 



---
#### Property Entities.Response.Date

 Date is the date the Response was completed 



---
#### Property Entities.Response.Comment

 Comment is for the comment field at the end of the survey 



---
#### Property Entities.Response.Unit

 Unit is the unit connected to the Response via UnitId 



---
#### Property Entities.Response.QuestionResponses

 QuestionResponses is a collection of question responses connected to the Response via ResponseId 



---
## Type Entities.Security.ApplicationUser

 ApplicationUser is the code first entity for the user table. It has all the basic information that the user table requires in the database. 



---
#### Property Entities.Security.ApplicationUser.SiteId

 SiteId is the unique Id of the site that the user belongs to (works in). Unless the user has a SuperUser role 



---
#### Property Entities.Security.ApplicationUser.FirstName

 FirstName is the user's First name 



---
#### Property Entities.Security.ApplicationUser.LastName

 LastName is the user's last name 



---
#### Property Entities.Security.ApplicationUser.Active

 Active is a boolean which indicates whether the user is active or not (inactive users can no longer log in and do not have access to the system) 



---
#### Property Entities.Security.ApplicationUser.Site

 Site is a navigation property used to look at the attributes of the site that the user is connected to 



---
## Type Entities.Security.RoleProfile

 RoleProfile is the code first entity for the role table. It has all the basic information that the role table requires in the database. 



---
#### Property Entities.Security.RoleProfile.RoleId

 RoleId is a unique hashed text identifier for the tole 



---
#### Property Entities.Security.RoleProfile.RoleName

 RoleName is the name of the role in the database(eg. AdminView). 



---
#### Property Entities.Security.RoleProfile.UserNames

 UserNames is a collection that contains the username(s) of the user(s) the role belongs to 



---
## Type Entities.Security.SecurityRoles

 SecurityRoles class contains the default roles that will be entered into the database. It is purely a data storage class with no functionality and does not connect to the database in any way 



---
#### Field Entities.Security.SecurityRoles.SuperUser

 SuperUser is the string that contains the name of the SuperUser role 



---
#### Field Entities.Security.SecurityRoles.AdminViews

 AdminViews is the string that contains the name of the AdminView role 



---
#### Field Entities.Security.SecurityRoles.AdminEdits

 AdminEdits is the string that contains the name of the AdminEdits role 



---
#### Property Entities.Security.SecurityRoles.DefaultSecurityRoles

 Returns all of the default roles in this class in a list 



---
## Type Entities.Security.UserProfile

 UserProfile is a custom class that we use to store user information when we are attempting to add change or archive a user in the user manager 



---
#### Property Entities.Security.UserProfile.UserId

 UserId is the user's id is the same as the Id of the Id of the user in the aspNetUsers table in the database 



---
#### Property Entities.Security.UserProfile.UserName

 UserName is the user's username that this user is going to use to log into our system 



---
#### Property Entities.Security.UserProfile.SiteId

 SiteId is the Id of the site that the user belongs to (works in). Unless the user has a SuperUser role 



---
#### Property Entities.Security.UserProfile.FirstName

 FirstName is the user's First name 



---
#### Property Entities.Security.UserProfile.LastName

 LastName is the user's last name 



---
#### Property Entities.Security.UserProfile.Active

 Active is a boolean which indicates whether the user is active or not (inactive users can no longer log in and do not have access to the system) 



---
#### Property Entities.Security.UserProfile.EmailConfirmation

 EmailConfirmation is the field contains information about whether the user has confirmed their email 



---
#### Property Entities.Security.UserProfile.RequestedPassword

 RequestedPassword is the user's requested password (this will most likely end up as their password in the database although it is only the requested password) 



---
#### Property Entities.Security.UserProfile.RoleMemberships

 RoleMemberships is a collection of the roles that the user has () 



---
#### Property Entities.Security.UserProfile.Site

 Site is the description of the site that the user works in 



---
## Type Entities.Site

 Site is the Site Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation. 



---
#### Method Entities.Site.#ctor

 Allows the instantiation of empty Site objects. 



---
#### Property Entities.Site.SiteId

 SiteId is a unique identifier that identifies a specific Site. 



---
#### Property Entities.Site.SiteName

 SiteName is the name of the Site. 



---
#### Property Entities.Site.Description

 Description is a short description of the Site. 



---
#### Property Entities.Site.Passcode

 Passcode is the code that is required for patients to access the survey for each specific Site. 



---
#### Property Entities.Site.Disabled

 Disabled is a field that determines whether or not a Site is currently active. false = active, true = inactive. 



---
#### Property Entities.Site.Units

 Units is a collection of units linked to the Site via its SiteId. 



---
## Type Entities.Unit

 Unit is the Unit Entity. It has all the basic information that the table requires in the database, as well as virtual attributes for navigation. 



---
#### Method Entities.Unit.#ctor

 Unit() is a default constructor for the Unit class that allows the creation of empty Unit objects. 



---
#### Property Entities.Unit.UnitId

 UnitId is a unique identifier that identifies a specific Unit. 



---
#### Property Entities.Unit.SiteId

 SiteId is a unique identifier that identifies the specific site attached to a Unit 



---
#### Property Entities.Unit.UnitName

 UnitName is the name of the Unit. 



---
#### Property Entities.Unit.Description

 Description is a short description of the Unit. 



---
#### Property Entities.Unit.Disabled

 Disabled is a field that determines whether or not a Unit is currently active. false = active, true = inactive. 



---
#### Property Entities.Unit.Responses

 Responses is a collection of responses linked to the Unit via its UnitId. 



---
#### Property Entities.Unit.Site

 Site is the site connected to the Unit via it's SiteId. 



---
## Type POCOs.AnswerPOCO

 Handles the description and answer Id of an answer entity 



---
#### Property POCOs.AnswerPOCO.description

 Description is a short description of the answer. May also be referred to as the answer text. 



---
#### Property POCOs.AnswerPOCO.answerId

 answerId is the unique identifier of the answer 



---
## Type POCOs.ChartingResponse

 The parent class to hold the data for each charting request 



---
#### Property POCOs.ChartingResponse.Question

 question contains Question Text 



---
#### Property POCOs.ChartingResponse.Subtext

 Subtext contains Question subtext 



---
#### Property POCOs.ChartingResponse.Parameter

 Parameter contains the shorthand question parameter 



---
#### Property POCOs.ChartingResponse.Gender

 Gender contains array of genders 



---
#### Property POCOs.ChartingResponse.Age

 Age contains array of ages 



---
#### Property POCOs.ChartingResponse.Data

 Data contains the array of data orginized by date 



---
## Type POCOs.DateData

 Holds answers with thir data value and date 



---
#### Property POCOs.DateData.AnswerText

 AnswerText contains the descriptive text of the answer selected 



---
#### Property POCOs.DateData.Date

 Date contains the date the response was submitted 



---
#### Property POCOs.DateData.Data

 Data contains the value associated with the answer 



---
## Type POCOs.ChartingHomeResponse

 This object holds all relevant data for generating the analytics on the home page. It is a list of surveys. 



---
#### Property POCOs.ChartingHomeResponse.questions

 questions contains the survey's questions as a string value 



---
#### Property POCOs.ChartingHomeResponse.subQuestions

 subQuestions contains questions that belong under a branach of a larger overarching question as a string value. 



---
#### Property POCOs.ChartingHomeResponse.answers

 answers contains all answers' text of the survey. 



---
#### Property POCOs.ChartingHomeResponse.answerID

 Stores all ID's of the pulled response. 



---
#### Property POCOs.ChartingHomeResponse.questionID

 Stores all ID's of pulled response. 



---
#### Property POCOs.ChartingHomeResponse.value

 Stores the associated scored value of each answer. 



---
#### Property POCOs.ChartingHomeResponse.unitID

 unitId is the unit of the response being shown. 



---
#### Property POCOs.ChartingHomeResponse.questionParam

 questionParam stores the associated search parameter of each question. 



---
#### Property POCOs.ChartingHomeResponse.maxValue

 maxValue stores the maximum point value that can be scored for a each question. 



---
#### Property POCOs.ChartingHomeResponse.colour

 colour stores the colour associated with each question 



---
#### Property POCOs.ChartingHomeResponse.removed

 removed stores the unit's deactivated status. 



---
#### Property POCOs.ChartingHomeResponse.unitName

 unitName stores the unit name. 



---
## Type POCOs.LookupValues

 For charting lookup tables. Holds all types of queries (lookupAnswers, lookupQuestion, lookupSites) 



---
#### Property POCOs.LookupValues.Id

 Id of the item being returned 



---
#### Property POCOs.LookupValues.Description

 Description of the item pulled back 



---
#### Property POCOs.LookupValues.Value

 Value of the item pulled back 



---
#### Property POCOs.LookupValues.Type

 Type of the query that was originally passed in (lookupAnswers, lookupQuestion, lookupSites) 



---
## Type POCOs.QuestionPOCO

 Handles only the question Id of a question entity 



---
#### Property POCOs.QuestionPOCO.questionId

 questionId is the unique identifier of the question 



---
## Type POCOs.QuestionSubQuestion

 QuestionSubQuestion is a POCO class that contains the id and text for question. 



---
#### Property POCOs.QuestionSubQuestion.QuestionText

 QuestionText is used to store the QuestionText from database when used. 



---
#### Property POCOs.QuestionSubQuestion.SubquestionText

 SubquestionText is used to store the SubQuestionText from database when used. 



---
#### Property POCOs.QuestionSubQuestion.id

 id is used to store the question id from database when used. 



---
## Type POCOs.SubQuestionPOCO

 Handles the question Id and the subquestion text of a question entity 



---
#### Property POCOs.SubQuestionPOCO.QuestionId

 QuestionId is the unique identifier of the question entity 



---
#### Property POCOs.SubQuestionPOCO.SubQuestionText

 SubquestionText of the question entity 



---
## Type POCOs.SurveyOverview

 Holds the all data associated with a survey overview, taken from the Response, Site and Unit entities. 



---
#### Property POCOs.SurveyOverview.ResponseId

 ResponseId contains the ResponseId field from the Response entity. 



---
#### Property POCOs.SurveyOverview.SiteName

 SiteName contains the SiteName field from the Site entity. 



---
#### Property POCOs.SurveyOverview.UnitName

 UnitName contains the UnitName field from the Unit entity. 



---
#### Property POCOs.SurveyOverview.Gender

 Gender contains the Gender field (as a string) from the Response entity. 



---
#### Property POCOs.SurveyOverview.Age

 Age contains the Age field from the Response entity. 



---
#### Property POCOs.SurveyOverview.Date

 Date contains the Date field from the Response entity. 



---
#### Property POCOs.SurveyOverview.Comment

 Comment contains the Comment field from the Response entity. 



---


