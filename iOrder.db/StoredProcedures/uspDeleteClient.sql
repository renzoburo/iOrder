﻿CREATE PROCEDURE [dbo].[uspDeleteClient]
    @Id UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    BEGIN TRANSACTION

    BEGIN TRY
        DELETE [dbo].[Client]
         WHERE [Id] = @Id

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
    END CATCH

    IF @@TRANCOUNT > 0
        COMMIT TRANSACTION
END