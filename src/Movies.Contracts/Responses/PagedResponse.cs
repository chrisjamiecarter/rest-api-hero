﻿using System.Diagnostics.CodeAnalysis;

namespace Movies.Contracts.Responses;

public class PagedResponse<TResponse>
{
    public required IEnumerable<TResponse> Items { get; init; } = Enumerable.Empty<TResponse>();

    public required int PageNumber { get; init; }

    public required int PageSize { get; init; }

    public required int Total { get; init; }

    public bool HasNextPage => Total > (PageNumber * PageSize);
}
