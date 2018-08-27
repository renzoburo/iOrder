CREATE PROCEDURE [dbo].[uspSaveClient]
    @Id UNIQUEIDENTIFIER = NULL OUTPUT,
    @FirstNames NVARCHAR(128),
    @Surname NVARCHAR(128),
    @AddressType UNIQUEIDENTIFIER,
    @StreetAddress NVARCHAR(128),
    @Suburb NVARCHAR(128),
    @City NVARCHAR(128),
    @PostalCode INT
AS
BEGIN
    DECLARE @result  TABLE
    (
      Id UNIQUEIDENTIFIER
    )

    BEGIN TRANSACTION

    BEGIN TRY
        /*
         * IF WE DON'T HAVE THIS CLIENT THEN WE DO AN INSERT
         */
        IF ISNULL(@Id, 0x0) = 0x0 OR NOT EXISTS ( SELECT 1 FROM [dbo].[Client] c WHERE c.[Id] = @Id)
        BEGIN
            INSERT [dbo].[Client] ([FirstNames], [Surname], [AddressType], [StreetAddress], [Suburb], [City], [PostalCode])
            OUTPUT INSERTED.Id INTO @result
            VALUES (@FirstNames, @Surname, @AddressType, @StreetAddress, @Suburb, @City, @PostalCode)

            SELECT TOP 1
                   @Id = Id
              FROM @result
        END
        ELSE
        BEGIN
            /*
             * IF THIS CLIENT EXISTS THEN UPDATE THE FIELDS
             * WE ONLY UPDATE FIELDS WE HAVE PASSED IN (THEY ARE LEFT AT THEIR ORIGINAL VALUES OTHERWISE)
             */
            UPDATE [dbo].[Client]
               SET [FirstNames]    = @FirstNames
                  ,[Surname]       = @Surname
                  ,[AddressType]   = @AddressType
                  ,[StreetAddress] = @StreetAddress
                  ,[Suburb]        = @Suburb
                  ,[City]          = @City
                  ,[PostalCode]    = @PostalCode
             WHERE [Id] = @Id
         END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
        THROW
    END CATCH

    IF @@TRANCOUNT > 0
        COMMIT TRANSACTION
END
