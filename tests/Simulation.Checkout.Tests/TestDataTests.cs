using System;
using AutoFixture;
using FluentAssertions;
using Moq;
using Simulation.Checkout.Application;
using Simulation.Checkout.Application.Abstractions;
using Simulation.Checkout.Domain;
using Simulation.Checkout.Tests.TestData;

namespace Simulation.Checkout.Tests;

public class TestDataTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly Mock<IInventoryService> _inventoryMock = new();
    private readonly Mock<INotificationService> _notificationMock = new();

    private CheckoutService CreateSut()
    {
        _inventoryMock
            .Setup(x => x.IsAvailableAsync(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        return new CheckoutService(
            _repositoryMock.Object,
            _inventoryMock.Object,
            _notificationMock.Object);
    }

    [Fact]
    public async Task CheckoutAsync_WhenOrderIsCreatedByAutoFixture_ShouldSucceed()
    {
        // Arrange
        var fixture = OrderFixture.Create();
        var order = fixture.Create<Order>();
        var sut = CreateSut();

        // Act
        var receipt = await sut.CheckoutAsync(order);

        // Assert
        receipt.OrderId.Should().Be(order.Id);
        receipt.Items.Should().HaveCount(order.Items.Count);
    }

    [Fact]
    public async Task CheckoutAsync_WhenCustomerIsCreatedByBogus_ShouldReturnReceiptWithSameNameAndEmail()
    {
        // Arrange
        var (name, email) = CustomerFaker.Create();
        var order = new Order
        {
            CustomerName = name,
            CustomerEmail = email,
            Items = new List<OrderItem>
            {
                new()
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Notebook",
                    UnitPrice = 50m,
                    Quantity = 2
                }
            }
        };
        var sut = CreateSut();

        // Act
        var receipt = await sut.CheckoutAsync(order);

        // Assert
        receipt.CustomerName.Should().Be(name);
        receipt.CustomerEmail.Should().Be(email);
    }
}