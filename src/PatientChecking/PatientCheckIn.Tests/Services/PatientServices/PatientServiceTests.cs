using Microsoft.EntityFrameworkCore;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.Services.Patient;
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
        private PatientCheckInContext CreateMockContext(List<Patient> patients)
        {
            var builder = new DbContextOptionsBuilder<PatientCheckInContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            DbContextOptions<PatientCheckInContext> options = builder.Options;

            var context = new PatientCheckInContext(options);
            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();

            if(patients != null)
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
                new Patient {PatientId = 1, PatientIdentifier = "KMS.0001", FirstName = "Long", MiddleName = "Thanh", LastName = "Do", FullName = "Long Thanh Do", 
                            DoB = new DateTime(1999,11,09), Gender = 0, PhoneNumber = "0905512324", Email = "longtdo@kms-technology.com", MaritalStatus = false, 
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Da Nang", BirthplaceCity = "Ho Chi Minh", IdcardNo = "201754622", 
                            IssuedDate = new DateTime(2014, 09, 14), IssuedPlace = "Da Nang", InsuranceNo = "201329231", AvatarLink = "/Image/avatar.jpg",
                            Addresses = new List<Address> { addresses[0] } },

                new Patient {PatientId = 2, PatientIdentifier = "KMS.0002", FirstName = "Duc", MiddleName = "Van", LastName = "Tran", FullName = "Duc Van Tran", 
                            DoB = new DateTime(1999,05,10), Gender = 0, PhoneNumber = "0905879425", Email = "ducvant@kms-technology.com", MaritalStatus = false, 
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Da Nang", BirthplaceCity = "Da Nang", IdcardNo = "201251266", 
                            IssuedDate = new DateTime(2013, 08, 16), IssuedPlace = "Da Nang", InsuranceNo = "203125325", AvatarLink = null,
                            Addresses = new List<Address> { addresses[1] } },

                new Patient {PatientId = 3, PatientIdentifier = "KMS.0003", FirstName = "Phien", MiddleName = "Minh", LastName = "Le", FullName = "Phien Minh Le", 
                            DoB = new DateTime(1987,06,12), Gender = 0, PhoneNumber = "0905879425", Email = "phienle@kms-technology.com", MaritalStatus = false, 
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Can Tho", BirthplaceCity = "Can Tho", IdcardNo = "021252358", 
                            IssuedDate = new DateTime(2011, 11, 16), IssuedPlace = "Ho Chi Minh", InsuranceNo = "203431326", AvatarLink = null,
                            Addresses = new List<Address> { addresses[2] } },

                new Patient {PatientId = 4, PatientIdentifier = "KMS.0004", FirstName = "Viet", MiddleName = "Hoang", LastName = "Vo", FullName = "Viet Hoang Vo", 
                            DoB = new DateTime(1995,10,08), Gender = 0, PhoneNumber = "0905879425", Email = "vietvo@kms-technology.com", MaritalStatus = false, 
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Ben Tre", BirthplaceCity = "Ben Tre", IdcardNo = "829123242", 
                            IssuedDate = new DateTime(2010, 12, 16), IssuedPlace = "Ho Chi Minh", InsuranceNo = "203125445", AvatarLink = null,
                            Addresses = new List<Address> { addresses[3] } },
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

        [Fact]
        public void GetListPatientPaging_SortByID()
        {
            //Arrange
            var patients = new List<Patient>
            {
                new Patient {PatientId = 1, }
            };
        }

        [Fact]
        public async void UpdatePatientDetail_Ok()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);
            
            var expected = 1;

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.UpdatePatientDetail(patients[0]);

            //Assert
            Assert.True(actual != -1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UpdatePatientDetail_ParameterNull()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            var expected = -1;

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.UpdatePatientDetail(null);

            //Assert
            Assert.True(actual == -1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UpdatePatientDetail_PatientQueriedNull()
        {
            //Arrange
            var addresses = AddressDataTest();
            var patients = PatientDataTest(addresses);
            var context = CreateMockContext(patients);

            patients[0].PatientId = -1;

            var expected = -1;

            //Act
            var patientService = new PatientService(context);
            var actual = await patientService.UpdatePatientDetail(patients[0]);

            //Assert
            Assert.True(actual == -1);
            Assert.Equal(expected, actual);
        }
    }
}
