namespace Wordsmith.WordFlip.Api.Controllers
{
    using Models;

    using Services;
    using Services.Data;
    using Services.Data.Models;

    using Microsoft.AspNetCore.Mvc;

    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;


    [Route("api/[controller]")]
    [ApiController]
    public class FlipController : ControllerBase
    {
        private readonly WordFlipDataService _dataService;

        public FlipController(WordFlipDataService dataService)
        {
            _dataService = dataService;
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
                flippedSentence = WordFlippingService.Flip(payload.SourceSentence);
            }


            if (flippedSentence == null)
            {
                return RepondWithError(HttpStatusCode.BadRequest, "'sourceSentence' cannot be null or empty.");
            }



            //////////////
            // Save the flipped sentence to DB
            ///////

            await _dataService.NewFlippedSentence(flippedSentence);


            return new ObjectResult(new { resultSentence = flippedSentence });
        }


        // GET api/flip/getLastSentences
        [HttpGet("getLastSentences")]
        public async Task<ActionResult<IEnumerable<FlippedSentenceDto>>> GetLastSentences()
        {
            return await _dataService.GetLastSentences();
        }






        // GET error/{code}
        [Route("/error/{code}")]
        public IActionResult Error(int code)
        {
            return RepondWithError(HttpStatusCode.BadRequest, "An unexpected error occurred." );
        }
    }
}
