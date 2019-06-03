
/**  INSERT **/

USE [Survey]
GO

/* SITE */

INSERT INTO [dbo].[Site]
           ([SiteName]
           ,[Description]
           ,[Passcode]
		   ,[Disabled])
     VALUES
           ('Misericordia Hospital'
           ,'Misericordia Community Hospital'
           ,'Earthworm'
		   ,0)
GO

/* UNIT */

USE [Survey]
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'8E', '8East', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'7E', '7East', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'7W', '7West', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'6E', '6East', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'6W', '6West', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'5E', '5East', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'5W', '5West', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'3E', '3East', 0)
GO

INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (2,'3N', '3North', 0)

GO


USE [Survey]
GO

INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description],Colour)
     VALUES
           (4,4, 'Very good', '#60B760')
GO

INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (3,4,'Good', '#5FC0DC')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (2,4,'Fair', '#EFAD57')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (1,4,'Poor', '#D75553')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (0,4,'Don''t know/ No Opinion', '#fff')
GO

INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (1,3,'Portion sizes are too small', '#5FC0DC')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (3,3,'Portion sizes are just right', '#60B760')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (1,3,'Portion sizes are too large', '#D75553')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (4,4,'Always', '#60B760')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (3, 4,'Usually', '#5FC0DC')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (2,4,'Occasionally', '#EFAD57')
GO

INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (1,4,'Never', '#D75553')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (0,4,'I do not have any specific dietary requirements', '#E0F0D9')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (5,5,'5', '#60B760')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (4,5,'4', '#468CC8')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (3,5,'3', '#5FC0DC')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (2,5,'2', '#EFAD57')
GO
INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (1,5,'1', '#D75553')
GO

INSERT INTO [dbo].[Answer]
           ([Value],[MaxValue]
           ,[Description], Colour)
     VALUES
           (-1,5,'','#FFFFFF')
GO

/* Question */

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('During this hospital stay, how would you describe the following features of your meals?','The variety of food in your daily meals'
           ,'Variety',GETDATE(), '#919191')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('During this hospital stay, how would you describe the following features of your meals?','The taste and flavour of your meals'
           ,'Taste',GETDATE(), '#5FC0DC')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('During this hospital stay, how would you describe the following features of your meals?','The temperature of your hot food'
           ,'Temperature',GETDATE(), '#EFAD57')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('During this hospital stay, how would you describe the following features of your meals?','The overall appearance of your meal'
           ,'Appearance',GETDATE(), '#1fa318')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('During this hospital stay, how would you describe the following features of your meals?','The helpfulness of the staff who deliver your meals'
           ,'Helpfulness',GETDATE(), '#60B760')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('How satisfied are you with the portion sizes of your meals?',''
           ,'Portions',GETDATE(), '#D75553')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('Do your meals take into account your specific dietary requirements? (for example; food allergies, medical requirements, cultural preferences)',''
           ,'Dietary Requirements',GETDATE(), '#468CC8')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('Overall, how would you rate your meal experience?',''
           ,'Overall',GETDATE(), '#273D8D')
GO

INSERT INTO [dbo].[Question]
           ([QuestionText],[SubQuestionText],[QuestionParameter],[DateAdded], Colour)
     VALUES
           ('Is there anything else you would like to share about your meal experience?',''
           ,'Comments',GETDATE(), '#FFFFFF')
GO

/* QuestionAnswer */

/* Insert the first 5 Answer to each of the first 5 Question */
CREATE PROCEDURE insertQuestionAnswer_1 as
	DECLARE @q int = 0
	WHILE @q < 5 
	BEGIN
		SET @q = @q + 1
		DECLARE @a int = 0
		WHILE @a < 5
		BEGIN
			SET @a = @a + 1
			--print(@a)
			--print(@q)
			INSERT INTO [dbo].[QuestionAnswer]
				   ([AnswerId], [QuestionId])
			 VALUES
				   (@a, @q)
		END
	END
GO

EXEC insertQuestionAnswer_1

INSERT INTO [dbo].[QuestionAnswer]
				   ([AnswerId], [QuestionId])
			 VALUES
				   (6, 6)
GO

INSERT INTO [dbo].[QuestionAnswer]
				   ([AnswerId], [QuestionId])
			 VALUES
				   (7, 6)
GO

INSERT INTO [dbo].[QuestionAnswer]
				   ([AnswerId], [QuestionId])
			 VALUES
				   (8, 6)
GO

CREATE PROCEDURE insertQuestionAnswer_2 as
	DECLARE @a int = 8
	WHILE @a < 12 
	BEGIN
		SET @a = @a + 1
		INSERT INTO [dbo].[QuestionAnswer]
				([AnswerId], [QuestionId])
			VALUES
				(@a, 7)
	END
GO

EXEC insertQuestionAnswer_2

GO

INSERT INTO [dbo].[QuestionAnswer]
           ([AnswerId],[QuestionId])
     VALUES
           (13,7)
GO


USE [Survey]
GO

INSERT INTO [dbo].[QuestionAnswer]
           ([AnswerId],[QuestionId])
     VALUES
           (14,8)
GO
INSERT INTO [dbo].[QuestionAnswer]
           ([AnswerId],[QuestionId])
     VALUES
           (15,8)
GO
INSERT INTO [dbo].[QuestionAnswer]
           ([AnswerId],[QuestionId])
     VALUES
           (16,8)
GO
INSERT INTO [dbo].[QuestionAnswer]
           ([AnswerId],[QuestionId])
     VALUES
           (17,8)
GO
INSERT INTO [dbo].[QuestionAnswer]
           ([AnswerId],[QuestionId])
     VALUES
           (18,8)
GO


INSERT INTO [dbo].[QuestionAnswer]
           ([AnswerId],[QuestionId])
     VALUES
           (19,9)
GO