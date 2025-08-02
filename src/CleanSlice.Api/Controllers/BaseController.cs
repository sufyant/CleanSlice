using CleanSlice.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace CleanSlice.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException("Cannot handle success result as failure"),
            _ when result.Error is ValidationError validationError =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationError.Errors)),
            _ => MapErrorToStatusCode(result.Error)
        };

    private IActionResult MapErrorToStatusCode(Error error) =>
        error.Type switch
        {
            ErrorType.NotFound => NotFound(
                CreateProblemDetails("Not Found", StatusCodes.Status404NotFound, error)),
            ErrorType.Conflict => Conflict(
                CreateProblemDetails("Conflict", StatusCodes.Status409Conflict, error)),
            ErrorType.Validation => BadRequest(
                CreateProblemDetails("Validation Error", StatusCodes.Status400BadRequest, error)),
            ErrorType.Problem => BadRequest(
                CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, error)),
            _ => BadRequest(
                CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, error))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Description,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
