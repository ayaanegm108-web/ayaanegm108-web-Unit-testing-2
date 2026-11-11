using System;
using Bogus;

namespace Simulation.Checkout.Tests.TestData;

public static class CustomerFaker
{
    public const int Seed = 2026;

    public static (string Name, string Email) Create()
    {
        var faker = new Faker { Random = new Randomizer(Seed) };

        var name = faker.Name.FullName();
        var email = faker.Internet.Email(name);

        return (name, email);
    }
}
