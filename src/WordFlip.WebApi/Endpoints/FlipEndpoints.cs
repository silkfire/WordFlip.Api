namespace Wordsmith.WordFlip.WebApi.Endpoints;

using Models;
using Services;

using Domain;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using System.Linq;
using System.Threading.Tasks;

public static class FlipEndpoints
{
    public static void RegisterFlipEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/flip");

        group.MapPost("/", Flip);
        group.MapGet("/getLastSentences", GetLastSentences);
    }

    private static async Task<IResult> Flip([FromBody] FlipRequestDto? request, [FromServices] FlipSentenceService flipSentenceService, [FromServices] IOptions<Configuration> configuration, [FromQuery(Name = "p")] int? page)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.OriginalSentence))
        {
            return Results.BadRequest(new { Message = "'originalSentence' cannot be null or empty" });
        }

        var flipResult = await flipSentenceService.Flip(request.OriginalSentence, configuration.Value.ItemsPerPage, SanitizePageNumberValue(page));

        return Results.Ok(flipResult);
    }

    private static async Task<IResult> GetLastSentences([FromServices] FlipSentenceService flipSentenceService, [FromServices] IOptions<Configuration> configuration, [FromQuery(Name = "p")] int? page = 1)
    {
        var result = await flipSentenceService.GetLastSentences(configuration.Value.ItemsPerPage, SanitizePageNumberValue(page));

        return Results.Ok(new PaginatedResult<FlippedSentenceDto>
                          {
                              TotalCount = result.TotalCount,
                              PageSize = result.PageSize,
                              Items = result.Items.Select(FlippedSentenceDto.Convert).ToList().AsReadOnly()
                          });
    }

    private static int SanitizePageNumberValue(int? page)
    {
        return page is null
                    or < 1 ? 1 : page.Value;
    }
}
