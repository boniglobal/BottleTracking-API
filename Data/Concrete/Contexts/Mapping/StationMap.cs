using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.Contexts.Mapping
{
    public class StationMap : IEntityTypeConfiguration<Station>
    {
        public void Configure(EntityTypeBuilder<Station> builder)
        {
            builder.Property(x => x.Id).UseIdentityAlwaysColumn();
            builder.Property(x => x.CreateDate).HasDefaultValueSql("now() at time zone 'utc'");

            builder.HasMany(x => x.StationLogs).WithOne(x => x.Station).HasForeignKey(x => x.StationId);
        }
    }
}
