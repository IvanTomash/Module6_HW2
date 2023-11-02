using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests.Services
{
    public class CatalogServiceTest
    {
        private readonly ICatalogService _catalogService;

        private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
        private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<ILogger<CatalogService>> _logger;

        public CatalogServiceTest()
        {
            _catalogItemRepository = new Mock<ICatalogItemRepository>();
            _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
            _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
            _mapper = new Mock<IMapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _logger = new Mock<ILogger<CatalogService>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

            _catalogService = new CatalogService(
                _dbContextWrapper.Object,
                _logger.Object,
                _catalogItemRepository.Object,
                _catalogBrandRepository.Object,
                _catalogTypeRepository.Object,
                _mapper.Object);
        }

        [Fact]
        public async Task GetCatalogItemsAsync_Success()
        {
            var testPageIndex = 0;
            var testPageSize = 4;
            var testTotalCount = 4;

            var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
            {
                Data = new List<CatalogItem>()
                {
                    new CatalogItem()
                    {
                        Name = "TestName",
                    },
                },
                TotalCount = testTotalCount,
            };

            var catalogItemSuccess = new CatalogItem()
            {
                Name = "TestName",
            };

            var catalogItemDtoSuccess = new CatalogItemDto()
            {
                Name = "TestName",
            };

            _catalogItemRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

            var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.Count.Should().Be(testTotalCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
        }

        [Fact]
        public async Task GetCatalogItemAsync_Failed()
        {
            var testPageIndex = 1000;
            var testPageSize = 10000;

            _catalogItemRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

            var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCatalogBrandsAsync_Success()
        {
            var testPageIndex = 0;
            var testPageSize = 4;
            var testTotalCount = 5;

            var pagingPaginatedBrandsSuccess = new PaginatedItems<CatalogBrand>()
            {
                Data = new List<CatalogBrand>()
                {
                    new CatalogBrand()
                    {
                        Brand = "TestBrand",
                    },
                },
                TotalCount = testTotalCount,
            };

            var catalogBrandSuccess = new CatalogBrand()
            {
                Brand = "TestBrand",
            };

            var catalogBrandDtoSuccess = new CatalogBrandDto()
            {
                Brand = "TestBrand",
            };

            _catalogBrandRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedBrandsSuccess);

            var result = await _catalogService.GetCatalogBrandsAsync(testPageSize, testPageIndex);

            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.Count.Should().Be(testTotalCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
        }

        [Fact]
        public async Task GetCatalogBrandAsync_Failed()
        {
            var testPageIndex = 1000;
            var testPageSize = 10000;

            _catalogBrandRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogBrandDto>>)null!);

            var result = await _catalogService.GetCatalogBrandsAsync(testPageSize, testPageIndex);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetCatalogTypesAsync_Success()
        {
            var testPageIndex = 0;
            var testPageSize = 4;
            var testTotalCount = 4;

            var pagingPaginatedTypesSuccess = new PaginatedItems<CatalogType>()
            {
                Data = new List<CatalogType>()
                {
                    new CatalogType()
                    {
                        Type = "TestType",
                    },
                },
                TotalCount = testTotalCount,
            };

            var catalogTypeSuccess = new CatalogType()
            {
                Type = "TestType",
            };

            var catalogTypeDtoSuccess = new CatalogTypeDto()
            {
                Type = "TestBrand",
            };

            _catalogTypeRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedTypesSuccess);

            var result = await _catalogService.GetCatalogTypesAsync(testPageSize, testPageIndex);

            result.Should().NotBeNull();
            result?.Data.Should().NotBeNull();
            result?.Count.Should().Be(testTotalCount);
            result?.PageIndex.Should().Be(testPageIndex);
            result?.PageSize.Should().Be(testPageSize);
        }

        [Fact]
        public async Task GetCatalogTypeAsync_Failed()
        {
            var testPageIndex = 1000;
            var testPageSize = 10000;

            _catalogTypeRepository.Setup(s => s.GetByPageAsync(
                It.Is<int>(i => i == testPageIndex),
                It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogTypeDto>>)null!);

            var result = await _catalogService.GetCatalogTypesAsync(testPageSize, testPageIndex);
            result.Should().BeNull();
        }
    }
}
