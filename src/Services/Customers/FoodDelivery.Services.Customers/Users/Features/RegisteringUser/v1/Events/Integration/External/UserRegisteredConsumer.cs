using BuildingBlocks.Abstractions.Commands;
using BuildingBlocks.Core.Events;
using FoodDelivery.Services.Customers.Customers.Features.CreatingCustomer.v1;
using FoodDelivery.Services.Shared.Identity.Users.Events.V1.Integration;
using MassTransit;

namespace FoodDelivery.Services.Customers.Users.Features.RegisteringUser.v1.Events.Integration.External;

public class UserRegisteredConsumer(ICommandBus commandBus) : IConsumer<EventEnvelope<UserRegisteredV1>>
{
    public async Task Consume(ConsumeContext<EventEnvelope<UserRegisteredV1>> context)
    {
        var userRegistered = context.Message.Data;
        if (userRegistered.Roles is null || !userRegistered.Roles.Contains(CustomersConstants.Role.User))
            return;

        await commandBus.SendAsync(new CreateCustomer(userRegistered.Email));
    }
}
