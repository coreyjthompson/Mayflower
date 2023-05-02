using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Mayflower.Core.Infrastructure.Interfaces.Queries;

namespace Mayflower.Core.Infrastructure.Decorators.Queries
{
    public class AuthorizationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IPrincipal _currentUser;
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly ILogger<AuthorizationQueryHandlerDecorator<TQuery, TResult>> _logger;

        public AuthorizationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> handler, IPrincipal currentUser, ILogger<AuthorizationQueryHandlerDecorator<TQuery, TResult>> logger)
        {
            _handler = handler;
            _currentUser = currentUser;
            _logger = logger;
        }

        public Task<TResult> HandleAsync(TQuery query)
        {
            Authorize();

            return _handler.HandleAsync(query);
        }

        private void Authorize()
        {
            var ns = typeof(TQuery).Namespace;

            if (ns?.Contains("Admin") == true && !_currentUser.IsInRole("Admin"))
            {
                throw new SecurityException();
            }

            _logger.LogError("User " + _currentUser.Identity?.Name + " has been authorized to execute " + typeof(TQuery).Name);
        }
    }
}
