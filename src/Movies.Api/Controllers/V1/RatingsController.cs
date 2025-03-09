//using Asp.Versioning;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Movies.Api.Extensions;
//using Movies.Api.Mappings;
//using Movies.Api.Routes;
//using Movies.Application.Services;
//using Movies.Contracts.Requests.V1;
//using Movies.Contracts.Responses.V1;

//namespace Movies.Api.Controllers.V1;

//[ApiController]
//[ApiVersion(1.0)]
//public class RatingsController : ControllerBase
//{
//    private readonly IRatingService _ratingService;

//    public RatingsController(IRatingService ratingService)
//    {
//        _ratingService = ratingService;
//    }

//    [Authorize]
//    [HttpDelete(ApiRoutes.Movies.DeleteMovieRating)]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    public async Task<IActionResult> DeleteMovieRating([FromRoute] Guid id, CancellationToken cancellationToken)
//    {
//        var userId = HttpContext.GetUserId();

//        var deleted = await _ratingService.DeleteRatingAsync(userId!.Value, id, cancellationToken);

//        return deleted
//            ? NoContent()
//            : NotFound();
//    }

//    [Authorize]
//    [HttpGet(ApiRoutes.Ratings.GetUserRatings)]
//    [ProducesResponseType(typeof(MovieRatingsResponse), StatusCodes.Status200OK)]
//    public async Task<IActionResult> GetUserRatings(CancellationToken cancellationToken)
//    {
//        var userId = HttpContext.GetUserId();

//        var result = await _ratingService.GetRatingsForUserAsync(userId!.Value, cancellationToken);

//        return Ok(result.ToResponse());
//    }

//    [Authorize]
//    [HttpPut(ApiRoutes.Movies.RateMovie)]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    public async Task<IActionResult> RateMovie([FromRoute] Guid id, [FromBody] RateMovieRequest request, CancellationToken cancellationToken)
//    {
//        var userId = HttpContext.GetUserId();

//        var result = await _ratingService.RateMovieAsync(userId!.Value, id, request.Rating, cancellationToken);

//        return result
//            ? NoContent()
//            : NotFound();
//    }
//}
