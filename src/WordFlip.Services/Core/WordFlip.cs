namespace Wordsmith.WordFlip.Services.Core
{
    using System.Linq;
    using System.Text.RegularExpressions;


    public static class WordFlip
    {
        // [LEADING PUNCTUATION MARKS "']   [ANY CHARACTERS] (<--- only reverse this part)   [TRAILING PUNCTUATION MARKS .,:;!?…""']

        private static readonly Regex FlipRegex = new Regex(@"^([""']+)?(.+?)([.,:;!?…""']+)?$");



        /// <summary>
        /// Reverses each individual word of a sentence, preserving original word order as well as a predefined set of leading and trailing punctuation marks.
        /// </summary>
        /// <param name="sentence">A sentence whose individual words to reverse.</param>
        public static string Flip(string sentence)
        {
            if (string.IsNullOrWhiteSpace(sentence))
            {
                return null;
            }


            // Merge any consecutive whitespace into one space character and trim the string.

            var sanitizedSentence = Regex.Replace(sentence.Trim(), @"\s+", " ");


            return string.Join(' ', sanitizedSentence.Split(' ').Select(w => FlipRegex.Replace(w, m => $"{m.Groups[1].Value}{new string(m.Groups[2].Value.Reverse().ToArray())}{m.Groups[3].Value}")));
        }
    }
}
