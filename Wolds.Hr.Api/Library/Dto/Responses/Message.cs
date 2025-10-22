namespace Wolds.Hr.Api.Library.Dto.Responses;
public record MessageResponse(List<Message> Messages);

public record Message(string Text, string Severity);