using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimulationStorm.AppStates.Persistence.Serialization.JsonConverters;

public class Vector2JsonConverter : JsonConverter<Vector2>
{
    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartObject)
            throw new JsonException();

        float x = default;
        float y = default;

        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return new Vector2(x, y);
                    
                case JsonTokenType.PropertyName:
                {
                    var propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case nameof(Vector2.X):
                            x = reader.GetSingle();
                            break;
                        case nameof(Vector2.Y):
                            y = reader.GetSingle();
                            break;
                    }

                    break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber(nameof(Vector2.X), value.X);
        writer.WriteNumber(nameof(Vector2.Y), value.Y);
        writer.WriteEndObject();
    }
}