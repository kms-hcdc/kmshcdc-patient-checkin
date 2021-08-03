IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Address'))
	BEGIN
		PRINT 'Table Address exists'
	END
ELSE 
	BEGIN 
		create table Address (
			Address_ID int primary key identity(1,1),
			TypeAddress int not null,
			Address nvarchar(150) not null,
			IsPrimary bit not null,
			Contact_ID int not null foreign key references Contact(Contact_ID)
		)
	END