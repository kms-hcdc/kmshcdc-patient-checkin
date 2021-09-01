using Moq;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.Feature.Patient.Queries;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.AppConfiguration;
using PatientChecking.Services.Patient;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Feature.Patient
{
    public class PatientQueryTests
    {
        [Fact]
        public async void GetAllPatientsPagingQueryTest()
        {
            //Arrange
            var result = await DataTest();
            var request = new PagingRequest
            {
                PageIndex = 1,
                PageSize = 4,
                SortSelection = 0
            };
            var mockPatientService = new Mock<IPatientService>();
            mockPatientService.Setup(x => x.GetListPatientPaging(request)).ReturnsAsync(result);

            var query = new GetAllPatientsPagingQuery() { requestPaging = request };
            var handler = new GetAllPatientsPagingQueryHandler(mockPatientService.Object);


            //For expected data
            var patientsVm = new List<PatientViewModel>();

            foreach (PatientChecking.ServiceModels.Patient p in result.Patients)
            {
                patientsVm.Add(new PatientViewModel
                {
                    PatientId = p.PatientId,
                    PatientIdentifier = p.PatientIdentifier,
                    FullName = p.FullName,
                    Gender = p.Gender.ToString(),
                    DoB = p.DoB.ToString("MM-dd-yyyy"),
                    AvatarLink = p.AvatarLink,
                    Address = p.PrimaryAddress?.StreetLine,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber
                });
            }

            var expected = new PatientListViewModel
            {
                Patients = patientsVm,
                SortSelection = request.SortSelection,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = result.TotalCount
            };

            //Act
            var actual = await handler.Handle(query, new System.Threading.CancellationToken());

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Patients.Count, actual.Patients.Count);
            Assert.Equal(expected.PageIndex, actual.PageIndex);
            Assert.Equal(expected.PageSize, actual.PageSize);
            Assert.Equal(expected.SortSelection, actual.SortSelection);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            for(int i = 0; i < expected.Patients.Count; i++)
            {
                Assert.Equal(expected.Patients[i].PatientId, actual.Patients[i].PatientId);
                Assert.Equal(expected.Patients[i].PatientIdentifier, actual.Patients[i].PatientIdentifier);
                Assert.Equal(expected.Patients[i].FullName, actual.Patients[i].FullName);
                Assert.Equal(expected.Patients[i].Gender, actual.Patients[i].Gender);
                Assert.Equal(expected.Patients[i].DoB, actual.Patients[i].DoB);
                Assert.Equal(expected.Patients[i].AvatarLink, actual.Patients[i].AvatarLink);
                Assert.Equal(expected.Patients[i].Address, actual.Patients[i].Address);
                Assert.Equal(expected.Patients[i].Email, actual.Patients[i].Email);
                Assert.Equal(expected.Patients[i].PhoneNumber, actual.Patients[i].PhoneNumber);
            }
        }

        [Fact]
        public async void GetPatientInDetailByIdQueryTest_NotEmptyModel()
        {
            //Arrange

            var cities = ProvinceCityDataTest();
            var cityList = new List<string>();
            
            foreach(ProvinceCity city in cities)
            {
                cityList.Add(city.ProvinceCityName);
            }

            var patient = new DataAccess.Models.Patient
            {
                PatientId = 1,
                PatientIdentifier = "KMS.0001",
                FirstName = "Long",
                MiddleName = "Thanh",
                LastName = "Do",
                FullName = "Long Thanh Do",
                DoB = new DateTime(1999, 11, 09),
                Gender = 0,
                PhoneNumber = "0905512324",
                Email = "longtdo@kms-technology.com",
                MaritalStatus = false,
                Nationality = "Vietnamese",
                EthnicRace = "Kinh",
                HomeTown = "Da Nang",
                BirthplaceCity = "Ho Chi Minh",
                IdcardNo = "201754622",
                IssuedDate = new DateTime(2014, 09, 14),
                IssuedPlace = "Da Nang",
                InsuranceNo = "201329231",
                AvatarLink = "/Image/avatar.jpg"
            };

            var mockPatientService = new Mock<IPatientService>();
            mockPatientService.Setup(x => x.GetPatientInDetail(1)).ReturnsAsync(patient);

            var mockAppConfigurationService = new Mock<IAppConfigurationService>();
            mockAppConfigurationService.Setup(x => x.GetProvinceCitiesAsync()).ReturnsAsync(cities);

            var query = new GetPatientInDetailByIdQuery() {PatientId = 1};
            var handler = new GetPatientInDetailByIdQueryHandler(mockPatientService.Object, mockAppConfigurationService.Object);

            var expected = new PatientDetailViewModel
            {
                PatientId = patient.PatientId,
                PatientIdentifier = patient.PatientIdentifier,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                MiddleName = patient.MiddleName,
                FullName = patient.FullName,
                Nationality = patient.Nationality,
                DoB = patient.DoB.ToString("yyyy-MM-dd"),
                MaritalStatus = (int)(patient.MaritalStatus == true ? PatientMaritalStatus.Married : PatientMaritalStatus.Unmarried),
                Gender = (int)patient.Gender,
                AvatarLink = patient.AvatarLink,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                EthnicRace = patient.EthnicRace,
                HomeTown = patient.HomeTown,
                BirthplaceCity = patient.BirthplaceCity,
                IdcardNo = patient.IdcardNo,
                IssuedDate = patient.IssuedDate?.ToString("yyyy-MM-dd"),
                IssuedPlace = patient.IssuedPlace,
                InsuranceNo = patient.InsuranceNo,
                ProvinceCities = cityList
            };

            //Act
            var actual = await handler.Handle(query, new System.Threading.CancellationToken());

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.PatientId, actual.PatientId);
            Assert.Equal(expected.PatientIdentifier, actual.PatientIdentifier);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.MiddleName, actual.MiddleName);
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.FullName, actual.FullName);
            Assert.Equal(expected.Nationality, actual.Nationality);
            Assert.Equal(expected.DoB, actual.DoB);
            Assert.Equal(expected.MaritalStatus, actual.MaritalStatus);
            Assert.Equal(expected.Gender, actual.Gender);
            Assert.Equal(expected.AvatarLink, actual.AvatarLink);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
            Assert.Equal(expected.EthnicRace, actual.EthnicRace);
            Assert.Equal(expected.HomeTown, actual.HomeTown);
            Assert.Equal(expected.BirthplaceCity, actual.BirthplaceCity);
            Assert.Equal(expected.IdcardNo, actual.IdcardNo);
            Assert.Equal(expected.IssuedDate, actual.IssuedDate);
            Assert.Equal(expected.IssuedPlace, actual.IssuedPlace);
            Assert.Equal(expected.InsuranceNo, actual.InsuranceNo);
            for(int i = 0; i < expected.ProvinceCities.Count; i++)
            {
                Assert.Equal(expected.ProvinceCities[i], actual.ProvinceCities[i]);
            }
        }

        [Fact]
        public async void GetPatientInDetailByIdQueryTest_EmptyModel()
        {
            //Arrange
            var cities = ProvinceCityDataTest();
            var cityList = new List<string>();

            foreach (ProvinceCity city in cities)
            {
                cityList.Add(city.ProvinceCityName);
            }

            var mockPatientService = new Mock<IPatientService>();

            var mockAppConfigurationService = new Mock<IAppConfigurationService>();
            mockAppConfigurationService.Setup(x => x.GetProvinceCitiesAsync()).ReturnsAsync(cities);

            var query = new GetPatientInDetailByIdQuery() { PatientId = -1 };
            var handler = new GetPatientInDetailByIdQueryHandler(mockPatientService.Object, mockAppConfigurationService.Object);

            var expected = new PatientDetailViewModel
            {
                PatientId = -1,
                PatientIdentifier = "",
                Nationality = "Vietnamese",
                ProvinceCities = cityList
            };

            //Act
            var actual = await handler.Handle(query, new System.Threading.CancellationToken());

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.PatientId, actual.PatientId);
            Assert.Equal(expected.PatientIdentifier, actual.PatientIdentifier);
            Assert.Equal(expected.Nationality, actual.Nationality);
            for (int i = 0; i < expected.ProvinceCities.Count; i++)
            {
                Assert.Equal(expected.ProvinceCities[i], actual.ProvinceCities[i]);
            }
        }


        private List<ProvinceCity> ProvinceCityDataTest()
        {
            var provinceCities = new List<ProvinceCity>
            {
                new ProvinceCity
                {
                   ProvinceCityId = 1, ProvinceCityName = "An Giang"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 2, ProvinceCityName = "Bà rịa – Vung tàu"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 3, ProvinceCityName = "Bắc Giang"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 4, ProvinceCityName = "Bắc Kạn"
                },
                new ProvinceCity
                {
                   ProvinceCityId = 5, ProvinceCityName = "Bạc Liêu"
                },
            };
            return provinceCities;
        }

        public async Task<PatientList> DataTest()
        {
            var addresses = new List<Address>
            {
                new Address {AddressId = 1, TypeAddress = 0, StreetLine = "To 1, Hoa Son, Hoa Vang, Da Nang", IsPrimary = true, PatientId = 1},
                new Address {AddressId = 2, TypeAddress = 0, StreetLine = "To 2, Hoa Lien, Hoa Vang, Da Nang", IsPrimary = true, PatientId = 2},
                new Address {AddressId = 3, TypeAddress = 0, StreetLine = "34 Bui Vien, Quan Cam, Thanh Pho Ho Chi Minh", IsPrimary = true, PatientId = 3},
                new Address {AddressId = 4, TypeAddress = 0, StreetLine = "124 Le Loi, Quan 1, Thanh Pho Ho Chi Minh", IsPrimary = true, PatientId = 4},
            };

            var patientsServiceModel = new List<PatientChecking.ServiceModels.Patient>
            {
                new PatientChecking.ServiceModels.Patient { PatientId = 1, PatientIdentifier = "KMS.0001", FullName = "Long Thanh Do", DoB = new DateTime(1999,11,09),
                                                            Gender = 0, Email = "longtdo@kms-technology.com", PhoneNumber = "0905512324", AvatarLink = "/Image/avatar.jpg",
                                                            PrimaryAddress = addresses[0]},
                new PatientChecking.ServiceModels.Patient { PatientId = 2, PatientIdentifier = "KMS.0002", FullName = "Duc Van Tran", DoB = new DateTime(1999,05,10),
                                                            Gender = 0, Email = "ducvant@kms-technology.com", PhoneNumber = "0905879123", AvatarLink = "",
                                                            PrimaryAddress = addresses[1]},
                new PatientChecking.ServiceModels.Patient { PatientId = 3, PatientIdentifier = "KMS.0003", FullName = "Phien Minh Le", DoB = new DateTime(1987,06,12),
                                                            Gender = 0, Email = "phienle@kms-technology.com", PhoneNumber = "0905879425", AvatarLink = "/Image/PhienProfile.jpg",
                                                            PrimaryAddress = addresses[2]},
                new PatientChecking.ServiceModels.Patient { PatientId = 4, PatientIdentifier = "KMS.0004", FullName = "Viet Hoang Vo", DoB = new DateTime(1995,10,08),
                                                            Gender = 0, Email = "vietvo@kms-technology.com", PhoneNumber = "0905879111", AvatarLink = "",
                                                            PrimaryAddress = addresses[3]}
            };

            var data = new PatientList
            {
                Patients = patientsServiceModel,
                TotalCount = patientsServiceModel.Count
            };

            return data;
        }
    }
}
