using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.Contexts.Mapping
{
    public class StationLogMap : IEntityTypeConfiguration<StationLog>
    {
        public void Configure(EntityTypeBuilder<StationLog> builder)
        {
            builder.Property(x => x.Id).UseIdentityAlwaysColumn();

            builder.Property(x => x.CreateDate).HasDefaultValueSql("now() at time zone 'utc'");
        }
    }
}
