create database NATemplate
GO
use NATemplate
GO
Create TABLE [Template]
(
    [id] uniqueidentifier primary key not null,
    [info] nvarchar(max) null,
    [address] nvarchar(max) null,
    [data_db] nvarchar(max) null,
    [files] nvarchar(max) null
)