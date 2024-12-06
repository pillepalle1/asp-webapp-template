using System.Text.Json;

namespace AppTemplate.Application.Cqrs.Common;

internal abstract class ARequestHandler<TRequest, TResponse>(
    ILogger<ARequestHandler<TRequest, TResponse>> logger,
    IEnumerable<IValidator<TRequest>> validators) : IRequestHandler<TRequest, OneOf<TResponse, Problem>>
    where TRequest : ARequest<TResponse>
{
    public abstract Task<OneOf<TResponse, Problem>> HandleImpl(TRequest request, CancellationToken cancellationToken);
    
    public async Task<OneOf<TResponse, Problem>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        // TIME THE OPERATION
        request.Stopwatch.Restart();
        
        // ------------------------------------------------------------------------------------------------------------
        // PERFORM THE ACTUAL OPERATION
        var response = await GenerateResponseAsync(request, cancellationToken);
        await PostProcessResponseAsync(request, response, cancellationToken);
        // ------------------------------------------------------------------------------------------------------------
        
        // END TIMING
        request.Stopwatch.Stop();
        
        return response;
    }

    private async Task<OneOf<TResponse, Problem>> GenerateResponseAsync(TRequest request, CancellationToken cancellationToken)
    {
        // LOG INCOMING REQUESTS
        // This allows for tracing what happened in the past
        if (request.ShouldBeLogged)
        {
            logger.LogInformation(
                "[Processing: {RequestId}] {Request}", 
                request.MediatorRequestId,
                JsonSerializer.Serialize(request));
        }

        // VALIDATE INCOMING REQUESTS
        // I am deeply agitated that this cannot be achieved through an IPipelineBehaviour, but apparently it is not
        // possible to use OneOf in IPipelineHandlers to return a Problem Instance there
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.Where(r => r.Errors.Any()).SelectMany(x => x.Errors).ToList();

            if (failures.Any()) 
                return Problem.RequestValidationFailed(failures.Select(x => x.ErrorMessage));
        }

        // WRAP LOGIC INTO TRY-CATCH
        // We do not want to expose Exceptions to the consumer. If an exception is thrown, it is converted to a Problem instance
        // and the user is informed about the failed request that way
        try
        {
            return await HandleImpl(request, cancellationToken);
        }
        catch (Exception e)
        {
            return Problem.ExceptionCaught(e);
        }
    }
    private async Task PostProcessResponseAsync(
        TRequest request,
        OneOf<TResponse, Problem> response,
        CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        
        // LOG DURATION FOR SUCCESSFUL AND REASONS FOR FAILED REQUESTS
        if (request.ShouldBeLogged)
        {
            if (response.Succeeded())
            {
                logger.LogInformation(
                    "{RequestId} completed in {RequestCompletedElapsed}",
                    request.MediatorRequestId,
                    request.GetElapsedTime());
            }
            else
            {
                var problem = response.Problem();
                logger.LogWarning(
                    "Failed: {RequestId} | {Details}",
                    request.MediatorRequestId,
                    problem.Details.Aggregate(problem.ProblemType.ToString(), (prev, next) => $"{prev}; {next}"));
            }
        }
    }
}