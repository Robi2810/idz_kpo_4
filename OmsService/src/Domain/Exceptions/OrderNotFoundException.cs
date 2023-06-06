using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class OrderNotFoundException: DomainException
{
    public OrderNotFoundException()
    {
    }

    protected OrderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public OrderNotFoundException(string? message) : base(message)
    {
    }

    public OrderNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}