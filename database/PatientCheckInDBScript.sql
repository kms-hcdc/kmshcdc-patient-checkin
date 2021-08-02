create database PatientCheckIn
--
go
--
use PatientCheckIn
--
go
--
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
--
go
--
create table Appointment (
			Appointment_ID int primary key identity(1,1),
			CheckInDate datetime not null,
			MedicalConcerns nvarchar(100),
			Status varchar(10) not null,
			Patient_ID int not null foreign key references Patient(Patient_ID)
)
--
go
--
create table Contact (
			Contact_ID int primary key identity(1,1),
			PhoneNumber varchar(15) not null,
			Email nvarchar(100) not null,
			Patient_ID int not null foreign key references Patient(Patient_ID)
)
--
go
--
create table Address (
			Address_ID int primary key identity(1,1),
			TypeAddress int not null,
			Address nvarchar(150) not null,
			IsPrimary bit not null,
			Contact_ID int not null foreign key references Contact(Contact_ID)
)
--
go
--
create table EmergencyContact (
			Emergency_ID int primary key identity(1,1),
			Relationship varchar(20) not null,
			Name nvarchar(150) not null,
			PhoneNumber varchar(15) not null,
			Contact_ID int not null foreign key references Contact(Contact_ID)
)
--
go
--
insert into Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0001', 'Phien','Minh', 'Le','Le Minh Phien', '1994-05-25',
					0,0,'Vietnamese', 'Kinh')
insert into Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0002', 'Viet','Hoang', 'Vo','Vo Hoang Viet', '1996-10-08',
					0,0,'Vietnamese', 'Kinh')
insert into Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0003', 'Long','Thanh', 'Do','Do Thanh Long', '1999-11-09',
					0,0,'Vietnamese', 'Kinh')
insert into Patient (PatientIdentifier, FirstName, MiddleName, LastName, FullName, DoB, 
					Gender, MaritalStatus, Nationality, EthnicRace) 
					values
					('KMS.0004', 'Duc','Van', 'Tran','Tran Van Duc', '1999-05-10',
					0,0,'Vietnamese', 'Kinh')
-- 
go
--
insert into Appointment (CheckInDate, MedicalConcerns, Status, Patient_ID) 
						values 
						(GETDATE(), 'Headache', 'CheckIn', 1)
insert into Appointment (CheckInDate, MedicalConcerns, Status, Patient_ID) 
						values 
						(GETDATE(), 'Stomach Pain', 'CheckIn', 2)
insert into Appointment (CheckInDate, MedicalConcerns, Status, Patient_ID) 
						values 
						(GETDATE(), null, 'Closed', 3)
insert into Appointment (CheckInDate, MedicalConcerns, Status, Patient_ID) 
						values 
						(GETDATE(), 'Metal', 'Cancel', 4)
--
go
--
insert into Contact (PhoneNumber, Email, Patient_ID) 
					values
					('0905512324', 'phienle@kms-technology.com', 1)
insert into Contact (PhoneNumber, Email, Patient_ID) 
					values
					('0905879425', 'vietvo@kms-technology.com', 2)
insert into Contact (PhoneNumber, Email, Patient_ID) 
					values
					('0934221311', 'longtdo@kms-technology.com', 3)
insert into Contact (PhoneNumber, Email, Patient_ID) 
					values
					('0905879425', 'ducvant@kms-technology.com', 4)
--
go
--
insert into Address (TypeAddress, Address, IsPrimary, Contact_ID)
					values
					(0, '34 Bui Vien, Quan Cam, Thanh Pho Ho Chi Minh', 1, 1)
insert into Address (TypeAddress, Address, IsPrimary, Contact_ID)
					values
					(0, '124 Le Loi, Quan 1, Thanh Pho Ho Chi Minh', 1, 2)
insert into Address (TypeAddress, Address, IsPrimary, Contact_ID)
					values
					(0, 'To 1, Hoa Son, Hoa Vang, Da Nang', 1, 3)
insert into Address (TypeAddress, Address, IsPrimary, Contact_ID)
					values
					(0, 'To 2, Hoa Lien, Hoa Vang, Da Nang', 1, 4)
--
go
--
insert into EmergencyContact (Relationship, Name, PhoneNumber,Contact_ID)
							values 
							('Father', 'Do Dinh Dao', '0905225121', 3)
insert into EmergencyContact (Relationship, Name, PhoneNumber,Contact_ID)
							values 
							('Father', 'Tran Vinh', '0905225789', 4)
