﻿namespace Movies.Contracts.Requests;

public sealed record GetAllMoviesRequest(string? Title, int? Year);
