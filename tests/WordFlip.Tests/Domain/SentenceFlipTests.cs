namespace Wordsmith.WordFlip.Tests.Domain
{
    using WordFlip.Domain.Models;

    using Xunit;


    public class SentenceFlipTests
    {
        [Theory]
        [InlineData("Hello",
                    "olleH")]

        [InlineData("Hello \n    World",
                    "olleH dlroW")]

        [InlineData("The red fox crosses the ice, intent on none of my business.",
                    "ehT der xof sessorc eht eci, tnetni no enon fo ym ssenisub.")]

        [InlineData("For the 12-month period ending August 31, 2016, the airport had 72,351 aircraft operations, an average of 198 per day: 70% general aviation, 23% air taxi, 2% military and 5% scheduled commercial.",
                    "roF eht htnom-21 doirep gnidne tsuguA 13, 6102, eht tropria dah 153,27 tfarcria snoitarepo, na egareva fo 891 rep yad: %07 lareneg noitaiva, %32 ria ixat, %2 yratilim dna %5 deludehcs laicremmoc.")]

        [InlineData(@"""The Unknown"" was used during the credits of the 2011 film ""Soul Surfer"".",
                    @"""ehT nwonknU"" saw desu gnirud eht stiderc fo eht 1102 mlif ""luoS refruS"".")]

        [InlineData(@"""Commonly green or brown; however, some species present bright colors""",
                    @"""ylnommoC neerg ro nworb; revewoh, emos seiceps tneserp thgirb sroloc""")]

        [InlineData(@"The Hatter opened his eyes very wide on hearing this; but all he said was ""Why is a raven like a writing desk?""",
                    @"ehT rettaH denepo sih seye yrev ediw no gniraeh siht; tub lla eh dias saw ""yhW si a nevar ekil a gnitirw ksed?""")]

        [InlineData("He was without hope... desolate, empty… the epitome of a broken heart.",
                    "eH saw tuohtiw epoh... etalosed, ytpme… eht emotipe fo a nekorb traeh.")]

        [InlineData("Word1? word2: word3; 'word4'",
                    "1droW? 2drow: 3drow; '4drow'")]
        public void Should_produce_expected_flipped_sentence(string originalSentence, string expectedFlippedSentence)
        {
            /////////////
            // ARRANGE
            /////

            var sentence = new Sentence(originalSentence);


            /////////////
            // ACT
            /////

            var flippedSentence = sentence.Flip();


            /////////////
            // ASSERT
            /////

            Assert.Equal(expectedFlippedSentence, flippedSentence.Value);
        }
    }
}
