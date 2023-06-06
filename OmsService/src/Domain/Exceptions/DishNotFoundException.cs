using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class DishNotFoundException: DomainException
{
    public DishNotFoundException()
    {
    }

    protected DishNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public DishNotFoundException(string? message) : base(message)
    {
    }

    public DishNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}