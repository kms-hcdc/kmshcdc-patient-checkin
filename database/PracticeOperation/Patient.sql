IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Patient'))
	BEGIN
		PRINT 'Table Patient exists'
	END
ELSE 
	BEGIN 
		create table Patient (
			Patient_ID int primary key identity(1,1),
			PatientIdentifier nvarchar(8) not null,
			FirstName nvarchar(50) not null,
			MiddleName nvarchar(50),
			LastName nvarchar(50) not null,
			FullName nvarchar(150) not null,
			DoB date not null,
			Gender int not null,
			MaritalStatus bit not null,
			Nationality nvarchar(100) not null,
			EthnicRace nvarchar(50),
			HomeTown nvarchar(50),
			BirthplaceCity nvarchar(50),
			IDCardNo varchar(20),
			IssuedDate date,
			IssuedPlace nvarchar(50),
			InsuranceNo varchar(20)
		)
	END
