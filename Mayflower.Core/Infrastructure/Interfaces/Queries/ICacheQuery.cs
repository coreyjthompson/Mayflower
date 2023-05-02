using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mayflower.Core.Infrastructure.Queries;

namespace Mayflower.Core.Infrastructure.Interfaces.Queries
{
    public interface ICacheQuery
    {
        CacheQueryOptions CacheQueryOptions { get; }
    }
}
