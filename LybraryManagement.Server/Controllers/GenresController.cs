using LybraryManagement.Server.Application.Services;
using LybraryManagement.Shared.Library.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LybraryManagement.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenresController(IGenreService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll(CancellationToken ct)
        => Ok(await service.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreDto>> GetById(int id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> Create([FromBody] CreateGenreDto dto, CancellationToken ct)
    {
        var created = await service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = created.GenreId }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GenreDto>> Update(int id, [FromBody] UpdateGenreDto dto, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, dto, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
        => await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}

