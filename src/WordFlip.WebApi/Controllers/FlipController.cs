namespace Wordsmith.WordFlip.WebApi.Controllers
{
    using Models;
    using Utils;

    using Services.Core;
    using Services.Data.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;


    [Route("api/[controller]")]
    [ApiController]
    public class FlipController : ControllerBase
    {
        private readonly WordFlippingService _flippingService;
        private readonly ApiSettings _settings;



        public FlipController(WordFlippingService flippingService, IOptions<ApiSettings> settings)
        {
            _flippingService = flippingService;
            _settings        = settings.Value;
        }



        // POST api/flip
        [HttpPost]
        public async Task<IActionResult> Flip(FlipPayload payload)
        {
            FlippedSentenceDto flippedSentenceRecord = null;


            if (payload != null)
            {
                flippedSentenceRecord = await _flippingService.Flip(payload.OriginalSentence);
            }


            if (flippedSentenceRecord == null)
            {
                return this.RespondWithJsonError(HttpStatusCode.BadRequest, "'originalSentence' cannot be null or empty.");
            }


            return new ObjectResult(flippedSentenceRecord);
        }

        // GET api/flip/getLastSentences
        [HttpGet("getLastSentences/{page?}")]
        public async Task<ActionResult<IEnumerable<FlippedSentenceDto>>> GetLastSentences(int page = 1)
        {
            return await _flippingService.GetLastSentences(_settings.ItemsPerPage, page);
        }
    }
}
