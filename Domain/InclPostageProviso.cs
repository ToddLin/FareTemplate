using System;

namespace Nop.Core.Domain.FareTemplates
{
    public class InclPostageProviso : BaseEntity
    {
        public virtual int FareId { get; set; }

        public virtual string Region { get; set; }

        public virtual int? PieceNo { get; set; }

        public virtual decimal? WeightNo { get; set; }

        public virtual decimal? BulkNo { get; set; }

        public virtual decimal? Amount { get; set; }

        public virtual int CarryWay { get; set; }

        public virtual bool IsAll { get; set; }

        public virtual FareTemplate FareTemplate { get; set; }

    }
}
