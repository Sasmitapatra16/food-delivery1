using System.Diagnostics.CodeAnalysis;
using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Extensions;
using FoodDelivery.Services.Customers.Customers.Exceptions.Domain;

namespace FoodDelivery.Services.Customers.Customers.ValueObjects;

// https://learn.microsoft.com/en-us/ef/core/modeling/constructors
public class CustomerName : ValueObject
{
    // EF
    private CustomerName() { }

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string FullName => FirstName + " " + LastName;

    public static CustomerName Of([NotNull] string? firstName, [NotNull] string? lastName)
    {
        firstName.NotBeNullOrWhiteSpace();
        lastName.NotBeNullOrWhiteSpace();

        if (firstName.Length is > 100 or < 3)
        {
            throw new InvalidNameException(firstName);
        }

        if (lastName.Length is > 100 or < 3)
        {
            throw new InvalidNameException(lastName);
        }

        return new CustomerName { FirstName = firstName, LastName = lastName };
    }

    public void Deconstruct(out string firstName, out string lastName) => (firstName, lastName) = (FirstName, LastName);

    // will call for equality(==) checks.
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
