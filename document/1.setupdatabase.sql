create database NATemplate
GO
use NATemplate
GO
CREATE SCHEMA aut;
-- create index column physical
go
CREATE TABLE [aut].[Users](
	[id] bigint identity(1,1) primary key NOT NULL,
	[accessFailedCount] [int] NOT NULL,
	[concurrencyStamp] [nvarchar](max) NULL,
	[email] [nvarchar](256) NULL,
	[emailConfirmed] [bit] NOT NULL,
	[lockoutEnabled] [bit] NOT NULL,
	[lockoutEnd] [datetimeoffset](7) NULL,
	[normalizedEmail] [nvarchar](256) NULL,
	[normalizedUserName] [nvarchar](256) NULL,
	[passwordHash] [nvarchar](max) NULL,
	[phoneNumber] [nvarchar](max) NULL,
	[phoneNumberConfirmed] [bit] NOT NULL,
	[securityStamp] [nvarchar](max) NULL,
	[twoFactorEnabled] [bit] NOT NULL,
	[userName] [nvarchar](256) NULL,
	[data] nvarchar(max) null,
	[data_db] nvarchar(max) null,
	[files] [nvarchar](max) null
)
GO
CREATE TABLE [aut].[Roles](
	[id] bigint IDENTITY(1,1) primary key NOT NULL,
	[concurrencyStamp] [nvarchar](max) NULL,
	[name] [nvarchar](256) NULL,
	[normalizedName] [nvarchar](256) NULL,
	[data] nvarchar(max) null,
	[data_db] nvarchar(max) null
)
Go
CREATE TABLE [aut].[RoleClaims](
	[id] int IDENTITY(1,1) primary key NOT NULL,
	[claimType] [nvarchar](max) NULL,
	[claimValue] [nvarchar](max) NULL,
	[roleId] bigint references [aut].[Roles](id) ON DELETE CASCADE NOT NULL,	
)
GO
CREATE TABLE [aut].[UserClaims](
	[id] int IDENTITY(1,1) primary key NOT NULL,
	[claimType] [nvarchar](max) NULL,
	[claimValue] [nvarchar](max) NULL,
	[userId] bigint references [aut].[Users](id) ON DELETE CASCADE NOT NULL,	
)
GO
CREATE TABLE [aut].[UserLogins](
	[loginProvider] [nvarchar](450) NOT NULL,
	[providerKey] [nvarchar](450) NOT NULL,
	[providerDisplayName] [nvarchar](max) NULL,
	[userId] bigint references [aut].[Users](id) ON DELETE CASCADE NOT NULL
	primary key ([loginProvider],[providerKey])
)
GO
CREATE TABLE [aut].[UserRoles](
	[userId] bigint references [aut].[Users](id) ON DELETE CASCADE NOT NULL,
	[roleId] bigint references [aut].[Roles](id) ON DELETE CASCADE NOT NULL,
	primary key ([UserId],[RoleId])
)
GO
CREATE TABLE [aut].[UserTokens](
	[userId] bigint NOT NULL,
	[loginProvider] [nvarchar](450) NOT NULL,
	[name] [nvarchar](450) NOT NULL,
	[value] [nvarchar](max) NULL,
	primary key ([userId],[loginProvider],[name])
)
