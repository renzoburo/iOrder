CREATE TABLE [dbo].[Client]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(), 
    [FirstNames] NVARCHAR(128) NOT NULL, 
    [Surname] NVARCHAR(128) NOT NULL, 
    [AddressType] UNIQUEIDENTIFIER NOT NULL, 
    [StreetAddress] NVARCHAR(128) NOT NULL, 
    [Suburb] NVARCHAR(128) NOT NULL, 
    [City] NVARCHAR(128) NOT NULL, 
    [PostalCode] INT NOT NULL, 
    CONSTRAINT [FK_ClientToAddressType] FOREIGN KEY ([AddressType]) REFERENCES [dbo].[AddressType]([Id]) 
)
