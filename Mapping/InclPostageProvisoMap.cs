using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.FareTemplates;

namespace Nop.Data.Mapping.FareTemplates
{
    internal class InclPostageProvisoMap : EntityTypeConfiguration<InclPostageProviso>
    {
        public InclPostageProvisoMap()
        {
            this.ToTable("InclPostageProviso");
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.FareTemplate).WithMany(t=>t.InclPostageProvisos).HasForeignKey(x => x.FareId);
        }
    }
}
