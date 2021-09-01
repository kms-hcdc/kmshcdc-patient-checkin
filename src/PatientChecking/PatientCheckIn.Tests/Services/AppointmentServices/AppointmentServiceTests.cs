using Microsoft.EntityFrameworkCore;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.ServiceModels;
using PatientChecking.ServiceModels.Enum;
using PatientChecking.Services.Appointment;
using PatientChecking.Views.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Services.AppointmentServices
{
    public class AppointmentServiceTests
    {
        private PatientCheckInContext CreateMockContext(List<DataAccess.Models.Appointment> appointments)
        {
            var builder = new DbContextOptionsBuilder<PatientCheckInContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            DbContextOptions<PatientCheckInContext> options = builder.Options;

            var context = new PatientCheckInContext(options);
            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();

            if (appointments != null)
            {
                context.AddRange(appointments);
            }

            context.SaveChanges();

            return context;
        }
        private List<DataAccess.Models.Appointment> AppointmentDataTest(List<DataAccess.Models.Patient> patients)
        {
            var appointments = new List<DataAccess.Models.Appointment>
            {
                new DataAccess.Models.Appointment
                {
                    AppointmentId = 1, MedicalConcerns = "Head", CheckInDate = new DateTime(2021, 08, 26), Status = "CheckIn", PatientId = patients[0].PatientId, Patient = patients[0]
                },
                new DataAccess.Models.Appointment
                {
                    AppointmentId = 2, MedicalConcerns = "Hand", CheckInDate = new DateTime(2021, 08, 27), Status = "Cancel", PatientId = patients[1].PatientId, Patient = patients[1]
                },
                new DataAccess.Models.Appointment
                {
                    AppointmentId = 3, MedicalConcerns = "Stomach", CheckInDate = new DateTime(2021, 08, 25), Status = "Closed", PatientId = patients[2].PatientId, Patient = patients[2]
                },
                new DataAccess.Models.Appointment
                {
                    AppointmentId = 4, MedicalConcerns = "Lung", CheckInDate = new DateTime(2021, 08, 25), Status = "CheckIn", PatientId = patients[3].PatientId, Patient = patients[3]
                }
            };
            return appointments;
        }

        private List<DataAccess.Models.Patient> PatientDataTest()
        {
            var patients = new List<DataAccess.Models.Patient>
            {
                new DataAccess.Models.Patient {PatientId = 1, PatientIdentifier = "KMS.0001", FirstName = "Long", MiddleName = "Thanh", LastName = "Do", FullName = "Long Thanh Do",
                            DoB = new DateTime(1999,11,09), Gender = 0, PhoneNumber = "0905512324", Email = "longtdo@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Da Nang", BirthplaceCity = "Ho Chi Minh", IdcardNo = "201754622",
                            IssuedDate = new DateTime(2014, 09, 14), IssuedPlace = "Da Nang", InsuranceNo = "201329231", AvatarLink = "/Image/avatar.jpg"},

                new DataAccess.Models.Patient {PatientId = 2, PatientIdentifier = "KMS.0002", FirstName = "Duc", MiddleName = "Van", LastName = "Tran", FullName = "Duc Van Tran",
                            DoB = new DateTime(1999,05,10), Gender = 0, PhoneNumber = "0905879425", Email = "ducvant@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Da Nang", BirthplaceCity = "Da Nang", IdcardNo = "201251266",
                            IssuedDate = new DateTime(2013, 08, 16), IssuedPlace = "Da Nang", InsuranceNo = "203125325", AvatarLink = null},

                new DataAccess.Models.Patient {PatientId = 3, PatientIdentifier = "KMS.0003", FirstName = "Phien", MiddleName = "Minh", LastName = "Le", FullName = "Phien Minh Le",
                            DoB = new DateTime(1987,06,12), Gender = 0, PhoneNumber = "0905879425", Email = "phienle@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Can Tho", BirthplaceCity = "Can Tho", IdcardNo = "021252358",
                            IssuedDate = new DateTime(2011, 11, 16), IssuedPlace = "Ho Chi Minh", InsuranceNo = "203431326", AvatarLink = null},

                new DataAccess.Models.Patient {PatientId = 4, PatientIdentifier = "KMS.0004", FirstName = "Viet", MiddleName = "Hoang", LastName = "Vo", FullName = "Viet Hoang Vo",
                            DoB = new DateTime(1995,10,08), Gender = 0, PhoneNumber = "0905879425", Email = "vietvo@kms-technology.com", MaritalStatus = false,
                            Nationality = "Vietnamese", EthnicRace = "Kinh", HomeTown = "Ben Tre", BirthplaceCity = "Ben Tre", IdcardNo = "829123242",
                            IssuedDate = new DateTime(2010, 12, 16), IssuedPlace = "Ho Chi Minh", InsuranceNo = "203125445", AvatarLink = null},
            };

            return patients;
        }

        private PagingRequest PagingRequestData(int index, int pageSize, int optionSort)
        {
            var request = new PagingRequest
            {
                PageIndex = index,
                PageSize = pageSize,
                SortSelection = optionSort,
            };
            return request;
        }

        [Fact]
        public async void GetAppointmentSummary()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);
            var expected = new AppointmentDashboard
            {
                NumOfAppointments = 4,
                NumOfAppointmentsInMonth = context.Appointments.Where(x => x.CheckInDate.Date.Year == DateTime.Now.Year && x.CheckInDate.Date.Month == DateTime.Now.Month).Count(),
                NumOfAppointmentsInToday = context.Appointments.Where(x => x.CheckInDate.Date == DateTime.Today && x.Status == AppointmentStatus.CheckIn.ToString()).Count(),
                NumOfPatientsInMonth = 4,
            };

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.GetAppointmentSummary();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.NumOfPatientsInMonth, actual.NumOfPatientsInMonth);
            Assert.Equal(expected.NumOfAppointmentsInToday, actual.NumOfAppointmentsInToday);
            Assert.Equal(expected.NumOfAppointmentsInMonth, actual.NumOfAppointmentsInMonth);
            Assert.Equal(expected.NumOfAppointments, actual.NumOfAppointments);
        }

        [Fact]
        public async void GetAppointmentById()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);

            var expected = new PatientChecking.ServiceModels.Appointment
            {
                AppointmentId = appointments[2].AppointmentId,
                CheckInDate = appointments[2].CheckInDate,
                MedicalConcerns = appointments[2].MedicalConcerns,
                Status = appointments[2].Status,
                PatientId = appointments[2].PatientId,
            };

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.GetAppointmentById(3);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.AppointmentId, actual.AppointmentId);
            Assert.Equal(expected.CheckInDate, actual.CheckInDate);
            Assert.Equal(expected.MedicalConcerns, actual.MedicalConcerns);
            Assert.Equal(expected.Status, actual.Status);
            Assert.Equal(expected.PatientId, actual.PatientId);
        }

        [Fact]
        public async void GetListAppoinmentsPaging_SortByName()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);
            var pagingRequest = PagingRequestData(1,2,0);

            var appoinmentListServiceModel = new List<PatientChecking.ServiceModels.Appointment>
            {
                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 2,
                    MedicalConcerns = "Hand",
                    CheckInDate = new DateTime(2021, 08, 27),
                    Status = "Cancel",
                    PatientId = patients[1].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[1].AvatarLink, DoB = patients[1].DoB, FullName = patients[1].FullName, PatientIdentifier = patients[1].PatientIdentifier }
                },

                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 1, 
                    MedicalConcerns = "Head", 
                    CheckInDate = new DateTime(2021, 08, 26), 
                    Status = "CheckIn", 
                    PatientId = patients[0].PatientId, 
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[0].AvatarLink, DoB = patients[0].DoB, FullName = patients[0].FullName, PatientIdentifier = patients[0].PatientIdentifier }
                }
            };

            var expected = new AppointmentList
            {
                Appointments = appoinmentListServiceModel,
                TotalCount = 4,
            };

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.GetListAppoinmentsPaging(pagingRequest);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Appointments.Count, actual.Appointments.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            for(int i = 0; i < expected.Appointments.Count; i++)
            {
                Assert.Equal(expected.Appointments[i].AppointmentId, actual.Appointments[i].AppointmentId);
                Assert.Equal(expected.Appointments[i].CheckInDate, actual.Appointments[i].CheckInDate);
                Assert.Equal(expected.Appointments[i].Status, actual.Appointments[i].Status);
                Assert.Equal(expected.Appointments[i].Patient.AvatarLink, actual.Appointments[i].Patient.AvatarLink);
                Assert.Equal(expected.Appointments[i].Patient.DoB, actual.Appointments[i].Patient.DoB);
                Assert.Equal(expected.Appointments[i].Patient.FullName, actual.Appointments[i].Patient.FullName);
                Assert.Equal(expected.Appointments[i].Patient.PatientIdentifier, actual.Appointments[i].Patient.PatientIdentifier);
            }
        }

        [Fact]
        public async void GetListAppoinmentsPaging_SortByID()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);
            var pagingRequest = PagingRequestData(1, 2, 1);
            var appoinmentListServiceModel = new List<PatientChecking.ServiceModels.Appointment>
            {
                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 1,
                    MedicalConcerns = "Head",
                    CheckInDate = new DateTime(2021, 08, 26),
                    Status = "CheckIn",
                    PatientId = patients[0].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[0].AvatarLink, DoB = patients[0].DoB, FullName = patients[0].FullName, PatientIdentifier = patients[0].PatientIdentifier }
                },

                 new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 2,
                    MedicalConcerns = "Hand",
                    CheckInDate = new DateTime(2021, 08, 27),
                    Status = "Cancel",
                    PatientId = patients[1].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[1].AvatarLink, DoB = patients[1].DoB, FullName = patients[1].FullName, PatientIdentifier = patients[1].PatientIdentifier }
                },
            };

            var expected = new AppointmentList
            {
                Appointments = appoinmentListServiceModel,
                TotalCount = 4,
            };

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.GetListAppoinmentsPaging(pagingRequest);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Appointments.Count, actual.Appointments.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            for (int i = 0; i < expected.Appointments.Count; i++)
            {
                Assert.Equal(expected.Appointments[i].AppointmentId, actual.Appointments[i].AppointmentId);
                Assert.Equal(expected.Appointments[i].CheckInDate, actual.Appointments[i].CheckInDate);
                Assert.Equal(expected.Appointments[i].Status, actual.Appointments[i].Status);
                Assert.Equal(expected.Appointments[i].Patient.AvatarLink, actual.Appointments[i].Patient.AvatarLink);
                Assert.Equal(expected.Appointments[i].Patient.DoB, actual.Appointments[i].Patient.DoB);
                Assert.Equal(expected.Appointments[i].Patient.FullName, actual.Appointments[i].Patient.FullName);
                Assert.Equal(expected.Appointments[i].Patient.PatientIdentifier, actual.Appointments[i].Patient.PatientIdentifier);
            }
        }

        [Fact]
        public async void GetListAppoinmentsPaging_SortByDoB()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);
            var pagingRequest = PagingRequestData(1, 2, 2);
            var appoinmentListServiceModel = new List<PatientChecking.ServiceModels.Appointment>
            {
                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 3,
                    MedicalConcerns = "Stomach",
                    CheckInDate = new DateTime(2021, 08, 25),
                    Status = "Closed",
                    PatientId = patients[2].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[2].AvatarLink, DoB = patients[2].DoB, FullName = patients[2].FullName, PatientIdentifier = patients[2].PatientIdentifier }
                },

                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 4,
                    MedicalConcerns = "Lung",
                    CheckInDate = new DateTime(2021, 08, 25),
                    Status = "CheckIn",
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[3].AvatarLink, DoB = patients[3].DoB, FullName = patients[3].FullName, PatientIdentifier = patients[3].PatientIdentifier }
                }
            };

            var expected = new AppointmentList
            {
                Appointments = appoinmentListServiceModel,
                TotalCount = 4,
            };

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.GetListAppoinmentsPaging(pagingRequest);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Appointments.Count, actual.Appointments.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            for (int i = 0; i < expected.Appointments.Count; i++)
            {
                Assert.Equal(expected.Appointments[i].AppointmentId, actual.Appointments[i].AppointmentId);
                Assert.Equal(expected.Appointments[i].CheckInDate, actual.Appointments[i].CheckInDate);
                Assert.Equal(expected.Appointments[i].Status, actual.Appointments[i].Status);
                Assert.Equal(expected.Appointments[i].Patient.AvatarLink, actual.Appointments[i].Patient.AvatarLink);
                Assert.Equal(expected.Appointments[i].Patient.DoB, actual.Appointments[i].Patient.DoB);
                Assert.Equal(expected.Appointments[i].Patient.FullName, actual.Appointments[i].Patient.FullName);
                Assert.Equal(expected.Appointments[i].Patient.PatientIdentifier, actual.Appointments[i].Patient.PatientIdentifier);
            }
        }

        [Fact]
        public async void GetListAppoinmentsPaging_SortByCheckInDate()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);
            var pagingRequest = PagingRequestData(1, 2, 3);
            var appoinmentListServiceModel = new List<PatientChecking.ServiceModels.Appointment>
            {
                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 2,
                    MedicalConcerns = "Hand",
                    CheckInDate = new DateTime(2021, 08, 27),
                    Status = "Cancel",
                    PatientId = patients[1].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[1].AvatarLink, DoB = patients[1].DoB, FullName = patients[1].FullName, PatientIdentifier = patients[1].PatientIdentifier }
                },

                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 1,
                    MedicalConcerns = "Head",
                    CheckInDate = new DateTime(2021, 08, 26),
                    Status = "CheckIn",
                    PatientId = patients[0].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[0].AvatarLink, DoB = patients[0].DoB, FullName = patients[0].FullName, PatientIdentifier = patients[0].PatientIdentifier }
                }
            };

            var expected = new AppointmentList
            {
                Appointments = appoinmentListServiceModel,
                TotalCount = 4,
            };

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.GetListAppoinmentsPaging(pagingRequest);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Appointments.Count, actual.Appointments.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            for (int i = 0; i < expected.Appointments.Count; i++)
            {
                Assert.Equal(expected.Appointments[i].AppointmentId, actual.Appointments[i].AppointmentId);
                Assert.Equal(expected.Appointments[i].CheckInDate, actual.Appointments[i].CheckInDate);
                Assert.Equal(expected.Appointments[i].Status, actual.Appointments[i].Status);
                Assert.Equal(expected.Appointments[i].Patient.AvatarLink, actual.Appointments[i].Patient.AvatarLink);
                Assert.Equal(expected.Appointments[i].Patient.DoB, actual.Appointments[i].Patient.DoB);
                Assert.Equal(expected.Appointments[i].Patient.FullName, actual.Appointments[i].Patient.FullName);
                Assert.Equal(expected.Appointments[i].Patient.PatientIdentifier, actual.Appointments[i].Patient.PatientIdentifier);
            }
        }

        [Fact]
        public async void GetListAppoinmentsPaging_SortByStatus()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);
            var pagingRequest = PagingRequestData(1, 2, 4);
            var appoinmentListServiceModel = new List<PatientChecking.ServiceModels.Appointment>
            {
                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 1,
                    MedicalConcerns = "Head",
                    CheckInDate = new DateTime(2021, 08, 26),
                    Status = "CheckIn",
                    PatientId = patients[0].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[0].AvatarLink, DoB = patients[0].DoB, FullName = patients[0].FullName, PatientIdentifier = patients[0].PatientIdentifier }
                },

                new PatientChecking.ServiceModels.Appointment
                {
                    AppointmentId = 4,
                    MedicalConcerns = "Lung",
                    CheckInDate = new DateTime(2021, 08, 25),
                    Status = "CheckIn",
                    PatientId = patients[3].PatientId,
                    Patient =  new PatientChecking.ServiceModels.Patient{ AvatarLink = patients[3].AvatarLink, DoB = patients[3].DoB, FullName = patients[3].FullName, PatientIdentifier = patients[3].PatientIdentifier }
                }
            };

            var expected = new AppointmentList
            {
                Appointments = appoinmentListServiceModel,
                TotalCount = 4,
            };

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.GetListAppoinmentsPaging(pagingRequest);

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Appointments.Count, actual.Appointments.Count);
            Assert.Equal(expected.TotalCount, actual.TotalCount);
            for (int i = 0; i < expected.Appointments.Count; i++)
            {
                Assert.Equal(expected.Appointments[i].AppointmentId, actual.Appointments[i].AppointmentId);
                Assert.Equal(expected.Appointments[i].CheckInDate, actual.Appointments[i].CheckInDate);
                Assert.Equal(expected.Appointments[i].Status, actual.Appointments[i].Status);
                Assert.Equal(expected.Appointments[i].Patient.AvatarLink, actual.Appointments[i].Patient.AvatarLink);
                Assert.Equal(expected.Appointments[i].Patient.DoB, actual.Appointments[i].Patient.DoB);
                Assert.Equal(expected.Appointments[i].Patient.FullName, actual.Appointments[i].Patient.FullName);
                Assert.Equal(expected.Appointments[i].Patient.PatientIdentifier, actual.Appointments[i].Patient.PatientIdentifier);
            }
        }

        [Fact]
        public async void UpdateAppointment_Ok()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);
            var modifiedAppointment = appointments[0];
            modifiedAppointment.MedicalConcerns = "Stomach";

            var expected = 1;

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.UpdateAppointment(modifiedAppointment);

            //Assert
            Assert.True(actual != -1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void UpdateAppointment_ParamNull()
        {
            //Arrange
            var patients = PatientDataTest();
            var appointments = AppointmentDataTest(patients);
            var context = CreateMockContext(appointments);

            var expected = -1;

            //Act
            var apointmentService = new AppointmentService(context);
            var actual = await apointmentService.UpdateAppointment(null);

            //Assert
            Assert.True(actual == -1);
            Assert.Equal(expected, actual);
        }
    }
}
