using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.Contexts.Mapping
{
    public class PanelUserMap : IEntityTypeConfiguration<PanelUser>
    {
        public void Configure(EntityTypeBuilder<PanelUser> builder)
        {
            builder.Property(x => x.Id).UseIdentityAlwaysColumn();

            builder.Property(x => x.CreateDate).HasDefaultValueSql("now() at time zone 'utc'");
            builder.HasQueryFilter(x => !x.Deleted);
        }
    }
}
