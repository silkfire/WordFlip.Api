namespace Wordsmith.WordFlip.Benchmarks
{
    using Services.Core;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Order;


    [RankColumn]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class WordFlipBenchmarks
    {
        //[Params("gabriel", )]
        //public string OriginalSentence { get; set; }

        private const string OriginalSentence = "   gab    ri    \r\n   l ";


        [Benchmark(Baseline = true)]
        public void Flip()
        {
            WordFlip.Flip(OriginalSentence);
        }

        [Benchmark]
        public void FlipSpanListSlices()
        {
            WordFlip.FlipSpanListSlices(OriginalSentence);
        }

        [Benchmark]
        public void FlipSpanValueTupleCustomRangesStringBuilder()
        {
            WordFlip.FlipSpanValueTupleCustomRangesStringBuilder(OriginalSentence);
        }

        [Benchmark]
        public void FlipSpanImmediateStringBuilder()
        {
            WordFlip.FlipSpanImmediateStringBuilder(OriginalSentence);
        }

        [Benchmark]
        public void FlipSpanMultiPassStringBuilder()
        {
            WordFlip.FlipSpanMultiPassStringBuilder(OriginalSentence);
        }

        [Benchmark]
        public void FlipReadOnlyMemoryAndSequence()
        {
            WordFlip.FlipReadOnlyMemoryAndSequence(OriginalSentence);
        }

        [Benchmark]
        public void FlipSpanPointerArray()
        {
            WordFlip.FlipSpanPointerArray(OriginalSentence);
        }
    }
}
