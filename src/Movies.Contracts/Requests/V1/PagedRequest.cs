namespace Movies.Contracts.Requests.V1;

public record PagedRequest(int PageNumber = 1, int PageSize = 10);
