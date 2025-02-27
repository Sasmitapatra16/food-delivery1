using BuildingBlocks.Abstractions.Commands;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Email;
using BuildingBlocks.Email.Options;
using FluentValidation;
using FoodDelivery.Services.Customers.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FoodDelivery.Services.Customers.RestockSubscriptions.Features.SendingRestockNotification.v1;

public record SendRestockNotification(long RestockSubscriptionId, int CurrentStock) : InternalCommand, ITxRequest;

internal class SendRestockNotificationValidator : AbstractValidator<SendRestockNotification>
{
    public SendRestockNotificationValidator()
    {
        RuleFor(x => x.RestockSubscriptionId).NotEmpty();

        RuleFor(x => x.CurrentStock).NotEmpty();
    }
}

internal class SendRestockNotificationHandler : ICommandHandler<SendRestockNotification>
{
    private readonly CustomersDbContext _customersDbContext;
    private readonly IEmailSender _emailSender;
    private readonly EmailOptions _emailConfig;
    private readonly ILogger<SendRestockNotificationHandler> _logger;

    public SendRestockNotificationHandler(
        CustomersDbContext customersDbContext,
        IEmailSender emailSender,
        IOptions<EmailOptions> emailConfig,
        ILogger<SendRestockNotificationHandler> logger
    )
    {
        _customersDbContext = customersDbContext;
        _emailSender = emailSender;
        _emailConfig = emailConfig.Value;
        _logger = logger;
    }

    public async Task Handle(SendRestockNotification command, CancellationToken cancellationToken)
    {
        command.NotBeNull();

        var restockSubscription = await _customersDbContext.RestockSubscriptions.FirstOrDefaultAsync(
            x => x.Id == command.RestockSubscriptionId,
            cancellationToken: cancellationToken
        );

        if (_emailConfig.Enable && restockSubscription is not null)
        {
            await _emailSender.SendAsync(
                new EmailObject(
                    restockSubscription.Email!,
                    _emailConfig.From,
                    "Restock Notification",
                    $"Your product {restockSubscription.ProductInformation.Name} is back in stock. Current stock is {command.CurrentStock}"
                )
            );

            _logger.LogInformation("Restock notification sent to email {Email}", restockSubscription.Email);
        }
    }
}
