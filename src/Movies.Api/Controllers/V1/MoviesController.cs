using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Constants;
using Movies.Api.Extensions;
using Movies.Api.Mappings;
using Movies.Api.Routes;
using Movies.Application.Services;
using Movies.Contracts.Requests.V1;
using Movies.Contracts.Responses.V1;

namespace Movies.Api.Controllers.V1;

[ApiController]
[ApiVersion(1.0)]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [Authorize(Auth.TrustedMemberPolicyName)]
    [HttpPost(Endpoints.Movies.Create)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = request.ToEntity();

        await _movieService.CreateAsync(movie, cancellationToken);

        var response = movie.ToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [Authorize(Auth.AdminUserPolicyName)]
    [HttpDelete(Endpoints.Movies.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _movieService.DeleteByIdAsync(id, cancellationToken);

        return deleted
            ? NoContent()
            : NotFound();
    }

    [HttpGet(Endpoints.Movies.Get)]
    [ResponseCache(Duration = 30, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id, userId, cancellationToken)
            : await _movieService.GetBySlugAsync(idOrSlug, userId, cancellationToken);

        return movie is not null
            ? Ok(movie.ToResponse())
            : NotFound();
    }

    [HttpGet(Endpoints.Movies.GetAll)]
    [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "title", "year", "sortBy", "pageNumber", "pageSize" }, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
    [ProducesResponseType(typeof(MoviesResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllMoviesRequest request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var options = request.ToOptions().WithUserId(userId);

        var movies = await _movieService.GetAllAsync(options, cancellationToken);
        var moviesCount = await _movieService.GetCountAsync(options.Title, options.ReleaseYear, cancellationToken);

        return Ok(movies.ToResponse(request.PageNumber, request.PageSize, moviesCount));
    }

    [Authorize(Auth.TrustedMemberPolicyName)]
    [HttpPut(Endpoints.Movies.Update)]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();

        var movie = request.ToEntity(id);

        var updatedMovie = await _movieService.UpdateAsync(movie, userId, cancellationToken);

        return updatedMovie != null
            ? Ok(movie.ToResponse())
            : NotFound();
    }
}
