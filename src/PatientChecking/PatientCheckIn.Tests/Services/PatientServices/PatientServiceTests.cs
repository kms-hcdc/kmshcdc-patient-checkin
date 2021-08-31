using Microsoft.EntityFrameworkCore;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.Services.Patient;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Services.PatientServices
{
    public class PatientServiceTests
    {
        [Fact]
        public async void GetListPatientPaging_SortByID_ReturnsPatientList()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);
            var request = PagingDataTest(1, 2, 0);

            var expected = GetPatientListDataExpected(patients, addresses, request.SortSelection);

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.GetListPatientPaging(request);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Patients.Count, actual.Patients.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            Assert.True(expected.Patients.All(x => actual.Patients.Any(y => x.PatientId == y.PatientId
                                                                         && x.PatientIdentifier == y.PatientIdentifier
                                                                         && x.FullName == y.FullName
                                                                         && x.DoB == y.DoB
                                                                         && x.Gender == y.Gender
                                                                         && x.PrimaryAddress.StreetLine == y.PrimaryAddress.StreetLine
                                                                         && x.PhoneNumber == y.PhoneNumber
                                                                         && x.Email == y.Email
                                                                         && x.AvatarLink == y.AvatarLink)));
        }

        [Fact]
        public async void GetListPatientPaging_SortByName_ReturnsPatientList()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);
            var request = PagingDataTest(1, 2, 1);

            var expected = GetPatientListDataExpected(patients, addresses, request.SortSelection);

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.GetListPatientPaging(request);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Patients.Count, actual.Patients.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            Assert.True(expected.Patients.All(x => actual.Patients.Any(y => x.PatientId == y.PatientId
                                                                         && x.PatientIdentifier == y.PatientIdentifier
                                                                         && x.FullName == y.FullName
                                                                         && x.DoB == y.DoB
                                                                         && x.Gender == y.Gender
                                                                         && x.PrimaryAddress.StreetLine == y.PrimaryAddress.StreetLine
                                                                         && x.PhoneNumber == y.PhoneNumber
                                                                         && x.Email == y.Email
                                                                         && x.AvatarLink == y.AvatarLink)));
        }

        [Fact]
        public async void GetListPatientPaging_SortByDoB_ReturnsPatientList()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);
            var request = PagingDataTest(1, 2, 2);

            var expected = GetPatientListDataExpected(patients, addresses, request.SortSelection);

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.GetListPatientPaging(request);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Patients.Count, actual.Patients.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            Assert.True(expected.Patients.All(x => actual.Patients.Any(y => x.PatientId == y.PatientId
                                                                         && x.PatientIdentifier == y.PatientIdentifier
                                                                         && x.FullName == y.FullName
                                                                         && x.DoB == y.DoB
                                                                         && x.Gender == y.Gender
                                                                         && x.PrimaryAddress.StreetLine == y.PrimaryAddress.StreetLine
                                                                         && x.PhoneNumber == y.PhoneNumber
                                                                         && x.Email == y.Email
                                                                         && x.AvatarLink == y.AvatarLink)));
        }

        [Fact]
        public async void GetPatientInDetail_ReturnsPatient()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            var expected = patients[3];

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.GetPatientInDetail(2);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.PatientId, actual.PatientId);
            Assert.Equal(expected.PatientIdentifier, actual.PatientIdentifier);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.MiddleName, actual.MiddleName);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.FullName, actual.FullName);
            Assert.Equal(expected.DoB, actual.DoB);
            Assert.Equal(expected.Gender, actual.Gender);
            Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.MaritalStatus, actual.MaritalStatus);
            Assert.Equal(expected.Nationality, actual.Nationality);
            Assert.Equal(expected.EthnicRace, actual.EthnicRace);
            Assert.Equal(expected.HomeTown, actual.HomeTown);
            Assert.Equal(expected.BirthplaceCity, actual.BirthplaceCity);
            Assert.Equal(expected.IdcardNo, actual.IdcardNo);
            Assert.Equal(expected.IssuedPlace, actual.IssuedPlace);
            Assert.Equal(expected.IssuedDate, actual.IssuedDate);
            Assert.Equal(expected.InsuranceNo, actual.InsuranceNo);
        }

        [Fact]
        public async void GetPatientsSummary_ReturnsNumberOfPatients()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            var expected = patients.Count;

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.GetPatientsSummary();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UpdatePatientDetail_Ok_ReturnsNumberOfEffectedRow()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            var modifiedPatient = patients[1];

            modifiedPatient.FirstName = "Hung";
            modifiedPatient.MiddleName = "Viet";
            modifiedPatient.LastName = "Nguyen";
            modifiedPatient.FullName = "Hung Viet Nguyen";
            modifiedPatient.DoB = new DateTime(1992, 10, 09);
            modifiedPatient.Gender = 0;
            modifiedPatient.MaritalStatus = true;
            modifiedPatient.Nationality = "Vietnamese";
            modifiedPatient.EthnicRace = "Kinh";
            modifiedPatient.HomeTown = "Soc Trang";
            modifiedPatient.BirthplaceCity = "Can Tho";
            modifiedPatient.IdcardNo = "201376855";
            modifiedPatient.IssuedDate = new DateTime(2012, 01, 14);
            modifiedPatient.IssuedPlace = "Can Tho";
            modifiedPatient.InsuranceNo = "201329231";

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.UpdatePatientDetail(modifiedPatient);

            //Assert
            Assert.True(actual == 1);
        }

        [Fact]
        public async void UpdatePatientDetail_NullParameter_ReturnsNumberOfEffectedRow()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.UpdatePatientDetail(null);

            //Assert
            Assert.True(actual == -1);
        }

        [Fact]
        public async void UploadPatientImage_Ok_ReturnsNumberOfEffectedRow()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.UploadPatientImage(patients[0].PatientId, "/Image/Profile.jpg");

            //Assert
            Assert.True(actual == 1);
        }

        [Fact]
        public async void UploadPatientImage_NotOk_ReturnsNumberOfEffectedRow()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.UploadPatientImage(-1, "/Image/Profile.jpg");

            //Assert
            Assert.True(actual == -1);
        }

        private PatientCheckInContext CreateMockContext(List<Patient> patients)
        {
            var builder = new DbContextOptionsBuilder<PatientCheckInContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            DbContextOptions<PatientCheckInContext> options = builder.Options;

            var context = new PatientCheckInContext(options);
            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();

            if (patients != null)
            {
                context.AddRange(patients);
            }

            context.SaveChanges();

            return context;
        }

        private List<Patient> PatientDataTest(List<Address> addresses)
        {
            var patients = new List<Patient>
            {
                new Patient {PatientId = 4, PatientIdentifier = "KMS.0004", FirstName = "Viet", MiddleName = "Hoang", LastName = "Vo", FullName = "Viet Hoang Vo",
                            DoB = new DateTime(1995,10,08), Gender = 0, PhoneNumber = "0905879111", Email = "vietvo@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Ben Tre", BirthplaceCity = "Ben Tre", IdcardNo = "829123242",
                            IssuedDate = new DateTime(2010, 12, 16), IssuedPlace = "Ho Chi Minh", InsuranceNo = "203125445", AvatarLink = null,
                            Addresses = new List<Address> { addresses[3] } },

                new Patient {PatientId = 1, PatientIdentifier = "KMS.0001", FirstName = "Long", MiddleName = "Thanh", LastName = "Do", FullName = "Long Thanh Do",
                            DoB = new DateTime(1999,11,09), Gender = 0, PhoneNumber = "0905512324", Email = "longtdo@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Da Nang", BirthplaceCity = "Ho Chi Minh", IdcardNo = "201754622",
                            IssuedDate = new DateTime(2014, 09, 14), IssuedPlace = "Da Nang", InsuranceNo = "201329231", AvatarLink = "/Image/avatar.jpg",
                            Addresses = new List<Address> { addresses[0] } },

                new Patient {PatientId = 3, PatientIdentifier = "KMS.0003", FirstName = "Phien", MiddleName = "Minh", LastName = "Le", FullName = "Phien Minh Le",
                            DoB = new DateTime(1987,06,12), Gender = 0, PhoneNumber = "0905879425", Email = "phienle@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Can Tho", BirthplaceCity = "Can Tho", IdcardNo = "021252358",
                            IssuedDate = new DateTime(2011, 11, 16), IssuedPlace = "Ho Chi Minh", InsuranceNo = "203431326", AvatarLink = "/Image/PhienProfile.jpg",
                            Addresses = new List<Address> { addresses[2] } },

                new Patient {PatientId = 2, PatientIdentifier = "KMS.0002", FirstName = "Duc", MiddleName = "Van", LastName = "Tran", FullName = "Duc Van Tran",
                            DoB = new DateTime(1999,05,10), Gender = 0, PhoneNumber = "0905879123", Email = "ducvant@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Da Nang", BirthplaceCity = "Da Nang", IdcardNo = "201251266",
                            IssuedDate = new DateTime(2013, 08, 16), IssuedPlace = "Da Nang", InsuranceNo = "203125325", AvatarLink = null,
                            Addresses = new List<Address> { addresses[1] } }
            };

            return patients;
        }

        private List<Address> AddressDataTest()
        {
            var addresses = new List<Address>
            {
                new Address {AddressId = 1, TypeAddress = 0, StreetLine = "To 1, Hoa Son, Hoa Vang, Da Nang", IsPrimary = true, PatientId = 1},
                new Address {AddressId = 2, TypeAddress = 0, StreetLine = "To 2, Hoa Lien, Hoa Vang, Da Nang", IsPrimary = true, PatientId = 2},
                new Address {AddressId = 3, TypeAddress = 0, StreetLine = "34 Bui Vien, Quan Cam, Thanh Pho Ho Chi Minh", IsPrimary = true, PatientId = 3},
                new Address {AddressId = 4, TypeAddress = 0, StreetLine = "124 Le Loi, Quan 1, Thanh Pho Ho Chi Minh", IsPrimary = true, PatientId = 4},
            };

            return addresses;
        }

        private PagingRequest PagingDataTest(int index, int size, int sort)
        {
            var request = new PagingRequest
            {
                PageIndex = index,
                PageSize = size,
                SortSelection = sort
            };

            return request;
        }

        private PatientChecking.ServiceModels.PatientList GetPatientListDataExpected(List<Patient> patients, List<Address> addresses, int sortOption)
        {
            var patientsServiceModelSortById = new List<PatientChecking.ServiceModels.Patient>
            {
                new PatientChecking.ServiceModels.Patient { PatientId = 1, PatientIdentifier = "KMS.0001", FullName = "Long Thanh Do", DoB = new DateTime(1999,11,09),
                                                            Gender = 0, Email = "longtdo@kms-technology.com", PhoneNumber = "0905512324", AvatarLink = "/Image/avatar.jpg",
                                                            PrimaryAddress = addresses[0]},
                new PatientChecking.ServiceModels.Patient { PatientId = 2, PatientIdentifier = "KMS.0002", FullName = "Duc Van Tran", DoB = new DateTime(1999,05,10),
                                                            Gender = 0, Email = "ducvant@kms-technology.com", PhoneNumber = "0905879123", AvatarLink = "",
                                                            PrimaryAddress = addresses[1]}
            };

            var patientsServiceModelSortByName = new List<PatientChecking.ServiceModels.Patient>
            {
                new PatientChecking.ServiceModels.Patient { PatientId = 2, PatientIdentifier = "KMS.0002", FullName = "Duc Van Tran", DoB = new DateTime(1999,05,10),
                                                            Gender = 0, Email = "ducvant@kms-technology.com", PhoneNumber = "0905879123", AvatarLink = "",
                                                            PrimaryAddress = addresses[1]},
                new PatientChecking.ServiceModels.Patient { PatientId = 1, PatientIdentifier = "KMS.0001", FullName = "Long Thanh Do", DoB = new DateTime(1999,11,09),
                                                            Gender = 0, Email = "longtdo@kms-technology.com", PhoneNumber = "0905512324", AvatarLink = "/Image/avatar.jpg",
                                                            PrimaryAddress = addresses[0]}
            };

            var patientsServiceModelSortByDob = new List<PatientChecking.ServiceModels.Patient>
            {
                new PatientChecking.ServiceModels.Patient { PatientId = 3, PatientIdentifier = "KMS.0003", FullName = "Phien Minh Le", DoB = new DateTime(1987,06,12),
                                                            Gender = 0, Email = "phienle@kms-technology.com", PhoneNumber = "0905879425", AvatarLink = "/Image/PhienProfile.jpg",
                                                            PrimaryAddress = addresses[2]},
                new PatientChecking.ServiceModels.Patient { PatientId = 4, PatientIdentifier = "KMS.0004", FullName = "Viet Hoang Vo", DoB = new DateTime(1995,10,08),
                                                            Gender = 0, Email = "vietvo@kms-technology.com", PhoneNumber = "0905879111", AvatarLink = "",
                                                            PrimaryAddress = addresses[3]}
            };

            var expected = new PatientChecking.ServiceModels.PatientList
            {
                Patients = sortOption == 0 ? patientsServiceModelSortById : sortOption == 1 ? patientsServiceModelSortByName : patientsServiceModelSortByDob,
                TotalCount = patients.Count
            };

            return expected;
        }
    }
}
