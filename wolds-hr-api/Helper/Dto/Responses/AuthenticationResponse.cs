namespace SwanSong.Domain.Dto;

public record ValidateResetTokenRequest(string Token);
public record LoginActionResponse(string JwtToken, string RefreshToken, ProfileResponse profile);
public record LoginRequest(string Email, string Password, bool IsAuthenticated, string JwtToken, string RefreshToken);
public record JwtRefreshTokenRequest(string RefreshToken);
public record JwtRefreshTokenActionResponse(bool IsAuthenticated, string JwtToken, string RefreshToken, ProfileResponse profile, string Role);
public record Authenticated(string JwtToken, Profile Profile); //string RefreshToken, 
public record Profile(string FirstName, string LastName, string Email);
public record JwtRefreshToken(bool IsAuthenticated, string JwtToken, string RefreshToken, Profile profile, string Role);
public record ProfileResponse(string FirstName, string LastName, string Email);