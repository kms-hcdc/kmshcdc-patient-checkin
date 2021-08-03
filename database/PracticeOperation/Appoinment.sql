IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
           WHERE TABLE_NAME = 'Appointment'))
	BEGIN
		PRINT 'Table Appointment exists'
	END
ELSE 
	BEGIN 
		create table Appointment (
			Appointment_ID int primary key identity(1,1),
			CheckInDate datetime not null,
			MedicalConcerns nvarchar(100),
			Status varchar(10) not null,
			Patient_ID int not null foreign key references Patient(Patient_ID)
		)
	END