using Newtonsoft.Json;
using System;
using System.Globalization;

public class DateOnlyConverter : JsonConverter<DateTime>
{
    private const string Format = "dd-MMM";

    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(Format, CultureInfo.InvariantCulture));
    }

    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return DateTime.ParseExact((string)reader.Value, Format, CultureInfo.InvariantCulture);
    }
}