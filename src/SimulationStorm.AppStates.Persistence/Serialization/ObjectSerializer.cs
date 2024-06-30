using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using SimulationStorm.AppStates.Persistence.Serialization.JsonConverters;

namespace SimulationStorm.AppStates.Persistence.Serialization;

public static class ObjectSerializer
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonNode,
        Converters =
        {
            new TypeJsonConverter(),
            new Vector2JsonConverter(),
            new CultureInfoJsonConverter()
        }
    };
    
    public static string SerializeObject(object obj)
    {
        var objectWithType = new ObjectWithType
        {
            ObjectType = obj.GetType(),
            Object = obj
        };
        return JsonSerializer.Serialize(objectWithType, JsonSerializerOptions);
    }

    public static object DeserializeObject(string serializedObject)
    {
        var objectWithType = JsonSerializer.Deserialize<ObjectWithType>(serializedObject, JsonSerializerOptions);
        var jsonNode = (JsonNode)objectWithType.Object;
        return jsonNode.Deserialize(objectWithType.ObjectType, JsonSerializerOptions)!;
    }
    
    private struct ObjectWithType
    {
        public Type ObjectType { get; init; }
        
        public object Object { get; init; }
    }
}