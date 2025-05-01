namespace wolds_hr_api.Helper.Dto.Responses;
public record MessageResponse(List<Message> Messages);

public record Message(string Text, string Severity);