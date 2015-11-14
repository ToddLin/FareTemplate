using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.FareTemplates;

namespace Nop.Data.Mapping.FareTemplates
{
    internal class FareTemplateMap:EntityTypeConfiguration<FareTemplate>
    {
        public FareTemplateMap()
        {
            this.ToTable("FareTemplate");
            this.HasKey(x=>x.Id);
        }
    }
}
