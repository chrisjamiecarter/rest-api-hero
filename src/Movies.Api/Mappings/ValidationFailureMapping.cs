﻿using FluentValidation;
using FluentValidation.Results;
using Movies.Contracts.Responses;

namespace Movies.Api.Mappings;

public static class ValidationFailureMapping
{
    public static ValidationResponse ToResponse(this ValidationFailure failure)
    {
        return new ValidationResponse(failure.PropertyName, failure.ErrorMessage);
    }

    public static ValidationFailureResponse ToResponse(this ValidationException exception)
    {
        return new ValidationFailureResponse(exception.Errors.Select(ToResponse));
    }
}
