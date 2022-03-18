using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.Contexts.Mapping
{
    public class BottleMap : IEntityTypeConfiguration<Bottle>
    {
        public void Configure(EntityTypeBuilder<Bottle> builder)
        {
            builder.Property(x => x.Id).UseIdentityAlwaysColumn();

            builder.Property(x => x.CreateDate).HasDefaultValueSql("now() at time zone 'utc'");

            builder.HasMany(x => x.StationLogs).WithOne(x => x.Bottle).HasForeignKey(x => x.BottleId);

            builder.HasQueryFilter(x => !x.Deleted);

        }
    }
}
