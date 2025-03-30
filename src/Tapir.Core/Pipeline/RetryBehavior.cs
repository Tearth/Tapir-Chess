using MediatR;
using Tapir.Core.Persistence.Exceptions;

namespace Tapir.Core.Pipeline
{
    public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            const int MAX_ATTEMPTS = 5;

            for (var attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                try
                {
                    return await next();
                }
                catch (AggregateConcurrentWriteException)
                {
                    if (attempt >= MAX_ATTEMPTS)
                    {
                        throw;
                    }
                }
                catch
                {
                    throw;
                }
            }

            throw new InvalidOperationException("RetryBehavior reached an invalid state.");
        }
    }
}
