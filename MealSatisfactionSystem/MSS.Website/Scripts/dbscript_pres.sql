
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



GO
CREATE PROCEDURE dbo.generateResponsesAndQuestionResponses
(
     @iterations int
)
AS
BEGIN
	print('Doing '+  CAST(@iterations as varchar(10)) + 'iterations...')
    
	/* ghetto arrays */
	print('Creating temp Age table...')
	declare @ageTable table(ageId int, ageDesc varchar(30))
	insert into @ageTable (ageId,ageDesc ) values (1, 'Under 18')
	print('Inserting Under 18...')
	insert into @ageTable (ageId, ageDesc ) values (2, '18-34')
	print('Inserting 18-34...')
	insert into @ageTable (ageId, ageDesc ) values (3, '75+')
	print('Inserting 75+...')
	insert into @ageTable (ageId, ageDesc ) values (4, '35-54')
	print('Inserting 35-54...')
	insert into @ageTable (ageId, ageDesc ) values (5, '55-74')
	print('Inserting 55-74...')
	insert into @ageTable (ageId, ageDesc ) values (6, 'Prefer not to provide')
	print('Inserting Prefer Not to Say...')
	--SELECT TOP 1 ageDesc FROM @ageTable
	--							ORDER BY NEWID()

	print('Creating temp Gender table')
	declare @genderTable table(genderId int, genderDesc char(1))
	insert into @genderTable (genderId,genderDesc ) values (1, 'M')
	print('Inserting M...')
	insert into @genderTable (genderId, genderDesc ) values (2, 'F')
	print('Inserting F...')
	insert into @genderTable (genderId, genderDesc ) values (3, 'O')
	print('Inserting O...')

	DECLARE @counter Int = 0;
	WHILE @counter < @iterations
	BEGIN
			print('Starting transaciont...')
			BEGIN TRANSACTION;
			PRINT('Saving transaction point...')
			SAVE TRANSACTION MySavePoint;

			print('Creating Response Variables...')
			/* Get the variables we can't randomly gen */
			DECLARE @siteId Int;
			Set @siteId = (SELECT TOP 1 SiteId FROM Site
								ORDER BY NEWID());
			DECLARE @unitId Int;
			SET @unitId = (SELECT TOP 1 unitId FROM Unit
							WHERE SiteId = @siteId
								ORDER BY NEWID());
			DECLARE @age varchar(20);
			SET @age = (SELECT TOP 1 ageDesc FROM @ageTable
								ORDER BY NEWID());

			DECLARE @gender varchar(1);
			SET @gender = (SELECT TOP 1 genderDesc FROM @genderTable
								ORDER BY NEWID());
			DECLARE @date datetime;
			declare @FromDate date = '2018-01-01'
			declare @ToDate date = '2018-04-24'
			SET @date = (select dateadd(day, 
					   rand(checksum(newid()))*(1+datediff(day, @FromDate, @ToDate)), 
					   @FromDate))
			print('unit= ' + CAST(@unitId as varchar(10)) + ', Age= ' + @age + ', gender= ' + @gender + ',date= ' + CAST(@date as varchar(20)))
	
			BEGIN TRY
			print('Inserting new response...')
				DECLARE @responseId int;
				/* Response */
				INSERT INTO Response
						(				
						UnitId
						,Age
						,Gender
						,Date				
						)
					VALUES (
						@unitId
						, @age
						, @gender
						, @date				
						);

				SET @responseId = (SELECT IDENT_CURRENT('Response'))
				print(@responseId)

				/* Question 1, Answer 1-5 */
				print('Inserting Question 1...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,1
						, (SELECT floor(RAND()*(6-1)+1))
						);

				/* Question 2, Answer 1-5 */
				print('Inserting Question 2...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,2
						, (SELECT floor(RAND()*(6-1)+1))
						);

				/* Question 3, Answer 1-5 */
				print('Inserting Question 3...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,3
						, (SELECT floor(RAND()*(6-1)+1))
						);
				/* Question 4, Answer 1-5 */
				print('Inserting Question 4...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,4
						, (SELECT floor(RAND()*(6-1)+1))
						);
				/* Question 5, Answer 1-5 */
				print('Inserting Question 5...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,5
						, (SELECT floor(RAND()*(6-1)+1))
						);
				/* Question 1, Answer 6-8 */
				print('Inserting Question 6...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,6
						, (SELECT floor(RAND()*(9-6)+6))
						);
				/* Question 7, Answer 9-13 */
				print('Inserting Question 7...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,7
						, (SELECT floor(RAND()*(14-9)+9))
						);
				/* Question 8, Answer 14-18 */
				print('Inserting Question 8...')
				INSERT INTO QuestionResponse
						(
						ResponseId
						,QuestionId
						, AnswerId
						)
					VALUES
						(
						@responseId
						,8
						, (SELECT floor(RAND()*(19-14)+14))
						);

				SET @counter = @counter + 1;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
				BEGIN
					print('Error. Rollback.')
					ROLLBACK TRANSACTION MySavePoint; -- rollback to MySavePoint
				END
			END CATCH
			COMMIT TRANSACTION 
			print('Transaction Saved.')
	END
END
GO

DECLARE @iterations Int = 400;
Execute generateResponsesAndQuestionResponses @iterations;




INSERT INTO [dbo].[Site]
           ([SiteName]
           ,[Description]
           ,[Passcode]
		   ,[Disabled])
     VALUES
           ('Grey Nuns'
           ,'Grey Nuns Hospital'
           ,'Raindrop'
		   ,0)
GO


INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (3,'RNL', 'Renal Clinic', 0)
GO


INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (3,'LTC', 'Long Term Care', 0)
GO


INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (3,'CRD', 'Cardiology', 0)
GO


INSERT INTO [dbo].[Unit]
           ([SiteId], [UnitName],[Description],[Disabled])
     VALUES (3,'CAF', 'Cafeteria', 0)
GO