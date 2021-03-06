USE [PatientCheckIn];

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
SET ANSI_NULLS ON;

GO

INSERT INTO dbo.Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, PhoneNumber, Email, MaritalStatus, Nationality, EthnicRace) 
					VALUES
					('KMS.0001', 'Phien','Minh', 'Le','Phien Minh Le', '1994-05-25',
					0,'0905512324', 'phienle@kms-technology.com', 0,'Vietnamese', 'Kinh'),
					('KMS.0002', 'Viet','Hoang', 'Vo','Viet Hoang Vo', '1996-10-08',
					0,'0905879425', 'vietvo@kms-technology.com', 0,'Vietnamese', 'Kinh'),
					('KMS.0003', 'Long','Thanh', 'Do','Long Thanh Do', '1999-11-09',
					0,'0934221311', 'longtdo@kms-technology.com', 0,'Vietnamese', 'Kinh'),
					('KMS.0004', 'Duc','Van', 'Tran','Duc Van Tran', '1999-05-10',
					0,'0905879425', 'ducvant@kms-technology.com', 0,'Vietnamese', 'Kinh');

GO

INSERT INTO dbo.Appointment (CheckInDate, MedicalConcerns, Status, PatientID) 
						VALUES 
						(GETDATE(), 'Headache', 'CheckIn', 1),
						(GETDATE(), 'Stomach Pain', 'CheckIn', 2),
						(GETDATE(), NULL, 'Closed', 3),
						(GETDATE(), 'Metal', 'Cancel', 4);

GO

INSERT INTO dbo.Address (TypeAddress, Address, IsPrimary, PatientID)
					VALUES
					(0, '34 Bui Vien, Quan Cam, Thanh Pho Ho Chi Minh', 1, 1),
					(0, '124 Le Loi, Quan 1, Thanh Pho Ho Chi Minh', 1, 2),
					(0, 'To 1, Hoa Son, Hoa Vang, Da Nang', 1, 3),
					(0, 'To 2, Hoa Lien, Hoa Vang, Da Nang', 1, 4);

GO

INSERT INTO dbo.EmergencyContact (Relationship, Name, PhoneNumber,PatientID)
							VALUES 
							('Father', 'Do Dinh Dao', '0905225121', 3),
							('Father', 'Tran Vinh', '0905225789', 4);