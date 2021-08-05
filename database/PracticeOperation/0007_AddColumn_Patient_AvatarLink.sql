USE [PatientCheckIn]

GO

IF COL_LENGTH('Patient', 'AvatarLink') IS NOT NULL
	BEGIN
		PRINT 'Column AvatarLink exist in table'
	END
ELSE
	BEGIN
		ALTER TABLE Patient
		ADD AvatarLink nvarchar(100)
	END