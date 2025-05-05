namespace wolds_hr_api.Helper.Dto.Responses;

public record JwtRefreshTokenActionResponse(bool IsAuthenticated, string JwtToken, string RefreshToken, ProfileResponse Profile, string Role);
public record LoginResponse(string Token, Profile Profile);
public record Profile(string FirstName, string LastName, string Email);
public record ProfileResponse(string FirstName, string LastName, string Email);