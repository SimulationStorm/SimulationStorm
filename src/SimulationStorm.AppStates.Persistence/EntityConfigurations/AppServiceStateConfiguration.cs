using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimulationStorm.AppStates.Persistence.Serialization;

namespace SimulationStorm.AppStates.Persistence.EntityConfigurations;

public class AppServiceStateEntityConfiguration : IEntityTypeConfiguration<AppServiceState>
{
    public void Configure(EntityTypeBuilder<AppServiceState> builder)
    {
        builder.ToTable("AppServiceStates");
        builder.HasKey(x => x.Id);
        builder
            .HasOne(x => x.AppState)
            .WithMany(x => x.ServiceStates)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .Property(x => x.StateType)
            .HasConversion
            (
                type => type.AssemblyQualifiedName!,
                typeName => Type.GetType(typeName)!
            )
            .IsRequired();
        builder
            .Property(x => x.State)
            .HasConversion
            (
                state => ObjectSerializer.SerializeObject(state),
                serializedState => ObjectSerializer.DeserializeObject(serializedState)
            )
            .IsRequired();
    }
}