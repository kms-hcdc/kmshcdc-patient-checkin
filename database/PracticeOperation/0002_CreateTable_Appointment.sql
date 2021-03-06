USE [PatientCheckIn];

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
SET ANSI_NULLS ON;

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Appointment'))
	BEGIN
		PRINT 'Table Appointment exists'
	END

ELSE 
	BEGIN 
		CREATE TABLE [dbo].[Appointment](
			[AppointmentID] [int] IDENTITY(1,1) NOT NULL,
			[CheckInDate] [datetime] NOT NULL,
			[MedicalConcerns] [nvarchar](100) NULL,
			[Status] [varchar](10) NOT NULL,
			[PatientID] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[AppointmentID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD FOREIGN KEY([PatientID])
		REFERENCES [dbo].[Patient] ([PatientID])
	END