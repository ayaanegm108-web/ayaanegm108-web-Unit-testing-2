using System;
using FluentAssertions;
using NetArchTest.Rules;
using Simulation.Checkout.Domain;
using Simulation.Checkout.Application;
namespace Simulation.Checkout.Tests;
using Simulation.Checkout.Infrastructure;

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


    [Fact]
    public void Application_ShouldNotDependOnInfrastructure()
    {
        // Arrange
        var applicationAssembly = typeof(CheckoutService).Assembly;

        // Act
        var result = Types.InAssembly(applicationAssembly)
            .Should()
            .NotHaveDependencyOn("Simulation.Checkout.Infrastructure")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }



    [Fact]
    public void Interfaces_ShouldStartWithI()
    {
        // Arrange
        var assemblies = new[]
        {
            typeof(Order).Assembly,
            typeof(CheckoutService).Assembly,
            typeof(InMemoryOrderRepository).Assembly
        };

        // Act
        var result = Types.InAssemblies(assemblies)
            .That()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        // Assert
        result.IsSuccessful.Should().BeTrue();
    }
}