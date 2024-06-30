using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DotNext;

namespace SimulationStorm.GameOfLife.DataTypes;

[JsonConverter(typeof(GameOfLifeRuleJsonConverter))]
public partial class GameOfLifeRule : IEquatable<GameOfLifeRule>
{
	#region Constants
    public static readonly GameOfLifeRule Empty = FromString("b/s");

	public const int MinNeighborCount = 0,
                     MaxNeighborCount = 8;
	#endregion

    #region Properties
    private IReadOnlySet<int> NeighborCountsToBorn { get; }

    private IReadOnlySet<int> NeighborCountsToSurvive { get; }
    #endregion

    [GeneratedRegex("^b(?<born>0?1?2?3?4?5?6?7?8?)\\/s(?<survival>0?1?2?3?4?5?6?7?8?)$",
	    RegexOptions.IgnoreCase | RegexOptions.Compiled)] private static partial Regex _ruleRegex();

	public GameOfLifeRule(IReadOnlySet<int> neighborCountsToBorn, IReadOnlySet<int> neighborCountsToSurvive)
	{
		ValidateNeighborCounts(neighborCountsToBorn);
		ValidateNeighborCounts(neighborCountsToSurvive);

		NeighborCountsToBorn = neighborCountsToBorn;
		NeighborCountsToSurvive = neighborCountsToSurvive;
	}

	#region Public methods
    public bool IsBornWhen(int neighborCount) => NeighborCountsToBorn.Contains(neighborCount);

    public bool IsSurviveWhen(int neighborCount) => NeighborCountsToSurvive.Contains(neighborCount);

    public GameOfLifeRule WithNeighborCountToBorn(int neighborCount)
    {
        var neighborCountsToBorn = new HashSet<int>(NeighborCountsToBorn) { neighborCount };
        return new GameOfLifeRule(neighborCountsToBorn, NeighborCountsToSurvive);
    }

    public GameOfLifeRule WithoutNeighborCountToBorn(int neighborCount)
    {
        var neighborCountsToBorn = new HashSet<int>(NeighborCountsToBorn);
        neighborCountsToBorn.Remove(neighborCount);
        return new GameOfLifeRule(neighborCountsToBorn, NeighborCountsToSurvive);
    }

    public GameOfLifeRule WithNeighborCountToSurvive(int neighborCount)
    {
        var neighborCountsToSurvive = new HashSet<int>(NeighborCountsToSurvive) { neighborCount };
        return new GameOfLifeRule(NeighborCountsToBorn, neighborCountsToSurvive);
    }

    public GameOfLifeRule WithoutNeighborCountToSurvive(int neighborCount)
    {
        var neighborCountsToSurvive = new HashSet<int>(NeighborCountsToSurvive);
        neighborCountsToSurvive.Remove(neighborCount);
        return new GameOfLifeRule(NeighborCountsToBorn, neighborCountsToSurvive);
    }

    public override string ToString()
	{
		var stringBuilder = new StringBuilder();

		stringBuilder.Append('b');
		foreach (var neighborCount in NeighborCountsToBorn.Order())
            stringBuilder.Append(neighborCount);

        stringBuilder.Append("/s");
        foreach (var neighborCount in NeighborCountsToSurvive.Order())
            stringBuilder.Append(neighborCount);

		return stringBuilder.ToString();
	}

	public static GameOfLifeRule FromString(string ruleString)
	{
		ValidateRuleString(ruleString);
		
		var groups = _ruleRegex().Match(ruleString).Groups;

		HashSet<int> neighborCountsToBorn = [],
			neighborCountsToSurvival = [];

		foreach (var neighborCount in groups["born"].Value)
			neighborCountsToBorn.Add(neighborCount - '0');

		foreach (var neighborCount in groups["survival"].Value)
			neighborCountsToSurvival.Add(neighborCount - '0');

		return new GameOfLifeRule(neighborCountsToBorn, neighborCountsToSurvival);
	}

	public static GameOfLifeRule GenerateRandomRule()
	{
		HashSet<int> neighborCountsToBorn = [],
			neighborCountsToSurvival = [];

		for (var neighborCount = MinNeighborCount; neighborCount <= MaxNeighborCount; neighborCount++)
			if (Random.Shared.NextBoolean())
				neighborCountsToBorn.Add(neighborCount);

		for (var neighborCount = MinNeighborCount; neighborCount <= MaxNeighborCount; neighborCount++)
			if (Random.Shared.NextBoolean())
				neighborCountsToSurvival.Add(neighborCount);

		return new GameOfLifeRule(neighborCountsToBorn, neighborCountsToSurvival);
	}

    #region Equality comparing
    public override bool Equals(object? obj) => Equals(obj as GameOfLifeRule);

    public bool Equals(GameOfLifeRule? other) => ToString() == other?.ToString();

    public override int GetHashCode() => ToString().GetHashCode();
    #endregion
    #endregion

    #region Private methods
    private static void ValidateNeighborCounts(IReadOnlySet<int> neighborCounts)
    {
        foreach (var neighborCount in neighborCounts)
            if (neighborCount is < MinNeighborCount or > MaxNeighborCount)
                throw new ArgumentOutOfRangeException(nameof(neighborCounts), neighborCount,
                    $"Must be greater than or equal to {MinNeighborCount} and less than or equal to {MaxNeighborCount}.");
    }

    private static void ValidateRuleString(string ruleString)
    {
	    if (_ruleRegex().IsMatch(ruleString) == false)
		    throw new ArgumentException($"Must match the following regular expression: {_ruleRegex()}.", nameof(ruleString));
    }
    #endregion
    
    private class GameOfLifeRuleJsonConverter : JsonConverter<GameOfLifeRule>
    {
	    public override GameOfLifeRule Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		    FromString(reader.GetString()!);

	    public override void Write(Utf8JsonWriter writer, GameOfLifeRule value, JsonSerializerOptions options) =>
		    writer.WriteStringValue(value.ToString());
    }
}