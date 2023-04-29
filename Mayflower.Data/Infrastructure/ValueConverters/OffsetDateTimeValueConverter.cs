using System;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using NodaTime;

namespace Mayflower.Core.Infrastructure.ValueConverters
{
    public class OffsetDateTimeValueConverter
        : ValueConverter<OffsetDateTime, DateTimeOffset>
    {
        public OffsetDateTimeValueConverter()
            : base(v => v.ToDateTimeOffset(), v => OffsetDateTime.FromDateTimeOffset(v))
        {
        }
    }
}