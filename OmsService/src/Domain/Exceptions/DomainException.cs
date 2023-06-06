using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class DomainException: Exception
{
    public DomainException()
    {
    }

    protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DomainException(string? message) : base(message)
    {
    }

    public DomainException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}