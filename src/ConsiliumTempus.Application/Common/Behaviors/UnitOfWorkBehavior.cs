using ConsiliumTempus.Application.Common.Interfaces.Persistence;
using ErrorOr;
using MediatR;

namespace ConsiliumTempus.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (IsNotCommand()) return await next();
        
        var response = await next();

        if (response is IErrorOr { IsError: false } || response is not IErrorOr)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return response;
    }

    private static bool IsNotCommand()
    {
        return !typeof(TRequest).Name.EndsWith("Command");
    }
}