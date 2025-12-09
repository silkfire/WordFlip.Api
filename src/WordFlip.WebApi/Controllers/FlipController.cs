namespace Wordsmith.WordFlip.WebApi.Controllers;

using Extensions;
using Models;

using Domain;

using Services.SentenceFlipping;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Linq;
using System.Net;
using System.Threading.Tasks;

[Route("[controller]")]
[ApiController]
public class FlipController(FlipSentenceService flipSentenceService, IOptions<Configuration> configuration) : ControllerBase
{
    private readonly Configuration _configuration = configuration.Value;

    // POST api/flip
    [HttpPost]
    public async Task<IActionResult> Flip(FlipRequestDto request)
    {
        FlippedSentenceDto flippedSentence = null;

        if (request != null)
        {
            flippedSentence = FlippedSentenceDto.Convert(await flipSentenceService.Flip(request.OriginalSentence));
        }

        if (flippedSentence == null)
        {
            return this.RespondWithJsonError(HttpStatusCode.BadRequest, "'originalSentence' cannot be null or empty.");
        }

        return new ObjectResult(flippedSentence);
    }

    // GET api/flip/getLastSentences
    [HttpGet("getLastSentences/{page?}")]
    public async Task<ActionResult<PaginatedResult<FlippedSentenceDto>>> GetLastSentences(int page = 1)
    {
        var result = await flipSentenceService.GetLastSentences(_configuration.ItemsPerPage, page);

        return Ok(new PaginatedResult<FlippedSentenceDto>
        {
            TotalCount = result.TotalCount,
            PageSize = result.PageSize,
            Items = result.Items.Select(FlippedSentenceDto.Convert).ToList().AsReadOnly()
        });
    }
}
