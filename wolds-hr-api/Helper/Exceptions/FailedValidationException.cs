using wolds_hr_api.Helper.Dto.Responses;

namespace wolds_hr_api.Helper.Exceptions;
public class FailedValidationException(FailedValidationResponse failedValidationResponse) : Exception
{
    public FailedValidationResponse FailedValidationResponse { get; set; } = failedValidationResponse;
}