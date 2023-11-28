using System;
using Microsoft.Extensions.Logging;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Greggs.Products.UnitTests
{
    public class ProductControllerUnitTest
    {

        private readonly Mock<IDataAccess<Product>> _productAccessMock;
        private readonly Mock<ILogger<ProductController>> _loggerMock;
        private readonly ProductController _controller;

        public ProductControllerUnitTest()
        {
            // Setup
            _productAccessMock = new Mock<IDataAccess<Product>>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(
                _loggerMock.Object,
                _productAccessMock.Object
            );

        }

        [Fact]
        public void Get_Request_Returns_Latest_Items_OK()
        {
            // Arrange
            var expectedProducts = new List<Product>();
            _productAccessMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Returns(expectedProducts);

            // Act
            var result = _controller.GetLatestProducts();

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.Equal(200, okResult.StatusCode);

            var productList = okResult.Value as List<Product>;
            Assert.NotNull(productList);
            Assert.Equal(productList, expectedProducts);
        }

        [Fact]
        public void Get_Request_Returns_Latest_Items_Exception()
        {
            // Arrange
            _productAccessMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Throws(new Exception("Simulated error"));

            //Act
            var result = _controller.GetLatestProducts();

            // Assert
            var badResult = (BadRequestObjectResult)result;
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal("Simulated error", badResult.Value);
        }

        [Fact]
        public void Get_Latest_Items_Pricing_Euros_OK()
        {
            // Arrange
            var expectedBaseProducts = new List<Product>(); 
            _productAccessMock.Setup(
                x => x.List(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(expectedBaseProducts
            );

            var expectedFinalProducts = new List<ProductEUR>(); 
            foreach (var product in expectedBaseProducts)
            {
                expectedFinalProducts.Add(product.ProductGBPtoEUR());
            }

            // Act
            var result = _controller.GetLatestProductsPricingEuros();

            // Assert 
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            Assert.Equal(200, okResult.StatusCode);

            var productEUList = okResult.Value as List<ProductEUR>;
            Assert.NotNull(productEUList);
            Assert.Equal(productEUList, expectedFinalProducts);
        }
    }
}
