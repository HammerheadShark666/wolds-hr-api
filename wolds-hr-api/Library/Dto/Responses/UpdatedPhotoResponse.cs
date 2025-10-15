namespace wolds_hr_api.Library.Dto.Responses;

public class UpdatedPhotoResponse(Guid id, string filename)
{
    public Guid Id { get; set; } = id;
    public string Filename { get; set; } = filename;
}