using Nop.Core;
using Nop.Core.Domain.FareTemplates;

namespace Nop.Admin.Models.FareTemplates
{
    public class FareTemplateListModel
    {
        public FareTemplateListModel()
        {
            PageFilterModel = new PageModel();
        }

        public IPagedList<FareTemplateModel> FareTemplates { get; set; }

        public PageModel PageFilterModel { get; set; }
    }
}