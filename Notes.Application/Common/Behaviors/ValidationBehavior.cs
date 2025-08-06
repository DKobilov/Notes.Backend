using FluentValidation;
using MediatR;

namespace Notes.Application.Common.Behaviors
{  //фильтр, работает до вызова действий контроллера
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        { //request -это объект запроса переданный через метод IMediator.Send()
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
               .Select(v => v.Validate(context))
               .SelectMany(result => result.Errors)
               .Where(failure => failure != null)
               .ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
