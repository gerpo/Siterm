using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Siterm.Settings.Models;

namespace Siterm.Settings.Converter
{
    internal class SettingJsonConverter : JsonConverter<Setting>
    {
        public override Setting Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Setting value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(value.Name.ToString());
            writer.WriteStringValue(value.Value);
            writer.WriteEndObject();
        }
    }
}