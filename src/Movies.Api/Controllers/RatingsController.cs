using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Extensions;
using Movies.Api.Mappings;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [Authorize]
    [HttpDelete(Endpoints.Movies.DeleteMovieRating)]
    public async Task<IActionResult> DeleteMovieRating([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _ratingService.DeleteRatingAsync(userId!.Value, id, cancellationToken);

        return result
            ? Ok()
            : NotFound();
    }

    [Authorize]
    [HttpGet(Endpoints.Ratings.GetUserRatings)]
    public async Task<IActionResult> GetUserRatings(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _ratingService.GetRatingsForUserAsync(userId!.Value, cancellationToken);

        return Ok(result.ToResponse());
    }

    [Authorize]
    [HttpPut(Endpoints.Movies.RateMovie)]
    public async Task<IActionResult> RateMovie([FromRoute] Guid id, [FromBody] RateMovieRequest request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var result = await _ratingService.RateMovieAsync(userId!.Value, id, request.Rating, cancellationToken);

        return result
            ? Ok()
            : NotFound();
    }
}
