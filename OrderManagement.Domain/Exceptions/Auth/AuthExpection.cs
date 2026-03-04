namespace OrderManagement.Domain.Exceptions.Auth;

public class AuthException : DomainException
{
    public AuthException(string message)
        : base(message)
    {
    }
}


// public class InvalidCredentialsException : AuthException
// {
//     public InvalidCredentialsException() 
//         : base("Invalid email or password") { }
// }

// public class TokenExpiredException : AuthException
// {
//     public TokenExpiredException() 
//         : base("Token has expired") { }
// }

// public class TokenRevokedException : AuthException
// {
//     public TokenRevokedException() 
//         : base("Token has been revoked") { }
// }

// public class UserAlreadyExistsException : AuthException
// {
//     public UserAlreadyExistsException(string email) 
//         : base($"User with email {email} already exists") { }
// }