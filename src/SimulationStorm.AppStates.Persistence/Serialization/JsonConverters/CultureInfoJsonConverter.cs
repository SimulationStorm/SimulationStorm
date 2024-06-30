using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimulationStorm.AppStates.Persistence.Serialization.JsonConverters;

public class CultureInfoJsonConverter : JsonConverter<CultureInfo>
{
    public override CultureInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        CultureInfo.GetCultureInfo(reader.GetInt32());

    public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.LCID);
}