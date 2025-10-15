namespace wolds_hr_api.Library.Dto.Responses;

public record LoginResponse(string Token, string RefreshToken, Profile Profile);
public record Profile(string FirstName, string LastName, string Email);
public record ProfileResponse(string FirstName, string LastName, string Email);
public record JwtRefreshToken(bool IsAuthenticated, string Token, string RefreshToken, Profile Profile);