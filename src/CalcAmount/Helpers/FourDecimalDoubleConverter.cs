using Newtonsoft.Json;
using System;
using System.Globalization;

namespace CalcAmount.Helpers
{
    public class FourDecimalDoubleConverter : JsonConverter<double>
    {
        public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value.ToString("F4", CultureInfo.InvariantCulture));
        }

        public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Convert.ToDouble(reader.Value);
        }

        public override bool CanRead => true;
    }
}