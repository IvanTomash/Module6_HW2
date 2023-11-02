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
    public class CatalogBrandServiceTest
    {
        private readonly ICatalogBrandService _catalogBrandService;

        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogBrand _testBrand = new CatalogBrand()
        {
            Brand = "TestBrand"
        };

        public CatalogBrandServiceTest()
        {
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object);
        }

        [Fact]
        public async Task Add_Success()
        {
            int testResult = 1;

            _catalogBrandRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogBrandService.Add(_testBrand.Brand);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Add_Failed()
        {
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Add(
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogBrandService.Add(_testBrand.Brand);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Delete_Success()
        {
            int testResult = 1;

            _catalogBrandRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            var result = await _catalogBrandService.Delete(_testBrand.Id);
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Delete_Failed()
        {
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            var result = await _catalogBrandService.Delete(_testBrand.Id);
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Success()
        {
            int testResult = 1;

            _catalogBrandRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogBrandService.Update(_testBrand.Id, _testBrand.Brand);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Failed()
        {
            int? testResult = null;

            _catalogBrandRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogBrandService.Update(_testBrand.Id, _testBrand.Brand);

            result.Should().Be(testResult);
        }
    }
}
