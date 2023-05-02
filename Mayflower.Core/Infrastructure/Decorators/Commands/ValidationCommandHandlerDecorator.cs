using System;
using System.Threading.Tasks;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Mayflower.Core.Validation;

namespace Mayflower.Core.Infrastructure.Decorators.Commands
{
    public class ValidationCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;
        private readonly IValidator _validator;

        public ValidationCommandHandlerDecorator(IValidator validator, ICommandHandler<TCommand, TResult> handler)
        {
            _validator = validator;
            _handler = handler;
        }

        public Task<TResult> HandleAsync(TCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _validator.ValidateObject(command);

            return _handler.HandleAsync(command);
        }
    }
}