using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class NotEnoughQuantityException: DomainException
{
    public NotEnoughQuantityException()
    {
    }

    protected NotEnoughQuantityException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public NotEnoughQuantityException(string? message) : base(message)
    {
    }

    public NotEnoughQuantityException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}