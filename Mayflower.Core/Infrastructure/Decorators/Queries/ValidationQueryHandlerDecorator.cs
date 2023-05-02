using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Mayflower.Core.Validation;
using Mayflower.Core.Infrastructure.Interfaces.Queries;

namespace Mayflower.Core.Infrastructure.Decorators.Queries
{
    public class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly IValidator _validator;

        public ValidationQueryHandlerDecorator(IValidator validator, IQueryHandler<TQuery, TResult> handler)
        {
            _validator = validator;
            _handler = handler;
        }

        public Task<TResult> HandleAsync(TQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            _validator.ValidateObject(query);

            return _handler.HandleAsync(query);
        }
    }
}
