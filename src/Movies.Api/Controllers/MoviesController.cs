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
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, response);
    }

    [HttpDelete(ApiEndpoints.Movies.DeleteById)]
    public async Task<IActionResult> DeleteByDelete([FromRoute] Guid id)
    {
        var deleted = await _movieRepository.DeleteByIdAsync(id);

        return deleted
            ? NoContent()
            : NotFound();
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();

        return Ok(movies.ToReponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetById)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var movie  = await _movieRepository.GetByIdAsync(id);

        return movie is not null
            ? Ok(movie.ToReponse())
            : NotFound();
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
