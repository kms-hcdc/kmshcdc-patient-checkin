USE [PatientCheckIn]

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Address'))
	BEGIN
		PRINT 'Table Address exists'
	END

ELSE 
	BEGIN 
		CREATE TABLE [dbo].[Address](
			[AddressID] [int] IDENTITY(1,1) NOT NULL,
			[TypeAddress] [int] NOT NULL,
			[Address] [nvarchar](150) NOT NULL,
			[IsPrimary] [bit] NOT NULL,
			[ContactID] [int] NOT NULL,
		PRIMARY KEY CLUSTERED 
		(
			[AddressID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
		) ON [PRIMARY]

		ALTER TABLE [dbo].[Address]  WITH CHECK ADD FOREIGN KEY([ContactID])
		REFERENCES [dbo].[Contact] ([ContactID])
	END 
