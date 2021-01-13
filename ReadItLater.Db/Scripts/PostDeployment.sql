/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


MERGE INTO [Folders] AS T
USING 
(VALUES ('6a26aa74-5fa5-4047-a0ca-096bf8128e62', null, 'coding', 10),
		('1DE2C0A2-3040-41C3-B87B-0A58122C5665', '6a26aa74-5fa5-4047-a0ca-096bf8128e62', '.net', 10),
		('CE3C370D-DD93-44D1-8A20-3C0348CC508F', '6a26aa74-5fa5-4047-a0ca-096bf8128e62', 'ai', 20),
		('C10386E3-355E-423B-842C-46D646BA5591', '6a26aa74-5fa5-4047-a0ca-096bf8128e62', 'other', 100),
		('41504905-a14d-407d-adc0-cc296252ff03', null, 'hardware', 20),
		('4383bad3-bc25-421f-aea8-7f891c3a48a0', null, 'business', 40),
		('08ff4b2f-c682-46f3-8362-c756abf423a5', null, 'travel', 30),
		('b2394c22-fce1-4106-b7f6-d13af4d56f5d', null, 'entertainment', 50)
) AS S ([Id], [ParentId], [Name], [Order])
ON T.[Id] = S.[Id] AND T.[Name] = S.[Name] AND T.[Order] = S.[Order]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [ParentId], [Name], [Order]) 
	VALUES (S.[Id], S.[ParentId], S.[Name], S.[Order]);