namespace Wordsmith.WordFlip.Tests
{
    using Xunit;

    using Services.Core;


    public class WordFlipTests
    {
        [Theory]
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
        public void Flipped_Sentence_Must_Equal_Expected_Flipped_Sentence(string originalSentence, string expectedFlippedSentence)
        {
            /////////////
            // ARRANGE
            /////



            /////////////
            // ACT
            /////

            var flippedSentence = WordFlip.Flip(originalSentence);


            /////////////
            // ASSERT
            /////

            Assert.Equal(expectedFlippedSentence, flippedSentence);
        }
    }
}
