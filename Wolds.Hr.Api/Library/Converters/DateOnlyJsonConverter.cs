namespace wolds_hr_api.Librarys.Converters;

using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class DateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    private readonly string _format = "yyyy-MM-dd";

    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        if (DateOnly.TryParseExact(value, _format, out var date))
        {
            return date;
        }

        throw new JsonException($"Invalid DateOnly format: {value}");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString(_format));
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}