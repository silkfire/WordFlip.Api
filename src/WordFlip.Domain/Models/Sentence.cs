namespace Wordsmith.WordFlip.Domain.Models
{
    using AggregatesModel.FlippedSentenceAggregate;

    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Sentence
    {
        // [LEADING PUNCTUATION MARKS "']   [ANY CHARACTERS] (<--- only reverse this part)   [TRAILING PUNCTUATION MARKS .,:;!?…""']

        private static readonly Regex _flipRegex = new(@"^([""']+)?(.+?)([.,:;!?…""']+)?$", RegexOptions.Compiled);

        private static readonly Regex _consecutiveWhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

        public string Value { get; }

        public Sentence(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Reverses each individual word of the given sentence, preserving original word order as well as a predefined set of leading and trailing punctuation marks.
        /// </summary>
        public FlippedSentence Flip()
        {
            return new FlippedSentence(Flip(Value));
        }

        /// <summary>
        /// Reverses each individual word of a sentence, preserving original word order as well as a predefined set of leading and trailing punctuation marks.
        /// </summary>
        /// <param name="sentence">A sentence whose individual words to reverse.</param>
        private static string Flip(ReadOnlySpan<char> sentence)
        {
            if (sentence.IsEmpty || sentence.IsWhiteSpace())
            {
                return null;
            }

            sentence = sentence.Trim();

            // Merge any consecutive whitespace into one space character.

            return string.Join(' ', _consecutiveWhitespaceRegex.Split(sentence.Trim().ToString()).Select(w => _flipRegex.Replace(w, m => $"{m.Groups[1].Value}{new string(m.Groups[2].Value.Reverse().ToArray())}{m.Groups[3].Value}")));
        }

        public override string ToString() => Value;
    }
}
