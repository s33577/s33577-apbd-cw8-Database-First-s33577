namespace Kolokwium.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message = "Resource not found") : base(message)
    {
    }
}