using Greggs.Products.Api.Models;
using System;
using Xunit;

namespace Greggs.Products.UnitTests
{
    public class ProductUnitTest
    {
        [Fact]
        public void Product_GBP_To_EUR_Correct()
        {
            // Arrange
            var testProduct = new Product() { Name = "Test Product", PriceInPounds = 1.0m };

            // Act
            var result = testProduct.ProductGBPtoEUR();

            //Assert
            Assert.IsType<ProductEUR>(result);
            Assert.Equal(1.11m, result.PriceInEuros);
        }
    }
}
