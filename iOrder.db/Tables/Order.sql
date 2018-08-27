CREATE TABLE [dbo].[Order]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(),
    [ClientId] UNIQUEIDENTIFIER NOT NULL,
    [DateCreated] DATETIME NOT NULL DEFAULT getdate(), 
    [DateUpdated] DATETIME NOT NULL DEFAULT getdate(),
    [Complete] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_ClientOrderProduct_Client] FOREIGN KEY ([ClientId]) REFERENCES [dbo].[Client]([Id]),
)
