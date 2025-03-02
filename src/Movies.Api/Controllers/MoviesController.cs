using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappings;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.ToEntity();

        await _movieRepository.CreateAsync(movie);
        
        var response = movie.ToReponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] string idOrSlug)
    {
        var deleted = Guid.TryParse(idOrSlug, out var id)
            ? await _movieRepository.DeleteByIdAsync(id)
            : await _movieRepository.DeleteBySlugAsync(idOrSlug);

        return deleted
            ? NoContent()
            : NotFound();
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieRepository.GetByIdAsync(id)
            : await _movieRepository.GetBySlugAsync(idOrSlug);

        return movie is not null
            ? Ok(movie.ToReponse())
            : NotFound();
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();

        return Ok(movies.ToReponse());
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.ToEntity(id);

        var updated = await _movieRepository.UpdateAsync(movie);

        return updated
            ? Ok(movie.ToReponse())
            : NotFound();
    }
}
