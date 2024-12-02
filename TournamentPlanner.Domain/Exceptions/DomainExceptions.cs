namespace TournamentPlanner.Domain.Exceptions;


public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
    public NotFoundException(string name) : base($"Entity \"{name}\" not found.")
    {

    }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "User is not authorized to perform this action.")
        : base(message)
    {
    }
}

public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "Access to the requested resource is forbidden.")
        : base(message)
    {
    }
}

public class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }
}

public class InternalServerErrorException : Exception
{
    public InternalServerErrorException(string message = "An unexpected error occurred.")
        : base(message)
    {
    }
}

public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string message = "The service is currently unavailable.")
        : base(message)
    {
    }
}

public class DependencyException : Exception
{
    public DependencyException(string message)
        : base(message)
    {
    }
}

public class AdminOwnershipException : Exception
{
    public AdminOwnershipException(string message = "Admin does not have ownership of the content.")
        : base(message)
    {
    }
}
