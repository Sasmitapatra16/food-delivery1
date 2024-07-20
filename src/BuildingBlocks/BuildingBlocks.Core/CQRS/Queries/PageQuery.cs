using BuildingBlocks.Abstractions.CQRS.Queries;
using BuildingBlocks.Core.Paging;

namespace BuildingBlocks.Core.CQRS.Queries;

// https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/tutorials/records#characteristics-of-records
public record PageQuery<TResponse> : PageRequest, IPageQuery<TResponse>
    where TResponse : notnull;
