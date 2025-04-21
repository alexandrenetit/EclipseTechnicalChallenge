namespace TaskManagement.Domain.Exceptions;

/// <summary>
/// Custom exception for domain layer violations
/// </summary>
public class DomainException : Exception
{
    public DomainException()
    { }

    public DomainException(string message)
        : base(message)
    { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}