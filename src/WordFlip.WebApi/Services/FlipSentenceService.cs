namespace Wordsmith.WordFlip.WebApi.Services;

using Models;

using Domain;
using Domain.AggregatesModel.FlippedSentenceAggregate;
using Wordsmith.WordFlip.Domain.Models;

using System.Linq;
using System.Threading.Tasks;

public class FlipSentenceService(IFlippedSentenceRepository flippedSentenceRepository)
{
    public async Task<FlipResult> Flip(string sentence, int itemsPerPage, int page)
    {
        var sentenceToFlip = new Sentence(sentence);
        var flippedSentence = sentenceToFlip.Flip();


        //////////////
        // Save the flipped sentence to DB
        ///////

        var addedFlippedSentence = await flippedSentenceRepository.Add(flippedSentence);

        var lastSentencesExcludingLatest = await flippedSentenceRepository.GetLastSentences(itemsPerPage, page, true);

        return new FlipResult
               {
                   FlippedSentence = FlippedSentenceDto.Convert(addedFlippedSentence),
                   LastSentences = new PaginatedResult<FlippedSentenceDto>
                                   {
                                        TotalCount = lastSentencesExcludingLatest.TotalCount,
                                        PageSize = lastSentencesExcludingLatest.PageSize,
                                        Items = lastSentencesExcludingLatest.Items.Select(FlippedSentenceDto.Convert).ToList().AsReadOnly()
                                   } 
               };
    }

    public Task<PaginatedResult<FlippedSentence>> GetLastSentences(int itemsPerPage, int page)
    {
        return flippedSentenceRepository.GetLastSentences(itemsPerPage, page);
    }
}
