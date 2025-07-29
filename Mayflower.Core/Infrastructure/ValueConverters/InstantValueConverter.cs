﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using NodaTime;

namespace Mayflower.Core.Infrastructure.ValueConverters
{
    public class InstantValueConverter
        : ValueConverter<Instant, long>
    {
        public InstantValueConverter()
            : base(v => v.ToUnixTimeTicks(), v => Instant.FromUnixTimeTicks(v))
        {
        }
    }
}