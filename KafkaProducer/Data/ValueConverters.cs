using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public static class ValueConverters
{
    public static readonly ValueConverter<List<string>, string> ListStringToJsonConverter =
        new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize<List<string>>(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

    public static readonly ValueConverter<Dictionary<string, string>, string> DictionaryStringToJsonConverter =
        new ValueConverter<Dictionary<string, string>, string>(
            v => JsonSerializer.Serialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>());
}