using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using LazyCache;
using Mayflower.Core.Infrastructure.Interfaces.Queries;

namespace Mayflower.Core.Infrastructure.Decorators.Queries
{
    public class CacheQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IAppCache _cache;
        private readonly IQueryHandler<TQuery, TResult> _handler;

        public CacheQueryHandlerDecorator(IQueryHandler<TQuery, TResult> handler, IAppCache cache)
        {
            _handler = handler;
            _cache = cache;
        }

        public Task<TResult> HandleAsync(TQuery query)
        {
            if (query is ICacheQuery cacheQuery)
            {
                if (cacheQuery.CacheQueryOptions == null)
                {
                    throw new ArgumentException("CacheQueryOptions must have a value.");
                }

                if (string.IsNullOrEmpty(cacheQuery.CacheQueryOptions.CacheKey))
                {
                    throw new ArgumentException("CacheKey must have a value.");
                }

                if (cacheQuery.CacheQueryOptions.AbsoluteExpiration != null)
                {
                    return _cache.GetOrAddAsync(cacheQuery.CacheQueryOptions.CacheKey, () => _handler.HandleAsync(query), cacheQuery.CacheQueryOptions.AbsoluteExpiration.Value);
                }

                if (cacheQuery.CacheQueryOptions.SlidingExpiration != null)
                {
                    return _cache.GetOrAddAsync(cacheQuery.CacheQueryOptions.CacheKey, () => _handler.HandleAsync(query), cacheQuery.CacheQueryOptions.SlidingExpiration.Value);
                }

                return _cache.GetOrAddAsync(cacheQuery.CacheQueryOptions.CacheKey, () => _handler.HandleAsync(query));
            }

            return _handler.HandleAsync(query);
        }
    }
}
