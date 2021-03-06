USE [IdealHiresStag]
GO
CREATE FUNCTION [dbo].[fn_Get_JobCategoryJob] (@JobId INT)
RETURNS VARCHAR(MAX)
AS BEGIN
 
	Declare @jobCatNameList Varchar(MAX);
	SELECT @jobCatNameList = COALESCE(@jobCatNameList + ', ' + Name, Name) From [dbo].[JobCategory]
	WHERE Id in (SELECT [JobCategoryId] FROM [dbo].[JobCategoryJob] WHERE [JobId]=@JobId)
	
	RETURN @jobCatNameList
END

GO
/****** Object:  UserDefinedFunction [dbo].[uf_UTCToEST]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].[uf_UTCToEST]
(
@UTC_Date datetime,
@WithDaylight bit=1
)
RETURNS DATETIME
AS
BEGIN
--DECLARE @UTC_Date DATETIME
--SET @UTC_Date = GETUTCDATE()
RETURN 
 CASE WHEN @WithDaylight=1 THEN
 DATEADD(hh,
   CASE WHEN YEAR(@UTC_Date) <= 2006 THEN 
                CASE WHEN
                      @UTC_Date >=  '4/' + CAST(ABS(8 - DATEPART(WEEKDAY,'4/1/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR))) % 7 + 1 AS VARCHAR) +  '/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR) + ' 2:00' AND
                      @UTC_Date < '10/' + CAST(32 - DATEPART(WEEKDAY,'10/31/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR)) AS VARCHAR) +  '/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR) + ' 2:00'
                THEN -5 ELSE -6 END
              ELSE
                CASE WHEN
                      @UTC_Date >= '3/' + CAST(ABS(8 - DATEPART(WEEKDAY,'3/1/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR))) % 7 + 8 AS VARCHAR) +  '/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR) + ' 2:00' AND
                      @UTC_Date <
                        '11/' + CAST(ABS(8 - DATEPART(WEEKDAY,'11/1/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR))) % 7 + 1 AS VARCHAR) +  '/'
    + CAST(YEAR(@UTC_Date) AS VARCHAR) + ' 2:00'
                THEN -4 ELSE -5 END
              END
   , @UTC_Date
   )
   ELSE
	DATEADD(hh, -5, @UTC_Date)
   END
END

GO
/****** Object:  Table [dbo].[Academics]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Academics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NOT NULL,
	[Major] [varchar](255) NULL,
	[Minor] [varchar](255) NULL,
	[InstituteName] [varchar](255) NOT NULL,
	[StartAt] [date] NULL,
	[EndAt] [date] NULL,
	[IsDegreeOrCertification] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsMinorDegree] [bit] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Action]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Action](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ActionName] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Address]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NOT NULL,
	[StreetAddress1] [varchar](255) NOT NULL,
	[StreetAddress2] [varchar](255) NULL,
	[City] [varchar](255) NOT NULL,
	[StateId] [int] NULL,
	[CountryId] [int] NULL,
	[ZipCode] [varchar](255) NOT NULL,
	[Phone1] [varchar](255) NOT NULL,
	[Phone2] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AddressEmployer]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AddressEmployer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EUserId] [int] NOT NULL,
	[StreetAddress1] [varchar](255) NOT NULL,
	[StreetAddress2] [varchar](255) NULL,
	[City] [varchar](255) NOT NULL,
	[State] [varchar](255) NOT NULL,
	[Country] [varchar](255) NOT NULL,
	[ZipCode] [varchar](255) NULL,
	[Phone1] [varchar](255) NULL,
	[Phone2] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AuditTrail]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditTrail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[EntityId] [int] NULL,
	[Orgobj] [varchar](255) NULL,
	[Modobj] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[City]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SID] [int] NULL,
	[Name] [varchar](max) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Company]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [varchar](255) NOT NULL,
	[Phone1] [varchar](50) NULL,
	[PhoneChecked] [bit] NULL,
	[Email] [varchar](255) NULL,
	[Location] [varchar](255) NULL,
	[Website] [varchar](255) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CompanyName] [varchar](255) NOT NULL,
	[IsAgree] [bit] NOT NULL,
	[ContactEmail] [varchar](255) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyAddress]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyAddress](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[AddressTypeId] [int] NULL,
	[ZipCode] [varchar](50) NULL,
	[CountryId] [int] NULL,
	[StateId] [int] NULL,
	[City] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyJobCreditDetail]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyJobCreditDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[CompanyPackageDetailsId] [int] NULL,
	[AvailableCredit] [int] NULL,
	[UsedCredit] [int] NULL,
	[LastUsedBy] [int] NULL,
	[LastUsedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyJobCreditDetailHistory]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyJobCreditDetailHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[AvailableCredit] [int] NULL,
	[UsedCredit] [int] NULL,
	[UsedBy] [int] NULL,
	[UsedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyLogo]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyLogo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[Img] [varbinary](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompanyPackageDetails]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyPackageDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobCreditId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[CompanyId] [int] NULL,
	[UserId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Country]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Currency]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NULL,
	[ShortCode] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmployerCompany]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployerCompany](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Entity]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityName] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Job]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[Title] [varchar](255) NULL,
	[Description] [varchar](max) NULL,
	[MinimumSalary] [decimal](18, 3) NULL,
	[MaximumSalary] [decimal](18, 3) NULL,
	[PayPeriodTypeId] [int] NULL,
	[CurrencyId] [int] NULL,
	[Positions] [varchar](255) NULL,
	[Location] [int] NULL,
	[LocationState] [varchar](50) NULL,
	[LocationCountry] [varchar](50) NULL,
	[ExpiredAt] [date] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[Assumption] [bit] NULL,
	[UnAssumption] [bit] NULL,
	[Status] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[Responsibility] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobCategory]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobCategoryJob]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobCategoryJob](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NULL,
	[JobCategoryId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobCategoryProfile]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobCategoryProfile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NULL,
	[JobCategoryId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [datetime] NULL,
	[UpdatedAt] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobCredit]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobCredit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [numeric](10, 2) NULL,
	[Duration] [varchar](50) NULL,
	[JobCredit] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[Discount] [numeric](4, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobType]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
	[IsActive] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobTypeJob]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobTypeJob](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NULL,
	[JobTypeId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobTypeProfile]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobTypeProfile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NULL,
	[JobTypeId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [datetime] NULL,
	[UpdatedAt] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Keywords]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Keywords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NULL,
	[Status] [bit] NULL,
	[UserId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeywordsJob]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeywordsJob](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[JobId] [int] NOT NULL,
	[Keywords] [varchar](200) NOT NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeywordsProfile]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeywordsProfile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[Keywords] [varchar](200) NOT NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Notification]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Title] [varchar](255) NULL,
	[Entity] [varchar](255) NULL,
	[EntityId] [int] NULL,
	[Status] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[IsRead] [bit] NULL,
	[CompanyId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](10) NOT NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NotificationTypeJob]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationTypeJob](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NULL,
	[NotificationTypeId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PayPeriodType]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayPeriodType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](10) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_PayPeriodType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permission]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[EntityId] [int] NULL,
	[ActionId] [int] NULL,
	[PermissionTypeId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PermissionType]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneFormat]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneFormat](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryId] [int] NULL,
	[Format] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Profile]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[JobTitle] [varchar](255) NOT NULL,
	[Objective] [varchar](255) NULL,
	[ResumeFile] [varchar](255) NULL,
	[IsWillingToRelocate] [bit] NULL,
	[MinimumSalary] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[OrgFileName] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProfileJob]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileJob](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NULL,
	[JobId] [int] NULL,
	[ActionId] [int] NULL,
	[UserId] [int] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserType] [varchar](10) NULL,
	[RoleName] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK__Role__3214EC07C7B393AD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RolePermission]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermission](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[PermissionId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaveSearch]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaveSearch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Type] [varchar](255) NULL,
	[RelativeLink] [varchar](255) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SortListedCandidate]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SortListedCandidate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[ProfileId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[IsSortListed] [bit] NOT NULL,
	[SortListedDate] [datetime] NOT NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[State]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[State](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CID] [int] NULL,
	[Name] [varchar](max) NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransactionDetails]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Amount] [numeric](18, 2) NULL,
	[TransactionId] [varchar](50) NULL,
	[TransactionDate] [datetime] NULL,
	[AccountType] [varchar](50) NULL,
	[Authorization] [nchar](10) NULL,
	[ErrorCode] [int] NULL,
	[ErrorMessage] [varchar](max) NULL,
	[Message] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](255) NULL,
	[LastName] [varchar](255) NULL,
	[EmailId] [varchar](255) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[UserType] [varchar](255) NULL,
	[IsActive] [bit] NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsEmailConfirm] [bit] NOT NULL,
	[Title] [varchar](10) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[RoleId] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Work]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Work](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NOT NULL,
	[CompanyName] [varchar](255) NOT NULL,
	[PositionName] [varchar](255) NOT NULL,
	[Description] [varchar](max) NULL,
	[Salary] [varchar](255) NULL,
	[CurrencyId] [int] NULL,
	[StartAt] [date] NULL,
	[EndAt] [date] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[PayPeriodTypeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Academics]  WITH CHECK ADD  CONSTRAINT [FK_Academics_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
GO
ALTER TABLE [dbo].[Academics] CHECK CONSTRAINT [FK_Academics_Profile]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Profile]
GO
ALTER TABLE [dbo].[AuditTrail]  WITH CHECK ADD FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entity] ([Id])
GO
ALTER TABLE [dbo].[AuditTrail]  WITH CHECK ADD FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entity] ([Id])
GO
ALTER TABLE [dbo].[AuditTrail]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[AuditTrail]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[CompanyAddress]  WITH CHECK ADD  CONSTRAINT [FK__CompanyAd__Compa__18D6A699] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CompanyAddress] CHECK CONSTRAINT [FK__CompanyAd__Compa__18D6A699]
GO
ALTER TABLE [dbo].[CompanyJobCreditDetail]  WITH CHECK ADD  CONSTRAINT [FK__CompanyJo__Compa__1C722D53] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CompanyJobCreditDetail] CHECK CONSTRAINT [FK__CompanyJo__Compa__1C722D53]
GO
ALTER TABLE [dbo].[CompanyJobCreditDetail]  WITH CHECK ADD FOREIGN KEY([CompanyPackageDetailsId])
REFERENCES [dbo].[CompanyPackageDetails] ([Id])
GO
ALTER TABLE [dbo].[CompanyJobCreditDetailHistory]  WITH CHECK ADD  CONSTRAINT [FK__CompanyJo__Compa__2042BE37] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CompanyJobCreditDetailHistory] CHECK CONSTRAINT [FK__CompanyJo__Compa__2042BE37]
GO
ALTER TABLE [dbo].[CompanyPackageDetails]  WITH CHECK ADD  CONSTRAINT [FK__CompanyPa__Compa__2136E270] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CompanyPackageDetails] CHECK CONSTRAINT [FK__CompanyPa__Compa__2136E270]
GO
ALTER TABLE [dbo].[CompanyPackageDetails]  WITH CHECK ADD FOREIGN KEY([JobCreditId])
REFERENCES [dbo].[JobCredit] ([Id])
GO
ALTER TABLE [dbo].[CompanyPackageDetails]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[EmployerCompany]  WITH CHECK ADD  CONSTRAINT [FK_EmployerCompany_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[EmployerCompany] CHECK CONSTRAINT [FK_EmployerCompany_Company]
GO
ALTER TABLE [dbo].[EmployerCompany]  WITH CHECK ADD  CONSTRAINT [FK_EmployerCompany_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[EmployerCompany] CHECK CONSTRAINT [FK_EmployerCompany_User]
GO
ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK__Job__CompanyId__2A363CC5] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK__Job__CompanyId__2A363CC5]
GO
ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK__Job__CurrencyId__2B2A60FE] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([Id])
GO
ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK__Job__CurrencyId__2B2A60FE]
GO
ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_PayPeriodType] FOREIGN KEY([PayPeriodTypeId])
REFERENCES [dbo].[PayPeriodType] ([Id])
GO
ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_PayPeriodType]
GO
ALTER TABLE [dbo].[JobCategoryJob]  WITH CHECK ADD FOREIGN KEY([JobCategoryId])
REFERENCES [dbo].[JobCategory] ([Id])
GO
ALTER TABLE [dbo].[JobCategoryJob]  WITH CHECK ADD  CONSTRAINT [fk_jobid] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobCategoryJob] CHECK CONSTRAINT [fk_jobid]
GO
ALTER TABLE [dbo].[JobCategoryProfile]  WITH CHECK ADD FOREIGN KEY([JobCategoryId])
REFERENCES [dbo].[JobCategory] ([Id])
GO
ALTER TABLE [dbo].[JobCategoryProfile]  WITH CHECK ADD  CONSTRAINT [FK_JobCatProfile_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobCategoryProfile] CHECK CONSTRAINT [FK_JobCatProfile_Profile]
GO
ALTER TABLE [dbo].[JobTypeJob]  WITH CHECK ADD FOREIGN KEY([JobTypeId])
REFERENCES [dbo].[JobType] ([Id])
GO
ALTER TABLE [dbo].[JobTypeJob]  WITH CHECK ADD  CONSTRAINT [fk_jobTyptbl_jobid] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobTypeJob] CHECK CONSTRAINT [fk_jobTyptbl_jobid]
GO
ALTER TABLE [dbo].[JobTypeProfile]  WITH CHECK ADD FOREIGN KEY([JobTypeId])
REFERENCES [dbo].[JobType] ([Id])
GO
ALTER TABLE [dbo].[JobTypeProfile]  WITH CHECK ADD  CONSTRAINT [FK_JobTypeProfile_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobTypeProfile] CHECK CONSTRAINT [FK_JobTypeProfile_Profile]
GO
ALTER TABLE [dbo].[Keywords]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[KeywordsJob]  WITH CHECK ADD  CONSTRAINT [FK_KeywordsJob_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
GO
ALTER TABLE [dbo].[KeywordsJob] CHECK CONSTRAINT [FK_KeywordsJob_Job]
GO
ALTER TABLE [dbo].[KeywordsJob]  WITH CHECK ADD  CONSTRAINT [fk_KeywordsJobtbl_jobid] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[KeywordsJob] CHECK CONSTRAINT [fk_KeywordsJobtbl_jobid]
GO
ALTER TABLE [dbo].[KeywordsProfile]  WITH CHECK ADD  CONSTRAINT [FK_KeywordsProfile_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[KeywordsProfile] CHECK CONSTRAINT [FK_KeywordsProfile_Profile]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entity] ([Id])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[NotificationTypeJob]  WITH CHECK ADD  CONSTRAINT [FK_NotificationTypeJob_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
GO
ALTER TABLE [dbo].[NotificationTypeJob] CHECK CONSTRAINT [FK_NotificationTypeJob_Job]
GO
ALTER TABLE [dbo].[NotificationTypeJob]  WITH CHECK ADD  CONSTRAINT [FK_NotificationTypeJob_NotificationType] FOREIGN KEY([NotificationTypeId])
REFERENCES [dbo].[NotificationType] ([Id])
GO
ALTER TABLE [dbo].[NotificationTypeJob] CHECK CONSTRAINT [FK_NotificationTypeJob_NotificationType]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([ActionId])
REFERENCES [dbo].[Action] ([Id])
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([ActionId])
REFERENCES [dbo].[Action] ([Id])
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entity] ([Id])
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([EntityId])
REFERENCES [dbo].[Entity] ([Id])
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([PermissionTypeId])
REFERENCES [dbo].[PermissionType] ([Id])
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD FOREIGN KEY([PermissionTypeId])
REFERENCES [dbo].[PermissionType] ([Id])
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [FK__Permissio__RoleI__420DC656] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [FK__Permissio__RoleI__420DC656]
GO
ALTER TABLE [dbo].[PhoneFormat]  WITH CHECK ADD FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[PhoneFormat]  WITH CHECK ADD FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([Id])
GO
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD FOREIGN KEY([ActionId])
REFERENCES [dbo].[Action] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD FOREIGN KEY([ActionId])
REFERENCES [dbo].[Action] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD  CONSTRAINT [FK__ProfileJo__JobId__44EA3301] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob] CHECK CONSTRAINT [FK__ProfileJo__JobId__44EA3301]
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[ProfileJob]  WITH CHECK ADD  CONSTRAINT [fk_ProfileJobtbl_jobid] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProfileJob] CHECK CONSTRAINT [fk_ProfileJobtbl_jobid]
GO
ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
GO
ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permission] ([Id])
GO
ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD  CONSTRAINT [FK__RolePermi__RoleI__48BAC3E5] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[RolePermission] CHECK CONSTRAINT [FK__RolePermi__RoleI__48BAC3E5]
GO
ALTER TABLE [dbo].[SaveSearch]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[SaveSearch]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[SortListedCandidate]  WITH CHECK ADD  CONSTRAINT [FK_SortListedCandidate_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[SortListedCandidate] CHECK CONSTRAINT [FK_SortListedCandidate_Company]
GO
ALTER TABLE [dbo].[SortListedCandidate]  WITH CHECK ADD  CONSTRAINT [FK_SortListedCandidate_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([Id])
GO
ALTER TABLE [dbo].[SortListedCandidate] CHECK CONSTRAINT [FK_SortListedCandidate_Job]
GO
ALTER TABLE [dbo].[SortListedCandidate]  WITH CHECK ADD  CONSTRAINT [FK_SortListedCandidate_Profile] FOREIGN KEY([ProfileId])
REFERENCES [dbo].[Profile] ([Id])
GO
ALTER TABLE [dbo].[SortListedCandidate] CHECK CONSTRAINT [FK_SortListedCandidate_Profile]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK__UserRole__RoleId__4B973090] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK__UserRole__RoleId__4B973090]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[Usp_Get_IHDashboard_Result]    Script Date: 4/19/2019 10:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create Procedure  [dbo].[Usp_Get_IHDashboard_Result]

As
Begin
-----Declare Parameter For Post New Jobs-----
declare @TodayNewJobPost int,@WeekJobPost int,@MonthJobPost int,@YearJobPost int,@WeekExpiredJob int,
@MonthExpiredJob int,@YearJobExpired int,@TotalJobPost int,@TotalExpiredJobPost int,@TotalToday int,@TotalWeek int,@TotalMonth int,
@TotalYear int, @AllTotal int,
 -----Declare Parameter For Modified Jobs-----
@TodayModifiedJobPost int,@WeekModifiedJobPost int,@MonthModifiedJobPost int,@ModifiedYearJobPost int,@TotalModifiedJobPost int,
@MTotalToday int,@MTotalWeek int,@MTotalMonth int,@MTotalYear int,@AllModifiedTotal int ,
 -----Declare Parameter For New Employers-----
 @TodayProfile int,@WeekProfile int,@MonthProfile int,@YearProfile int,@TotalProfile int,@TodayProfileNot int,
 @WeekProfileNot int,@MonthProfileNot int,@YearProfileNot int,@TotalProfileNot int,@TotalTProfileRow int,@TotalWRow int,
 @TotalMRow int,@TotalYRow int,@AllTotalcolumn int;
---Set Parameter For Post New Jobs---
set @TodayNewJobPost=(select count(CreatedAt)  from job where IsActive=1 and CONVERT(date,CreatedAt) = CONVERT(date,getdate())); 
set @WeekJobPost=(select count(CreatedAt)  from job where IsActive=1 and CreatedAt >= DATEADD(day,-7, GETDATE()));
 set @MonthjobPost=(select count(CreatedAt) from job where IsActive=1 and CreatedAt >= dateadd(month, datediff(month, 0, getdate())-1, 0)
 and CreatedAt <  dateadd(month, datediff(month, 0, getdate()), 0));
set @YearJobPost=(select count(CreatedAt) from job where IsActive=1 and CreatedAt >= dateadd(Year, datediff(year, 0, getdate())-1, 0)
 and CreatedAt <  dateadd(year, datediff(year, 0, getdate()), 0)); 
 set @WeekExpiredJob=(select count(ExpiredAt)  from job where ExpiredAt between DateAdd(DD,-7,GETDATE() ) and GETDATE());
 set @MonthExpiredJob=(select count(ExpiredAt)  from job where IsActive=1 and ExpiredAt between DateAdd(MONTH,-1,GETDATE() ) and GETDATE())
 set @YearJobExpired=(select Count(ExpiredAt) from job where IsActive=1 and ExpiredAt >= dateadd(Year, datediff(year, 0, getdate())-1, 0)
 and ExpiredAt <  dateadd(year, datediff(year, 0, getdate()), 0));
 set @TotalJobPost= @TodayNewJobPost + @WeekJobPost +@MonthjobPost+@YearJobPost
 set @TotalExpiredJobPost=0 +@WeekExpiredJob+@MonthExpiredJob+@YearJobExpired
 set @TotalToday=@TodayNewJobPost+0
 set @TotalWeek=@WeekJobPost+@WeekExpiredJob
 set @TotalMonth=@MonthjobPost+@MonthExpiredJob
 set @TotalYear=@YearJobPost+@YearJobExpired
 set @AllTotal=@TotalToday+@TotalWeek+@TotalMonth+@TotalYear
 ---Set Parameter For Modified Jobs-----
 set @TodayModifiedJobPost=(select count(UpdatedAt)  from job where IsActive=1 and CONVERT(date,UpdatedAt) = CONVERT(date,getdate()));
set @WeekModifiedJobPost=(select count(UpdatedAt)  from job where IsActive=1 and UpdatedAt >= DATEADD(day,-7, GETDATE()));
set @MonthModifiedJobPost=(select count(UpdatedAt) from job where IsActive=1 and UpdatedAt >= dateadd(month, datediff(month, 0, getdate())-1, 0)
 and UpdatedAt <  dateadd(month, datediff(month, 0, getdate()), 0))
 set @ModifiedYearJobPost=(select count(UpdatedAt) from job where IsActive=1 and UpdatedAt >= dateadd(Year, datediff(year, 0, getdate())-1, 0)
 and UpdatedAt <  dateadd(year, datediff(year, 0, getdate()), 0)); 
 set @TotalModifiedJobPost=@TodayModifiedJobPost+@WeekModifiedJobPost+@MonthModifiedJobPost+@ModifiedYearJobPost
 set @MTotalToday=@TodayModifiedJobPost+0
 set @MTotalWeek=@WeekModifiedJobPost+@WeekExpiredJob
 set @MTotalMonth=@MonthModifiedJobPost+@MonthExpiredJob
 set @MTotalYear=@ModifiedYearJobPost + @YearJobExpired
 set @AllModifiedTotal=@MTotalToday+@MTotalWeek+@MTotalMonth+@MTotalYear


  ----Set Parameter For New Employers-----
  set @TodayProfile=(select count(CreatedAt)  from Company where IsActive=1 and CONVERT(date,CreatedAt) = CONVERT(date,getdate()))
  set @WeekProfile=(select count(CreatedAt)  from Company where IsActive=1 and CreatedAt >= DATEADD(day,-7, GETDATE()))
  set @MonthProfile=(select count(CreatedAt) from Company where IsActive=1 and CreatedAt >= dateadd(month, datediff(month, 0, getdate())-1, 0)
 and CreatedAt <  dateadd(month, datediff(month, 0, getdate()), 0));
 set @YearProfile=(select count(CreatedAt) from Company where IsActive=1 and CreatedAt >= dateadd(Year, datediff(year, 0, getdate())-1, 0)
 and CreatedAt <  dateadd(year, datediff(year, 0, getdate()), 0));
 set @TodayProfileNot=(select count(CreatedAt) from [dbo].[user] where FirstName='' and UserType='Employer' and IsActive=1 and CONVERT(date,CreatedAt) = CONVERT(date,getdate()))
 set @WeekProfileNot=(select count(CreatedAt) from [dbo].[user] where FirstName='' and UserType='Employer'and IsActive=1 and CreatedAt >= DATEADD(day,-7, GETDATE()))
 set @MonthProfileNot=(select count(CreatedAt) from [dbo].[user] where FirstName='' and UserType='Employer'and IsActive=1 and CreatedAt >= dateadd(month, datediff(month, 0, getdate())-1, 0)
 and CreatedAt <  dateadd(month, datediff(month, 0, getdate()), 0));
 set @YearProfileNot=(select count(CreatedAt) from [dbo].[user] where FirstName='' and UserType='Employer'and IsActive=1 and CreatedAt >= dateadd(Year, datediff(year, 0, getdate())-1, 0)
 and CreatedAt <  dateadd(year, datediff(year, 0, getdate()), 0));

 set @TotalProfile=@TodayProfile+ @WeekProfile+ @MonthProfile+ @YearProfile 
 set @TotalProfileNot=@TodayProfileNot+@WeekProfileNot+@MonthProfileNot+@YearProfileNot
 set @TotalTProfileRow=@TodayProfile+@TodayProfileNot
  set @TotalWRow=@WeekProfile+@WeekProfileNot
 set @TotalMRow =@MonthProfile+@MonthProfileNot
 set @TotalYRow =@YearProfile+@YearProfileNot
 set @AllTotalcolumn=@TotalTProfileRow+ @TotalWRow+@TotalMRow+@TotalYRow

select @TodayNewJobPost as TodayJobPost,@WeekJobPost as PreviousWeekJob,@MonthjobPost  as PreviousMonthJob, @YearJobPost as PreviousYearJob, 
@WeekExpiredJob as PreviousWeekExpiredJob,@MonthExpiredJob as PreviousMonthExpiredJob,@YearJobExpired as PreviousYearJobExpired,
@TotalJobPost as TotalJob,@TotalExpiredJobPost as TotalExpiredJob,@TotalToday as TotalToday,@TotalWeek as TotalWeek,@TotalMonth
as TotalMonth,@TotalYear as TotalYear, @AllTotal as AllTotal,@TodayModifiedJobPost as TodayModifiedJobPost,@WeekModifiedJobPost as WeekModifiedJobPost,@MonthModifiedJobPost as MonthModifiedJob,@ModifiedYearJobPost as ModifiedYearJobPost,
@TotalModifiedJobPost as TotalModifiedJobPost,@MTotalToday as MTotalToday,@MTotalWeek as MTotalWeek,@MTotalMonth as MTotalMonth,@MTotalYear as MTotalYear,@AllModifiedTotal as MAllTotal,@TodayProfile as TodayProfile ,
@WeekProfile as PreviousWeekprofile,@MonthProfile as PreviousMonthProfile,@YearProfile as PreviousYearProfile,@TotalProfile as TotalProfile,
@TodayProfileNot as TodayProfileNot,@WeekProfileNot as PreviousWeekProfileNot,@MonthProfileNot as PreviousMonthProfileNot ,@YearProfileNot as PreviousYearProfileNot,
@TotalProfileNot as TotalProfileNot,@TotalTProfileRow as TotalTProfileRow,@TotalWRow as TotalWRow,@TotalMRow as TotalMRow,
@TotalYRow as TotalYRow,@AllTotalcolumn as AllTotalcolumn
End


GO
USE [master]
GO
ALTER DATABASE [IdealHiresStag] SET  READ_WRITE 
GO
