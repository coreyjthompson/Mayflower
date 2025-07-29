﻿using System;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using NodaTime;

namespace Mayflower.Core.Infrastructure.ValueConverters
{
    public class ZonedDateTimeValueConverter
        : ValueConverter<ZonedDateTime, DateTimeOffset>
    {
        public ZonedDateTimeValueConverter()
            : base(v => v.ToDateTimeOffset(), v => ZonedDateTime.FromDateTimeOffset(v))
        {
        }
    }
}