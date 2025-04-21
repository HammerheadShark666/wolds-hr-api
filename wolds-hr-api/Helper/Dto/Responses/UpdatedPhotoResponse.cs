namespace wolds_hr_api.Helper.Dto.Responses;

public class UpdatedPhotoResponse(int id, string filename)
{
    public int Id { get; set; } = id;
    public string Filename { get; set; } = filename;
}