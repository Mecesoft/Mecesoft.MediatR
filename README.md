# Mecesoft.MediatR

Mecesoft.MediatR is a lightweight mediator for .NET 8 and .NET 10. It supports
requests, notifications, dependency-injection scopes, and ordered pipeline behaviors
without a dependency on the MediatR package.

## Installation

```bash
dotnet add package Mecesoft.MediatR
```

## Registration

```csharp
builder.Services.AddMediatR(options =>
{
    options.AddRegisterAssemblies(typeof(Program).Assembly);
    options.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
```

`ISender` and discovered handlers are registered as transient services. Resolve
`ISender` from the request scope; this allows handlers to use scoped services such
as `DbContext` safely.

## Requests

```csharp
public sealed record GetUser(Guid Id) : IRequest<User>;

public sealed class GetUserHandler(IUserRepository repository)
    : IRequestHandler<GetUser, User>
{
    public Task<User> Handle(GetUser request, CancellationToken cancellationToken) =>
        repository.GetAsync(request.Id, cancellationToken);
}

var user = await sender.Send(new GetUser(id), cancellationToken);
```

Void requests implement `IRequest` and use `IRequestHandler<TRequest>`.
Cancellation tokens are passed to handlers and pipeline behaviors unchanged.

## Pipelines and notifications

For response requests, a pipeline behavior declares both request and response:

```csharp
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequesteHandlerDelete<TResponse> next,
        CancellationToken cancellationToken = default) =>
        await next();
}
```

Void requests use `IPipelineBehavior<TRequest>` and `RequesteHandlerDelete`.
Pipeline behaviors run in registration order. Notifications run concurrently by
default for compatibility with earlier releases. Applications whose notification
handlers share scoped dependencies that are not concurrency-safe can opt into
sequential execution:

```csharp
options.PublishNotificationsConcurrently = false;
```

Notification handler failures and request handler failures propagate to the caller
without being wrapped in reflection exceptions.

## Compatibility

The existing `IRequest`, `IRequest<TResponse>`, `IRequestHandler`, `ISender`,
`Sender`, `INotification`, `INotificationHandler`, `IPipelineBehavior`, and
`AddMediatR` APIs remain available. The legacy `RequesteHandlerDelete` delegate
names are also retained for source compatibility.

## License

MIT. See [LICENSE](LICENSE).
