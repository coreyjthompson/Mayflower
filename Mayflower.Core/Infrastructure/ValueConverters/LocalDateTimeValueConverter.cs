using System;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using NodaTime;

namespace Mayflower.Core.Infrastructure.ValueConverters
{
    public class LocalDateTimeValueConverter
        : ValueConverter<LocalDateTime, DateTime>
    {
        public LocalDateTimeValueConverter()
            : base(v => v.ToDateTimeUnspecified(), v => LocalDateTime.FromDateTime(v))
        {
        }
    }
}