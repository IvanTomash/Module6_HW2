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
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests.Services
{
    public class CatalogItemServiceTest
    {
        private readonly ICatalogItemService _catalogItemService;

        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        private readonly CatalogItem _testItem = new CatalogItem()
        {
            Name = "TestName",
            Description = "TestDescription",
            Price = 1000,
            AvailableStock = 100,
            PictureFileName = "test.png",
            CatalogBrandId = 1,
            CatalogTypeId = 1,
        };

        public CatalogItemServiceTest()
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogItemService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object);
        }

        [Fact]
        public async Task Add_Success()
        {
            int testResult = 1;

            _catalogItemRepository.Setup(s => s.Add(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogItemService.Add(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Add_Failed()
        {
            int? testResult = null;

            _catalogItemRepository.Setup(s => s.Add(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogItemService.Add(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Delete_Success()
        {
            int testResult = 1;

            _catalogItemRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            var result = await _catalogItemService.Delete(_testItem.Id);
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Delete_Failed()
        {
            int? testResult = null;

            _catalogItemRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

            var result = await _catalogItemService.Delete(_testItem.Id);
            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Success()
        {
            int testResult = 1;

            _catalogItemRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogItemService.Update(_testItem.Id, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

            result.Should().Be(testResult);
        }

        [Fact]
        public async Task Update_Failed()
        {
            int? testResult = null;

            _catalogItemRepository.Setup(s => s.Update(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(testResult);

            var result = await _catalogItemService.Update(_testItem.Id, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

            result.Should().Be(testResult);
        }
    }
}
