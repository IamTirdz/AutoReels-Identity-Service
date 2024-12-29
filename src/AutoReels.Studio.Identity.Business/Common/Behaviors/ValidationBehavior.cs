using AutoReels.Studio.Identity.Common.Exceptions;
using AutoReels.Studio.Identity.Common.Models;
using FluentValidation;
using MediatR;
using System.Diagnostics;

namespace AutoReels.Studio.Identity.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(validators
                .Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(e => e.Errors.Count != 0)
                .SelectMany(r => r.Errors)
                .ToList();

            var errors = failures
                .GroupBy(p => p.PropertyName, e => e.ErrorMessage)
                .ToDictionary(f => f.Key, f => f.ToArray());

            if (failures.Count != 0)
                throw new InputValidationException(
                    new ErrorResponse
                    {
                        ReferenceKey = Activity.Current!.RootId!,
                        Message = "One or more validation errors occurred.",
                        Errors = errors
                    });

            return await next();
        }
    }
}
