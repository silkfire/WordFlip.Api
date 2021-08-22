namespace Wordsmith.WordFlip.Domain.Models
{
    using AggregatesModel.FlippedSentenceAggregate;

    using System;
    using System.Linq;
    using System.Text.RegularExpressions;


    public class Sentence
    {
        // [LEADING PUNCTUATION MARKS "']   [ANY CHARACTERS] (<--- only reverse this part)   [TRAILING PUNCTUATION MARKS .,:;!?…""']

        private static readonly Regex _flipRegex = new(@"^([""']+)?(.+?)([.,:;!?…""']+)?$");


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


            var endPosition = 1;
            var contiguousWhitespaces = 0;

            while (endPosition < sentence.Length - 1)
            {
                if (char.IsWhiteSpace(sentence[endPosition]))
                {
                    contiguousWhitespaces++;

                    while (char.IsWhiteSpace(sentence[endPosition]))
                    {
                        endPosition++;
                    }
                }
                else
                {
                    endPosition++;
                }
            }


            if (contiguousWhitespaces == 0) return sentence.ToString();

            var startPosition = 0;
            endPosition = 1;
            var stringLength = 0;

            while (endPosition < sentence.Length - 1)
            {
                if (char.IsWhiteSpace(sentence[endPosition]))
                {
                    // Word character(s) + 1 space

                    stringLength += endPosition - startPosition + 1;

                    while (char.IsWhiteSpace(sentence[endPosition]))
                    {
                        endPosition++;
                    }

                    startPosition = endPosition;
                }
                else
                {
                    endPosition++;
                }
            }

            stringLength += sentence.Length - startPosition;

            string sanitizedSentence;

            unsafe
            {
                fixed (char* ptr = sentence)
                {
                    sanitizedSentence = string.Create(stringLength, (Pointer: new[] { ptr }, OriginalLength: sentence.Length), (buffer, sentencePtr) =>
                    {
                        var startPosition = 0;
                        var endPosition = 1;
                        var bufferPosition = 0;

                        while (endPosition < sentencePtr.OriginalLength - 1)
                        {
                            if (char.IsWhiteSpace(sentencePtr.Pointer[0][endPosition]))
                            {
                                // Word character(s) + 1 space

                                var segmentLength = endPosition - startPosition + 1;

                                for (var i = 0; i < segmentLength; i++)
                                {
                                    buffer[bufferPosition++] = sentencePtr.Pointer[0][startPosition + i];
                                }

                                while (char.IsWhiteSpace(sentencePtr.Pointer[0][endPosition]))
                                {
                                    endPosition++;
                                }

                                startPosition = endPosition;
                            }
                            else
                            {
                                endPosition++;
                            }
                        }

                        {
                            var segmentLength = sentencePtr.OriginalLength - startPosition;

                            for (var i = 0; i < segmentLength; i++)
                            {
                                buffer[bufferPosition++] = sentencePtr.Pointer[0][startPosition + i];
                            }
                        }
                    });
                }
            }

            return string.Join(' ', sanitizedSentence.Split(' ').Select(w => _flipRegex.Replace(w, m => $"{m.Groups[1].Value}{new string(m.Groups[2].Value.Reverse().ToArray())}{m.Groups[3].Value}")));
        }

        public override string ToString() => Value;
    }
}
