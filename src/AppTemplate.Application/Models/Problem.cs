namespace AppTemplate.Application.Models;

public class Problem
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required ProblemType ProblemType { get; set; }
    public required IEnumerable<string> Details { get; set; }
    
    /// <summary>
    /// Most generic instance of a Problem
    /// </summary>
    /// <param name="details">Human-readable description of the issue that was encountered</param>
    public static Problem General(string details) => new()
    {
        Title = "General Application Problem",
        Description = "Something went wrong",
        ProblemType = ProblemType.Crash,
        Details = details.ToEnumerable()
    };

    /// <summary>
    /// Indicates that a request object had one or more properties out of range
    /// </summary>
    /// <param name="details">Specific details about the property that was out of range</param>
    public static Problem ValidationFailed(IEnumerable<string> details) => new()
    {
        Title = "Validation Error",
        Description = "Object had one or more properties containing inconsistent values",
        ProblemType = ProblemType.Validation,
        Details = details
    };
    
    /// <summary>
    /// Indicates that a request object had one or more properties out of range
    /// </summary>
    /// <param name="details">Specific details about the property that was out of range</param>
    public static Problem RequestValidationFailed(IEnumerable<string> details) => new()
    {
        Title = "Request could not be validated",
        Description = "The provided request object was rejected by the model because one or more properties were out of range",
        ProblemType = ProblemType.Validation,
        Details = details
    };
    
    /// <summary>
    /// An exception was thrown by the application and is converted into a problem instance
    /// </summary>
    /// <param name="details">Error message provided by the Exception</param>
    public static Problem ExceptionCaught(IEnumerable<string> details) => new()
    {
        Title = "Model returned unsuccessfully",
        Description = "The request failed because the underlying model crashed during execution. Please see the logs for further details.",
        ProblemType = ProblemType.Crash,
        Details = details
    };
    public static Problem ExceptionCaught(Exception exception) => ExceptionCaught(exception.Message.ToEnumerable());
    public static Problem ExceptionCaught() => ExceptionCaught(Enumerable.Empty<string>());
    
    /// <summary>
    /// A requested object cannot be found in the database
    /// </summary>
    public static Problem EntityNotFound<TEntity>(string key) => new()
    {
        Title = "Requested entity not found",
        Description = $"The requested entity of type {typeof(TEntity)} with key {key} could not be found",
        ProblemType = ProblemType.DataUnavailable,
        Details = $"Key = {key}".ToEnumerable()
    };

    /// <summary>
    /// Data could not be persisted in the database
    /// </summary>
    public static Problem FailedToPersist<TEntity>(string details) => new()
    {
        Title = "Failed to persist entity",
        Description = $"The entity of type {typeof(TEntity)} could not be persisted",
        ProblemType = ProblemType.DataStorage,
        Details = details.ToEnumerable()
    };
}

public enum ProblemType
{
    /// <summary>
    /// Indicates that some piece of data could not be persisted
    /// </summary>
    DataStorage,
    
    /// <summary>
    /// Indicates that some piece of data required to process the request is not available
    /// </summary>
    DataUnavailable,
    
    /// <summary>
    /// Indicates that some kind of object validation failed
    /// </summary>
    Validation,
    
    /// <summary>
    /// Indicates that the application encountered an irrecoverable error 
    /// </summary>
    Crash
}

internal static class ProblemExtensions
{
    public static IEnumerable<string> ToEnumerable(this string s) => Enumerable.Empty<string>().Append(s);
}
