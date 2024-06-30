using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimulationStorm.ToolPanels.Presentation;

[JsonConverter(typeof(ToolPanelJsonConverter))]
public class ToolPanel(string name) : IEquatable<ToolPanel>
{
    public string Name { get; } = name;

    #region Equality memebers
    public bool Equals(ToolPanel? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        
        if (ReferenceEquals(this, other))
            return true;
        
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        
        if (ReferenceEquals(this, obj))
            return true;
        
        return obj.GetType() == GetType() && Equals((ToolPanel)obj);
    }

    public override int GetHashCode() => Name.GetHashCode();

    public static bool operator ==(ToolPanel? left, ToolPanel? right) => Equals(left, right);

    public static bool operator !=(ToolPanel? left, ToolPanel? right) => !Equals(left, right);
    #endregion

    private class ToolPanelJsonConverter : JsonConverter<ToolPanel>
    {
        public override ToolPanel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ToolPanel value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Name);
        
        public override ToolPanel ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

        public override void WriteAsPropertyName(Utf8JsonWriter writer, ToolPanel value, JsonSerializerOptions options) =>
            writer.WritePropertyName(value.Name);
    }
}