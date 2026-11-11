using FluentAssertions;
using Moq;
using Simulation.Checkout.Application;
using Simulation.Checkout.Application.Abstractions;
using Simulation.Checkout.Domain;
using Simulation.Checkout.Domain.Exceptions;

namespace Simulation.Checkout.Tests;

public class CheckoutServiceTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly Mock<IInventoryService> _inventoryMock = new();
    private readonly Mock<INotificationService> _notificationMock = new();

    private CheckoutService CreateSut(bool inventoryAvailable = true)
    {
        _inventoryMock
            .Setup(x => x.IsAvailableAsync(
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventoryAvailable);

        return new CheckoutService(
            _repositoryMock.Object,
            _inventoryMock.Object,
            _notificationMock.Object);
    }

    private static OrderItem CreateItem(string name, decimal price, int quantity) => new()
    {
        ProductId = Guid.NewGuid(),
        ProductName = name,
        UnitPrice = price,
        Quantity = quantity
    };

    private static Order CreateOrder(params OrderItem[] items) => new()
    {
        CustomerName = "Aya Ahmed",
        CustomerEmail = "aya@example.com",
        Items = items
    };
    [Fact]
    public async Task CheckoutAsync_WhenOrderIsEmpty_ShouldThrowEmptyOrderException()
    {
        //arrange
        var sut = CreateSut();
        var order = CreateOrder();

        //act
        var act = () => sut.CheckoutAsync(order);

        //assert
        await act.Should().ThrowAsync<EmptyOrderException>();
    }



    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CheckoutAsync_WhenQuantityIsNotPositive_ShouldThrowInvalidQuantityExceptionWithProductName(int quantity)
    {
        //arrange
        var sut = CreateSut();
        var order = CreateOrder(CreateItem("Keyboard", 100m, quantity));

        //act
        var act = () => sut.CheckoutAsync(order);

        //assert
        var exception = await act.Should().ThrowAsync<InvalidQuantityException>();
        exception.Which.Message.Should().Contain("Keyboard");
    }
}