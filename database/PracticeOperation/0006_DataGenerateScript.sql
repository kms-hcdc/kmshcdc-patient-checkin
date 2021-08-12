Use [PatientCheckIn]

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET QUOTED_IDENTIFIER ON
SET NOCOUNT ON 
--
GO
--
INSERT INTO dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					VALUES
					('KMS.0001', 'Phien','Minh', 'Le','Le Minh Phien', '1994-05-25',
					0,0,'Vietnamese', 'Kinh')
INSERT INTO dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					VALUES
					('KMS.0002', 'Viet','Hoang', 'Vo','Vo Hoang Viet', '1996-10-08',
					0,0,'Vietnamese', 'Kinh')
INSERT INTO dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					VALUES
					('KMS.0003', 'Long','Thanh', 'Do','Do Thanh Long', '1999-11-09',
					0,0,'Vietnamese', 'Kinh')
INSERT INTO dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					VALUES
					('KMS.0004', 'Duc','Van', 'Tran','Tran Van Duc', '1999-05-10',
					0,0,'Vietnamese', 'Kinh')
-- 
go
--
INSERT INTO dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						VALUES 
						(GETDATE(), 'Headache', 'CheckIn', 1)
INSERT INTO dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						VALUES 
						(GETDATE(), 'Stomach Pain', 'CheckIn', 2)
INSERT INTO dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						VALUES 
						(GETDATE(), null, 'Closed', 3)
INSERT INTO dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						VALUES 
						(GETDATE(), 'Metal', 'Cancel', 4)
--
go
--
INSERT INTO dbo.Contact (PhoneNumber, Email, PatientID) 
					VALUES
					('0905512324', 'phienle@kms-technology.com', 1)
INSERT INTO dbo.Contact (PhoneNumber, Email, PatientID) 
					VALUES
					('0905879425', 'vietvo@kms-technology.com', 2)
INSERT INTO dbo.Contact (PhoneNumber, Email, PatientID) 
					VALUES
					('0934221311', 'longtdo@kms-technology.com', 3)
INSERT INTO dbo.Contact (PhoneNumber, Email, PatientID) 
					VALUES
					('0905879425', 'ducvant@kms-technology.com', 4)
--
go
--
INSERT INTO dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					VALUES
					(0, '34 Bui Vien, Quan Cam, Thanh Pho Ho Chi Minh', 1, 1)
INSERT INTO dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					VALUES
					(0, '124 Le Loi, Quan 1, Thanh Pho Ho Chi Minh', 1, 2)
INSERT INTO dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					VALUES
					(0, 'To 1, Hoa Son, Hoa Vang, Da Nang', 1, 3)
INSERT INTO dbo.Address (TypeAddress, Address, IsPrimary, ContactID)
					VALUES
					(0, 'To 2, Hoa Lien, Hoa Vang, Da Nang', 1, 4)
--
go
--
INSERT INTO dbo.EmergencyContact (Relationship, Name, PhoneNumber,ContactID)
							VALUES 
							('Father', 'Do Dinh Dao', '0905225121', 3)
INSERT INTO dbo.EmergencyContact (Relationship, Name, PhoneNumber,ContactID)
							VALUES 
							('Father', 'Tran Vinh', '0905225789', 4)