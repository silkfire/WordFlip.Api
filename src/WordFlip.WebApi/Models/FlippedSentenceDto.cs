﻿namespace Wordsmith.WordFlip.WebApi.Models
{
    using Domain.AggregatesModel.FlippedSentenceAggregate;

    using System;

    public class FlippedSentenceDto
    {
        /// <summary>
        /// The ID of the flipped sentence record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The flipped sentence.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Timestamp of when the flipped sentence was saved.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        public static FlippedSentenceDto Convert(FlippedSentence domainModel)
        {
            if (domainModel == null)
            {
                return null;
            }

            return new FlippedSentenceDto
            {
                Id = domainModel.Id,
                Value = domainModel.Value,
                Created = domainModel.Created
            };
        }

        public override string ToString() => Value;
    }
}
