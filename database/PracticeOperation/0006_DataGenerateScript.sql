SET QUOTED_IDENTIFIER ON
SET NOCOUNT ON 
Use [PatientCheckIn]
--
GO
--
insert into dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0001', 'Phien','Minh', 'Le','Le Minh Phien', '1994-05-25',
					0,0,'Vietnamese', 'Kinh')
insert into dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0002', 'Viet','Hoang', 'Vo','Vo Hoang Viet', '1996-10-08',
					0,0,'Vietnamese', 'Kinh')
insert into dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0003', 'Long','Thanh', 'Do','Do Thanh Long', '1999-11-09',
					0,0,'Vietnamese', 'Kinh')
insert into dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0004', 'Duc','Van', 'Tran','Tran Van Duc', '1999-05-10',
					0,0,'Vietnamese', 'Kinh')
-- 
go
--
insert into dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						values 
						(GETDATE(), 'Headache', 'CheckIn', 1)
insert into dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						values 
						(GETDATE(), 'Stomach Pain', 'CheckIn', 2)
insert into dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						values 
						(GETDATE(), null, 'Closed', 3)
insert into dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						values 
						(GETDATE(), 'Metal', 'Cancel', 4)
--
go
--
insert into dbo.Contact (PhoneNumber, Email, PatientID) 
					values
					('0905512324', 'phienle@kms-technology.com', 1)
insert into dbo.Contact (PhoneNumber, Email, PatientID) 
					values
					('0905879425', 'vietvo@kms-technology.com', 2)
insert into dbo.Contact (PhoneNumber, Email, PatientID) 
					values
					('0934221311', 'longtdo@kms-technology.com', 3)
insert into dbo.Contact (PhoneNumber, Email, PatientID) 
					values
					('0905879425', 'ducvant@kms-technology.com', 4)
--
go
--
insert into dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					values
					(0, '34 Bui Vien, Quan Cam, Thanh Pho Ho Chi Minh', 1, 1)
insert into dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					values
					(0, '124 Le Loi, Quan 1, Thanh Pho Ho Chi Minh', 1, 2)
insert into dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					values
					(0, 'To 1, Hoa Son, Hoa Vang, Da Nang', 1, 3)
insert into dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					values
					(0, 'To 2, Hoa Lien, Hoa Vang, Da Nang', 1, 4)
--
go
--
insert into dbo.EmergencyContact (Relationship, Name, PhoneNumber,ContactID)
							values 
							('Father', 'Do Dinh Dao', '0905225121', 3)
insert into dbo.EmergencyContact (Relationship, Name, PhoneNumber,ContactID)
							values 
							('Father', 'Tran Vinh', '0905225789', 4)