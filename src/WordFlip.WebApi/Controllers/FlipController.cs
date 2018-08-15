namespace Wordsmith.WordFlip.WebApi.Controllers
{
    using Models;

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
        private readonly WordFlippingService   _flippingService;
        private readonly ApiSettings _settings;



        public FlipController(WordFlippingService flippingService, IOptions<ApiSettings> settings)
        {
            _flippingService = flippingService;
            _settings        = settings.Value;
        }



        private IActionResult RepondWithError(HttpStatusCode statusCode, string message)
        {
            return StatusCode((int)statusCode, new ErrorResult { Error = message });
        }



        // POST api/flip
        [HttpPost]
        public async Task<IActionResult> Flip(FlipPayload payload)
        {
            string flippedSentence = null;


            if (payload != null)
            {
                flippedSentence = await _flippingService.Flip(payload.OriginalSentence);
            }


            if (flippedSentence == null)
            {
                return RepondWithError(HttpStatusCode.BadRequest, "'originalSentence' cannot be null or empty.");
            }


            return new ObjectResult(new { resultSentence = flippedSentence });
        }


        // GET api/flip/getLastSentences
        [HttpGet("getLastSentences/{page?}")]
        public async Task<ActionResult<IEnumerable<FlippedSentenceDto>>> GetLastSentences(int page = 1)
        {
            return await _flippingService.GetLastSentences(_settings.ItemsPerPage, page);
        }






        // GET error/{code}
        [Route("/error/{code}")]
        public IActionResult Error(int code)
        {
            return RepondWithError(HttpStatusCode.BadRequest, "An unexpected error occurred." );
        }
    }
}
