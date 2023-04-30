using Mayflower.Core.Infrastructure.Commands;
using System.Threading.Tasks;
using System.Transactions;

namespace Mayflower.Core.Infrastructure.Commands.Decorators
{
    public class TransactionCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _decorated;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand, TResult> decorated)
        {
            _decorated = decorated;
        }

        public async Task<TResult> HandleAsync(TCommand command)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _decorated.HandleAsync(command);

                scope.Complete();

                return result;
            }
        }
    }
}