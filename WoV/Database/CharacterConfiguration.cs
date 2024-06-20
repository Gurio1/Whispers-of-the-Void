using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WoV.Database;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.ToTable("Test");
        
        builder.Property(ch => ch.Id)
            .ValueGeneratedNever();

        builder.HasOne(c => c.CultivationStage)
            .WithMany()
            .HasForeignKey(c => c.CultivationStageId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasOne(c => c.CultivationSubStage)
            .WithMany()
            .HasForeignKey(c => c.CultivationSubStageId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}