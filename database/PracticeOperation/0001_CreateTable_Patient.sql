USE [PatientCheckIn];

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
SET ANSI_NULLS ON;

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Patient'))
	BEGIN
		PRINT 'Table Patient exists'
	END

ELSE 
	BEGIN 
		CREATE TABLE [dbo].[Patient](
			[PatientID] [int] IDENTITY(1,1) NOT NULL,
			[PatientIdentifier] [nvarchar](8) NOT NULL,
			[FirstName] [nvarchar](50) NOT NULL,
			[MiddleName] [nvarchar](50) NULL,
			[LastName] [nvarchar](50) NOT NULL,
			[FullName] [nvarchar](150) NOT NULL,
			[DoB] [date] NOT NULL,
			[Gender] [int] NOT NULL,
			[PhoneNumber] [varchar](15) NOT NULL,
			[Email] [nvarchar](100) NOT NULL,
			[MaritalStatus] [bit] NOT NULL,
			[Nationality] [nvarchar](100) NOT NULL,
			[EthnicRace] [nvarchar](50) NULL,
			[HomeTown] [nvarchar](50) NULL,
			[BirthplaceCity] [nvarchar](50) NULL,
			[IDCardNo] [varchar](20) NULL,
			[IssuedDate] [date] NULL,
			[IssuedPlace] [nvarchar](50) NULL,
			[InsuranceNo] [varchar](20) NULL,
			[AvatarLink] [nvarchar](300) NULL
		PRIMARY KEY CLUSTERED 
		(
			[PatientID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]
	END
