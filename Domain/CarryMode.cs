using System;

namespace Nop.Core.Domain.FareTemplates
{
    public class CarryMode : BaseEntity
    {
        public virtual int FareId { get; set; }

        public virtual string Region { get; set; }

        public virtual int? FirstPiece { get; set; }

        public virtual decimal? FirstWeight { get; set; }

        public virtual decimal? FirstBulk { get; set; }

        public virtual decimal FirstAmount { get; set; }

        public virtual int? SecondPiece { get; set; }

        public virtual decimal? SecondWeight { get; set; }

        public virtual decimal? SecondBulk { get; set; }

        public virtual decimal SecondAmount { get; set; }

        public virtual int CarryWay { get; set; }

        public virtual bool? IsDefault { get; set; }

        public virtual FareTemplate FareTemplate { get; set; }
    }
}
