USE [test]
GO
/****** Object:  Table [dbo].[WebSafe]    Script Date: 12/13/2014 16:58:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebSafe](
	[LoginName] [nvarchar](50) NOT NULL,
	[LoginSalt] [nvarchar](10) NOT NULL,
	[LoginPass] [nvarchar](50) NOT NULL,
	[CreateTS] [bigint] NOT NULL,
	[UpdateTS] [bigint] NOT NULL,
 CONSTRAINT [PK_InnerShopWebSafe] PRIMARY KEY CLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KeyWordsLib]    Script Date: 12/13/2014 16:58:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyWordsLib](
	[UrlKey] [nvarchar](50) NOT NULL,
	[KeyWord] [nvarchar](20) NOT NULL,
	[KeyLength] [int] NOT NULL,
	[KeyWeight] [int] NOT NULL,
	[Stat] [int] NOT NULL,
	[CreateTS] [bigint] NOT NULL,
	[UpdateTS] [bigint] NOT NULL,
 CONSTRAINT [PK_KeyWordsLib] PRIMARY KEY CLUSTERED 
(
	[UrlKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_KeyWordsLib_KeyWord] UNIQUE NONCLUSTERED 
(
	[KeyWord] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IntroLib]    Script Date: 12/13/2014 16:58:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntroLib](
	[UrlKey] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Intro] [nvarchar](max) NOT NULL,
	[Stat] [int] NOT NULL,
	[CreateTS] [bigint] NOT NULL,
	[UpdateTS] [bigint] NOT NULL,
 CONSTRAINT [PK_IntroLib] PRIMARY KEY CLUSTERED 
(
	[UrlKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryLib]    Script Date: 12/13/2014 16:58:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryLib](
	[UrlKey] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Icon] [nvarchar](250) NOT NULL,
	[AttJson] [nvarchar](350) NOT NULL,
	[ViewOrder] [bigint] NOT NULL,
	[Stat] [int] NOT NULL,
	[CreateTS] [bigint] NOT NULL,
	[UpdateTS] [bigint] NOT NULL,
 CONSTRAINT [PK_CategoryLib] PRIMARY KEY CLUSTERED 
(
	[UrlKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



--admin admin888
              
INSERT INTO WebSafe (LoginName,LoginSalt,LoginPass,CreateTS,UpdateTS)
values ('admin','C3ED740FA8','XE889GwjXRuwQ+NsHvJei4t72bCeboOSFpwzCb25n/Y=',
20141208154515289,20141208154515289)