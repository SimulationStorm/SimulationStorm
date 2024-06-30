using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimulationStorm.AppStates.Persistence.EntityConfigurations;

public class AppStateEntityConfiguration(AppStatesOptions options) : IEntityTypeConfiguration<AppState>
{
    public void Configure(EntityTypeBuilder<AppState> builder)
    {
        builder.ToTable("AppStates");
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.ServiceStates);
        builder
            .Property(x => x.DateAndTime)
            .IsRequired();
        builder
            .Property(x => x.Name)
            .HasMaxLength(options.AppStateNameLengthRange.Maximum)
            .IsRequired();
    }
}