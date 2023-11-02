using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests.Services
{
    public class CatalogTypeServiceTest
    {
        private readonly ICatalogTypeService _catalogTypeService;

        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogType _testType = new CatalogType()
        {
            Type = "TestType"
        };

        public CatalogTypeServiceTest()
        {
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object);
        }

        [Fact]
        public async Task Add_Success()
        {
            int testResult = 1;

            _catalogTypeRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogTypeService.Add(_testType.Type);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Add_Failed()
        {
            int? testResult = null;

            _catalogTypeRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogTypeService.Add(_testType.Type);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Delete_Success()
        {
            int testResult = 1;

            _catalogTypeRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            var result = await _catalogTypeService.Delete(_testType.Id);
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Delete_Failed()
        {
            int? testResult = null;

            _catalogTypeRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            var result = await _catalogTypeService.Delete(_testType.Id);
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Success()
        {
            int testResult = 1;

            _catalogTypeRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogTypeService.Update(_testType.Id, _testType.Type);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Failed()
        {
            int? testResult = null;

            _catalogTypeRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogTypeService.Update(_testType.Id, _testType.Type);

            result.Should().Be(testResult);
        }
    }
}
