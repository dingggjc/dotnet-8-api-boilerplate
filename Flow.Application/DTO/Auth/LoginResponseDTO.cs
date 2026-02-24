namespace Flow.Application.DTO.Auth;

public record LoginResponseDTO(
    string AccessToken,      
    string RefreshToken,        
    DateTimeOffset ExpiresAt,    
    string UserId,
    string Email,
    string FirstName,
    string LastName
);