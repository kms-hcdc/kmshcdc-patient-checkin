IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'EmergencyContact'))
	BEGIN
		PRINT 'Table EmergencyContact exists'
	END
ELSE 
	BEGIN 
		create table EmergencyContact (
			Emergency_ID int primary key identity(1,1),
			Relationship varchar(20) not null,
			Name nvarchar(150) not null,
			PhoneNumber varchar(15) not null,
			Contact_ID int not null foreign key references Contact(Contact_ID)
		)
	END