USE [PatientCheckIn]

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Contact'))
	BEGIN
		PRINT 'Table Contact exists'
	END

ELSE 
	BEGIN 
		CREATE TABLE [dbo].[Contact](
			[ContactID] [int] IDENTITY(1,1) NOT NULL,
			[PhoneNumber] [varchar](15) NOT NULL,
			[Email] [nvarchar](100) NOT NULL,
			[PatientID] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[ContactID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Contact]  WITH CHECK ADD FOREIGN KEY([PatientID])
		REFERENCES [dbo].[Patient] ([PatientID])
	END 