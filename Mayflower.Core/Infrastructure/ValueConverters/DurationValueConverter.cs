﻿using System;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using NodaTime;

namespace Mayflower.Core.Infrastructure.ValueConverters
{
    public class DurationValueConverter
        : ValueConverter<Duration, TimeSpan>
    {
        public DurationValueConverter()
            : base(v => v.ToTimeSpan(), v => Duration.FromTimeSpan(v))
        {
        }
    }
}