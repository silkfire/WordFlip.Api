namespace Wordsmith.WordFlip.WebApi.Controllers
{
    using Extensions;
    using Models;

    using Services.SentenceFlipping;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;


    [Route("api/[controller]")]
    [ApiController]
    public class FlipController : ControllerBase
    {
        private readonly FlipSentenceService _flipSentenceService;
        private readonly GetLastFlippedSentencesService _getLastFlippedSentencesService;

        private readonly Configuration _configuration;



        public FlipController(FlipSentenceService flipSentenceService, GetLastFlippedSentencesService getLastFlippedSentencesService, IOptions<Configuration> configuration)
        {
            _flipSentenceService            = flipSentenceService;
            _getLastFlippedSentencesService = getLastFlippedSentencesService;

            _configuration                  = configuration.Value;
        }



        // POST api/flip
        [HttpPost]
        public async Task<IActionResult> Flip(FlipRequestDto request)
        {
            FlippedSentenceDto flippedSentence = null;


            if (request != null)
            {
                flippedSentence = FlippedSentenceDto.Convert(await _flipSentenceService.Flip(request.OriginalSentence));
            }


            if (flippedSentence == null)
            {
                return this.RespondWithJsonError(HttpStatusCode.BadRequest, "'originalSentence' cannot be null or empty.");
            }


            return new ObjectResult(flippedSentence);
        }

        // GET api/flip/getLastSentences
        [HttpGet("getLastSentences/{page?}")]
        public async IAsyncEnumerable<FlippedSentenceDto> GetLastSentences(int page = 1)
        {
            await foreach (var flippedSentence in _getLastFlippedSentencesService.Get(_configuration.ItemsPerPage, page))
            {
                yield return FlippedSentenceDto.Convert(flippedSentence);
            }
        }
    }
}
