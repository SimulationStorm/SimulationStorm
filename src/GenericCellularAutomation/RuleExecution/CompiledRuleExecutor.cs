using System;
using System.IO;
using System.Reflection;
using System.Text;
using GenericCellularAutomation.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SimulationStorm.Primitives;

namespace GenericCellularAutomation.RuleExecution;

public sealed class RuleExecutor : IRuleExecutor
{
    public byte CalculateNextCellState(byte[,] world, Point cellPosition)
    {
        int cellX = cellPosition.X,
            cellY = cellPosition.Y;
        
        var currentCellState = world[cellX, cellY];
        
        if (currentCellState != TARGET_CELL_STATE)
            return currentCellState;
        
        // Include if probability is 1 only.
        if (Random.Shared.NextDouble() >= APPLICATION_PROBABILITY)
            return currentCellState;
        //
        
        // Totalistic
        var neighborCount = 0;

        if (world[cellX + NEIGHBORHOOD_OFFSET_X, cellY + NEIGHBORHOOD_OFFSET_Y] == NEIGHBOR_CELL_STATE)
            neighborCount++;
        
        // <analogous if's>
        
        if (neighborCount == NEIGHBOR_COUNT_0
            || neighborCount == NEIGHBOR_COUNT_1) // <analogous checks>
                return NEW_CELL_STATE;

        return currentCellState;
        //
        
        // Nontotalistic
        if (world[cellX + OFFSET_X, cellY + OFFSET_Y] == NEIGHBOR_CELL_STATE
            && world[cellX + OFFSET_X_1, cellY + OFFSET_Y_1] == NEIGHBOR_CELL_STATE)
                return NEW_CELL_STATE;

        return currentCellState;
        //
    }
}

public sealed class CompiledRuleExecutorFactory : IRuleExecutorFactory
{
    private const string CodeHeader =
    @"
        namespace GenericCellularAutomation.RuleExecution;
        
        public sealed class RuleExecutor : IRuleExecutor
        {
            public byte CalculateNextCellState(byte[,] world, Point cellPosition)
            {
    ";
            
    private const string CodeFooter =
    @"
        }
    }
    ";
    
    public IRuleExecutor CreateRuleExecutor(Rule rule)
    {
        const string namespaceName = $"{nameof(GenericCellularAutomation)}.{nameof(RuleExecution)}";
        const string typeName = "RuleExecutor";
        
        var codeBuilder = new StringBuilder();
        codeBuilder.Append(CodeHeader);

        if (rule.ApplicationProbability is not 1)
        {
            codeBuilder.Append();
        }
        
        codeBuilder.Append(CodeFooter);

        var code = codeBuilder.ToString();
        
        var syntaxTree = CSharpSyntaxTree.ParseText(code);
        var compilation = CSharpCompilation
            .Create(namespaceName)
            .AddSyntaxTrees(syntaxTree)
            .AddReferences(
                // The reference to the System.
                CreateMetadataReference(typeof(Random)),
                // The reference to the GenericCellularAutomation.
                CreateMetadataReference(typeof(IRuleExecutor)))
            .WithOptions(
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    .WithOptimizationLevel(OptimizationLevel.Release));

        using var assemblyMemoryStream = new MemoryStream();
        compilation.Emit(assemblyMemoryStream);

        assemblyMemoryStream.Seek(0, SeekOrigin.Begin);
        var assemblyBytes = assemblyMemoryStream.ToArray();
        var assembly = Assembly.Load(assemblyBytes);

        var ruleExecutorType = assembly.GetType(typeName)!;
        var ruleExecutorInstance = (IRuleExecutor)Activator.CreateInstance(ruleExecutorType)!;
        return ruleExecutorInstance;
    }

    private static MetadataReference CreateMetadataReference(Type type) =>
        MetadataReference.CreateFromFile(type.Assembly.Location);
}