namespace Mecesoft.MediatR;

/// <summary>Marker interface for a request without a response.</summary>
public interface IRequest { }

/// <summary>Marker interface for a request with a response.</summary>
public interface IRequest<TResponse> { }

/// <summary>Handles a request without a response.</summary>
public interface IRequestHandler<TRequest> where TRequest : IRequest
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>Handles a request and returns a response.</summary>
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>Legacy delegate used by void request pipeline behaviors.</summary>
public delegate Task RequesteHandlerDelete();

/// <summary>Legacy delegate used by response request pipeline behaviors.</summary>
public delegate Task<TResponse> RequesteHandlerDelete<TResponse>();

/// <summary>Wraps execution of a void request handler.</summary>
public interface IPipelineBehavior<TRequest>
    where TRequest : IRequest
{
    Task Handle(
        TRequest request,
        RequesteHandlerDelete next,
        CancellationToken cancellationToken = default
    );
}

/// <summary>Wraps execution of a response request handler.</summary>
public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(
        TRequest request,
        RequesteHandlerDelete<TResponse> next,
        CancellationToken cancellationToken = default
    );
}

/// <summary>Marker interface for a notification.</summary>
public interface INotification { }

/// <summary>Handles a notification.</summary>
public interface INotificationHandler<TNotification> where TNotification : INotification
{
    Task Handle(TNotification notification, CancellationToken cancellationToken = default);
}