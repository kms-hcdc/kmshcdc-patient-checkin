IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Contact'))
	BEGIN
		PRINT 'Table Contact exists'
	END
ELSE 
	BEGIN 
		create table Contact (
			Contact_ID int primary key identity(1,1),
			PhoneNumber varchar(15) not null,
			Email nvarchar(100) not null,
			Patient_ID int not null foreign key references Patient(Patient_ID)
		)
	END