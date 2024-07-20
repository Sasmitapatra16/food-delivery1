using BuildingBlocks.Abstractions.Domain.Events.Internal;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Core.Domain.Events.Internal;
using FoodDelivery.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Domain;

namespace FoodDelivery.Services.Catalogs.Products.Features.CreatingProduct.v1.Events.Notification;

internal record ProductCreatedNotification(ProductCreated DomainEvent)
    : DomainNotificationEventWrapper<ProductCreated>(DomainEvent);

internal class ProductCreatedHandler : IDomainNotificationEventHandler<ProductCreatedNotification>
{
    private readonly IBus _bus;

    public ProductCreatedHandler(IBus bus)
    {
        _bus = bus;
    }

    public Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        // We could publish integration event to bus here
        // await _bus.PublishAsync(
        //     new FoodDelivery.Services.Shared.Catalogs.Products.Events.Integration.ProductCreatedV1(
        //         notification.InternalCommandId,
        //         notification.Name,
        //         notification.Stock,
        //         notification.CategoryName ?? "",
        //         notification.Stock),
        //     null,
        //     cancellationToken);

        return Task.CompletedTask;
    }
}
