using System;
using System.Collections.Generic;

namespace Nop.Core.Domain.FareTemplates
{
    public class FareTemplate : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual string ShopAddr { get; set; }

        public virtual string DispatchTime { get; set; }

        public virtual bool? IsInclPostage { get; set; }
        //1:按件 2:按重量 3:按体积
        public virtual int ValuationModel { get; set; }

        public virtual bool? IsInclPostageByif { get; set; }

        private ICollection<CarryMode> _carrymodes;
        public virtual ICollection<CarryMode> CarryModes
        {
            get { return _carrymodes ?? (_carrymodes = new List<CarryMode>()); }
            protected set { _carrymodes = value; }
        }

        private ICollection<InclPostageProviso> _inclPostageProvisos;
        public virtual ICollection<InclPostageProviso> InclPostageProvisos
        {
            get { return _inclPostageProvisos ?? (_inclPostageProvisos = new List<InclPostageProviso>()); }
            protected set { _inclPostageProvisos = value; }
        }
    }
}
