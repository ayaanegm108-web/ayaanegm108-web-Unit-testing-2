using System;

using AutoFixture;
using Simulation.Checkout.Domain;

namespace Simulation.Checkout.Tests.TestData;

public static class OrderFixture
{
    public static Fixture Create()
    {
        var fixture = new Fixture();

        fixture.Customize<OrderItem>(composer => composer
            .With(item => item.Quantity, () => Random.Shared.Next(1, 10))
            .With(item => item.UnitPrice, () => Random.Shared.Next(0, 500)));

        return fixture;
    }
}