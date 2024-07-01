using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimulationStorm.AppSaves.Persistence.EntityConfigurations;

public class AppSaveConfiguration(AppSavesOptions options) : IEntityTypeConfiguration<AppSave>
{
    public void Configure(EntityTypeBuilder<AppSave> builder)
    {
        builder.ToTable(nameof(AppSavesDatabaseContext.AppSaves));
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.ServiceSaves);
        builder
            .Property(x => x.DateAndTime)
            .IsRequired();
        builder
            .Property(x => x.Name)
            .HasMaxLength(options.AppSaveNameLengthRange.Maximum)
            .IsRequired();
    }
}