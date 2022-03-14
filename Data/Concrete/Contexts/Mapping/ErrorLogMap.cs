using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Concrete.Contexts.Mapping
{
    public class ErrorLogMap : IEntityTypeConfiguration<ErrorLog>
    {
        public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.Property(x => x.Id).UseIdentityAlwaysColumn();
            builder.Property(x => x.CreateDate).HasDefaultValueSql("now() at time zone 'utc'");
        }
    }
}
