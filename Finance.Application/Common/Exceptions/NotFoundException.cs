
namespace Finance.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
    : base(message)
    {
    }
    public static NotFoundException ForEntity(
    string entity,
    object key)
    => new($"{entity} with id '{key}' was not found.");
}
