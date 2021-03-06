USE [PatientCheckIn];

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
SET ANSI_NULLS ON;

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'EmergencyContact'))
	BEGIN
		PRINT 'Table EmergencyContact exists'
	END

ELSE 
	BEGIN 
		CREATE TABLE [dbo].[EmergencyContact](
			[EmergencyID] [int] IDENTITY(1,1) NOT NULL,
			[Relationship] [varchar](20) NOT NULL,
			[Name] [nvarchar](150) NOT NULL,
			[PhoneNumber] [varchar](15) NOT NULL,
			[PatientID] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[EmergencyID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[EmergencyContact]  WITH CHECK ADD FOREIGN KEY([PatientID])
		REFERENCES [dbo].[Patient] ([PatientID])
	END