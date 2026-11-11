using System;
using Moq;
using Simulation.Checkout.Application;
using Simulation.Checkout.Application.Abstractions;
using Simulation.Checkout.Domain;

namespace Simulation.Checkout.Tests;

public class ReceiptSnapshotTests
{
    [Fact]
    public async Task CheckoutAsync_WhenOrderMatchesExampleC_ShouldMatchVerifiedReceiptSnapshot()
    {
        // Arrange
        var inventory = new Mock<IInventoryService>();
        inventory
            .Setup(x => x.IsAvailableAsync(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var sut = new CheckoutService(
            new Mock<IOrderRepository>().Object,
            inventory.Object,
            new Mock<INotificationService>().Object);

        var order = new Order
        {
            Id = new Guid(1, 0, 0, new byte[8]),             //Guid.Parse("11111111-1111-1111-1111-111111111111"),
            CustomerName = "Aya Ahmed",
            CustomerEmail = "aya@example.com",
            Items = new List<OrderItem>
            {
                new()
                {
                    ProductId =new(2, 0, 0, new byte[8]),     //Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    ProductName = "Item 1",
                    UnitPrice = 500m,
                    Quantity = 3
                }
            }
        };

        // Act
        var receipt = await sut.CheckoutAsync(order);

        // Assert
        await Verify(receipt)
            .IgnoreMember("CreatedAt");
    }
}
