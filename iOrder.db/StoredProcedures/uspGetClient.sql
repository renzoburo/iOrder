CREATE PROCEDURE [dbo].[uspGetClient]
    @Id UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SELECT [Id]
          ,[FirstNames]    
          ,[Surname]       
          ,[AddressType]   
          ,[StreetAddress] 
          ,[Suburb]
          ,[City]
          ,[PostalCode]
      FROM [dbo].[Client]
     WHERE ISNULL(@Id, 0x0) = 0x0 OR [Id] = @Id --This will either get all or 1 client
END
