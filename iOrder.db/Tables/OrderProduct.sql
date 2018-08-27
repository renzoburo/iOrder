CREATE TABLE [dbo].[OrderProduct]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT newid(),
    [OrderId] UNIQUEIDENTIFIER NOT NULL, 
    [ProductId] UNIQUEIDENTIFIER NOT NULL, 
    [Quantity] INT NOT NULL DEFAULT 1, 
    PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_OrderProduct_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order]([Id]),
    CONSTRAINT [FK_OrderProduct_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([Id])
)
