﻿using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mappings;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = request.ToEntity();

        await _movieService.CreateAsync(movie, cancellationToken);
        
        var response = movie.ToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _movieService.DeleteByIdAsync(id, cancellationToken);

        return deleted
            ? NoContent()
            : NotFound();
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken cancellationToken)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await _movieService.GetByIdAsync(id, cancellationToken)
            : await _movieService.GetBySlugAsync(idOrSlug, cancellationToken);

        return movie is not null
            ? Ok(movie.ToResponse())
            : NotFound();
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var movies = await _movieService.GetAllAsync(cancellationToken);

        return Ok(movies.ToResponse());
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = request.ToEntity(id);

        var updatedMovie = await _movieService.UpdateAsync(movie, cancellationToken);

        return updatedMovie != null
            ? Ok(movie.ToResponse())
            : NotFound();
    }
}
