using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayflower.Core.Infrastructure.Queries
{
    public class CacheQueryOptions
    {
        public CacheQueryOptions()
        {
            Size = 1;
        }

        public string? CacheKey { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public DateTimeOffset? AbsoluteExpiration { get; set; }

        public long Size { get; set; }
    }
}
