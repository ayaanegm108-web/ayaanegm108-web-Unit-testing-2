using System;
using FluentAssertions;
using NetArchTest.Rules;
using Simulation.Checkout.Domain;

namespace Simulation.Checkout.Tests;

public class ArchitectureTests
{
    [Fact]
    public void Domain_ShouldNotDependOnApplicationOrInfrastructure()
    {
        // Arrange
        var domainAssembly = typeof(Order).Assembly;

        // Act
        var result = Types.InAssembly(domainAssembly)
            .Should()
            .NotHaveDependencyOnAny(
                "Simulation.Checkout.Application",
                "Simulation.Checkout.Infrastructure")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}