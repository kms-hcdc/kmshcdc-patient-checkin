using Microsoft.EntityFrameworkCore;
using PatientCheckIn.DataAccess.Models;
using PatientChecking.Services.AppConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PatientCheckIn.Tests.Services.AppConfigurationServices
{
    public class AppConfigurationServiceTests
    {
        private PatientCheckInContext CreateMockContext(List<DataAccess.Models.ProvinceCity> provinceCities)
        {
            var builder = new DbContextOptionsBuilder<PatientCheckInContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            DbContextOptions<PatientCheckInContext> options = builder.Options;

            var context = new PatientCheckInContext(options);
            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();

            if (provinceCities != null)
            {
                context.AddRange(provinceCities);
            }

            context.SaveChanges();

            return context;
        }

        private List<DataAccess.Models.ProvinceCity> ProvinceCityDataTest()
        {
            var provinceCities = new List<DataAccess.Models.ProvinceCity>
            {
                new DataAccess.Models.ProvinceCity
                {
                   ProvinceCityId = 1, ProvinceCityName = "An Giang"
                },
                new DataAccess.Models.ProvinceCity
                {
                   ProvinceCityId = 2, ProvinceCityName = "Bà rịa – Vung tàu"
                },
                new DataAccess.Models.ProvinceCity
                {
                   ProvinceCityId = 3, ProvinceCityName = "Bắc Giang"
                },
                new DataAccess.Models.ProvinceCity
                {
                   ProvinceCityId = 4, ProvinceCityName = "Bắc Kạn"
                },
                new DataAccess.Models.ProvinceCity
                {
                   ProvinceCityId = 5, ProvinceCityName = "Bạc Liêu"
                },
            };
            return provinceCities;
        }

        [Fact]
        public async void GetProvinceCities()
        {
            //Arrange
            var provinceCities = ProvinceCityDataTest();
            var context = CreateMockContext(provinceCities);

            var expected = provinceCities;

            //Act
            var appConfigurationService = new AppConfigurationService(context);
            var actual = await appConfigurationService.GetProvinceCitiesAsync();

            //Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].ProvinceCityId, actual[i].ProvinceCityId);
                Assert.Equal(expected[i].ProvinceCityName, actual[i].ProvinceCityName);
            }
        }
    }
}
