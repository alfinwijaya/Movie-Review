using Movie.Models;
using Movie.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Movie.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieReviewController : ControllerBase
{
    private readonly MovieReviewService _movieService;

    public MovieReviewController(MovieReviewService movieService) =>
        _movieService = movieService;

    [HttpGet]
    public async Task<List<MovieReview>> Get() =>
        await _movieService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<MovieReview>> Get(string id)
    {
        var movie = await _movieService.GetAsync(id);

        if (movie is null)
        {
            return NotFound();
        }

        return movie;
    }

    [HttpPost("insert")]
    public async Task<IActionResult> Post([FromQuery][BindRequired] string id, [FromQuery] Review review)
    {
        MovieReview mr = new MovieReview
        {
            MovieId = id,
            Review = new Review[] { review }
        };
        await _movieService.CreateAsync(mr);

        return CreatedAtAction(nameof(Get), new { id = mr.MovieId }, review);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery][BindRequired] string id, [FromQuery] Review updatedReview)
    {
        var movie = await _movieService.GetAsync(id);

        if (movie is null)
        {
            return NotFound();
        }
        MovieReview mr = new MovieReview { MovieId = movie.MovieId,
                                           Review = new Review[] { updatedReview }
        };

        await _movieService.UpdateAsync(mr);

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery][BindRequired] string movieId,[FromQuery][BindRequired] string userId)
    {
        var movie = await _movieService.GetAsync(movieId);

        if (movie is null)
        {
            return NotFound();
        }

        movie.Review = movie.Review.ToList().Where(a => a.UserId == userId).ToArray();
        await _movieService.RemoveAsync(movie);

        return NoContent();
    }
}