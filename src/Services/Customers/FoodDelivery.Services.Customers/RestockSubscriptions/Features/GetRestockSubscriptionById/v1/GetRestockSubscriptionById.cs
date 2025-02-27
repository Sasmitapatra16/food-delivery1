using AutoMapper;
using BuildingBlocks.Abstractions.Queries;
using BuildingBlocks.Core.Extensions;
using FluentValidation;
using FoodDelivery.Services.Customers.Customers.Data.UOW.Mongo;
using FoodDelivery.Services.Customers.RestockSubscriptions.Dtos.v1;
using FoodDelivery.Services.Customers.RestockSubscriptions.Exceptions.Application;

namespace FoodDelivery.Services.Customers.RestockSubscriptions.Features.GetRestockSubscriptionById.v1;

public record GetRestockSubscriptionById(Guid Id) : IQuery<GetRestockSubscriptionByIdResult>
{
    public static GetRestockSubscriptionById Of(Guid id)
    {
        id.NotBeEmpty();
        return new GetRestockSubscriptionById(id);
    }
}

internal class GetRestockSubscriptionByIdValidator : AbstractValidator<GetRestockSubscriptionById>
{
    public GetRestockSubscriptionByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

internal class GetRestockSubscriptionByIdHandler(CustomersReadUnitOfWork unitOfWork, IMapper mapper)
    : IQueryHandler<GetRestockSubscriptionById, GetRestockSubscriptionByIdResult>
{
    public async Task<GetRestockSubscriptionByIdResult> Handle(
        GetRestockSubscriptionById query,
        CancellationToken cancellationToken
    )
    {
        query.NotBeNull();

        var restockSubscription = await unitOfWork.RestockSubscriptionsRepository.FindOneAsync(
            x => x.IsDeleted == false && x.Id == query.Id,
            cancellationToken
        );

        if (restockSubscription is null)
        {
            throw new RestockSubscriptionNotFoundException(query.Id);
        }

        var subscriptionDto = mapper.Map<RestockSubscriptionDto>(restockSubscription);

        return new GetRestockSubscriptionByIdResult(subscriptionDto);
    }
}

public record GetRestockSubscriptionByIdResult(RestockSubscriptionDto RestockSubscription);
