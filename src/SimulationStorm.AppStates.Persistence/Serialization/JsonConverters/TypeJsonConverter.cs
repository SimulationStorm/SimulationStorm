using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimulationStorm.AppStates.Persistence.Serialization.JsonConverters;

public class TypeJsonConverter : JsonConverter<Type>
{
    public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Type.GetType(reader.GetString()!)!;

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.AssemblyQualifiedName);

    public override Type ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        Type.GetType(reader.GetString()!)!;

    public override void WriteAsPropertyName(Utf8JsonWriter writer, Type value, JsonSerializerOptions options) =>
        writer.WritePropertyName(value.AssemblyQualifiedName!);
}