using System.Diagnostics;
using Mayflower.Core.Infrastructure.Interfaces.Queries;

namespace Mayflower.Core.Infrastructure.Queries
{
    public class DynamicQueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public DynamicQueryProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //[DebuggerStepThrough]
        public async Task<TResult> Execute<TResult>(IQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _serviceProvider.GetService(handlerType) ?? new object(); ;

            return await handler?.HandleAsync((dynamic) query);
        }
    }
}
