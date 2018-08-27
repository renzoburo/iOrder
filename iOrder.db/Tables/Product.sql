﻿CREATE TABLE [dbo].[Product]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(), 
    [Title] NVARCHAR(128) NULL, 
    [Description] NVARCHAR(1024) NOT NULL, 
    [Price] MONEY NOT NULL, 
    [Image] NVARCHAR(256) NULL
)
