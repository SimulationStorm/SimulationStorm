using System;
using System.IO;
using System.Reflection;
using System.Text;
using DotNext.Collections.Generic;
using GenericCellularAutomation.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GenericCellularAutomation.RuleExecution;

public sealed class CompiledRuleExecutorFactory : IRuleExecutorFactory
{
    public IRuleExecutor CreateRuleExecutor(Rule rule)
    {
        const string className = "RuleExecutor";

        var sourceCode = GenerateSourceCode(className, rule);
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var compilation = CreateCompilation(syntaxTree);
        var assembly = CreateAssembly(compilation);
        
        var type = assembly.GetType(className)!;
        return (IRuleExecutor)Activator.CreateInstance(type)!;
    }

    private static string GenerateSourceCode(string className, Rule rule)
    {
        var sourceCodeBuilder = new StringBuilder();
        
        sourceCodeBuilder.Append
        (@$"
        using GcaCellState = byte;

        namespace GenericCellularAutomation.RuleExecution;
        
        public sealed class {className} : IRuleExecutor
        {{
            public GcaCellState CalculateNextCellState(GcaCellState[,] world, Point cellPosition)
            {{
                int cellX = cellPosition.X,
                    cellY = cellPosition.Y;
            
                GcaCellState currentCellState = world[cellX, cellY];
                
                if (currentCellState != {rule.TargetCellState})
                    return currentCellState;
                
        ");

        if (rule.ApplicationProbability is not 1)
        {
            sourceCodeBuilder.Append
            (@$"
            if (System.Random.Shared.NextDouble() >= {rule.ApplicationProbability})
                return currentCellState;
            
            ");
        }

        if (rule.Type is RuleType.Unconditional)
        {
            sourceCodeBuilder.Append
            (@$"
            return {rule.NewCellState};
            ");
        }
        else if (rule.Type is RuleType.Totalistic)
        {
            sourceCodeBuilder.Append
            (@"
            int cellNeighborCount = 0;
            
            ");
            
            rule.CellNeighborhood!.UsedPositions.ForEach(positionShift =>
            {
                sourceCodeBuilder.Append
                (@$"
                if (world[cellX + {positionShift.X}, cellY + {positionShift.Y}] == {rule.NeighborCellState})
                    cellNeighborCount++;
                
                ");
            });
            
            rule.NeighborCellCountSet!.ForEach(neighborCount =>
            {
                sourceCodeBuilder.Append
                (@$"
                if (neighborCount == {neighborCount})
                    return {rule.NewCellState};
                
                ");
            });
            
            sourceCodeBuilder.Append
            (@"
            return currentCellState;
            ");
        }
        else if (rule.Type is RuleType.Nontotalistic)
        {
            rule.NeighborCellPositionSet!.ForEach(neighborCellPos =>
            {
                sourceCodeBuilder.Append
                (@$"
                if (world[cellX + {neighborCellPos.X}, cellY + {neighborCellPos.Y}] != {rule.NeighborCellState})
                    return currentCellState;

                ");
            });
            
            sourceCodeBuilder.Append
            (@$"
            return {rule.NewCellState};
            ");
        }
        
        sourceCodeBuilder.Append(
        @"
            }
        }
        ");

        return sourceCodeBuilder.ToString();
    }

    private static CSharpCompilation CreateCompilation(SyntaxTree syntaxTree) =>
        CSharpCompilation
            .Create(assemblyName: null)
            .AddSyntaxTrees(syntaxTree)
            .AddReferences(
                // The reference to the System.
                CreateMetadataReference(typeof(Random)),
                // The reference to the GenericCellularAutomation.
                CreateMetadataReference(typeof(IRuleExecutor)))
            .WithOptions(
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    .WithOptimizationLevel(OptimizationLevel.Release));

    private static Assembly CreateAssembly(CSharpCompilation compilation)
    {
        using var memoryStream = new MemoryStream();
        compilation.Emit(memoryStream);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var assemblyBytes = memoryStream.ToArray();
        return Assembly.Load(assemblyBytes);
    }
    
    private static MetadataReference CreateMetadataReference(Type type) =>
        MetadataReference.CreateFromFile(type.Assembly.Location);
}