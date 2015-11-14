using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.FareTemplates;

namespace Nop.Data.Mapping.FareTemplates
{
    internal class CarryModeMap : EntityTypeConfiguration<CarryMode>
    {
        public CarryModeMap()
        {
            this.ToTable("CarryMode");
            this.HasKey(x => x.Id);
            this.HasRequired(x => x.FareTemplate).WithMany(t=>t.CarryModes).HasForeignKey(x => x.FareId);
        }
    }
}
