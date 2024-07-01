using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimulationStorm.AppSaves.Persistence.Serialization;

namespace SimulationStorm.AppSaves.Persistence.EntityConfigurations;

public class AppServiceSaveConfiguration : IEntityTypeConfiguration<ServiceSave>
{
    public void Configure(EntityTypeBuilder<ServiceSave> builder)
    {
        builder.ToTable(nameof(AppSavesDatabaseContext.ServiceSaves));
        builder.HasKey(x => x.Id);
        builder
            .HasOne(x => x.AppSave)
            .WithMany(x => x.ServiceSaves)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .Property(x => x.SaveObjectType)
            .HasConversion
            (
                type => type.AssemblyQualifiedName!,
                typeName => Type.GetType(typeName)!
            )
            .IsRequired();
        builder
            .Property(x => x.SaveObject)
            .HasConversion
            (
                saveObject => ObjectSerializer.SerializeObject(saveObject),
                serializedSaveObject => ObjectSerializer.DeserializeObject(serializedSaveObject)
            )
            .IsRequired();
    }
}