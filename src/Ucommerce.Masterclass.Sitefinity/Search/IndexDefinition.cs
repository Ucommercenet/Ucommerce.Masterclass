using Ucommerce.Search.Definitions;
using Ucommerce.Search.Extensions;

namespace Ucommerce.Masterclass.Sitefinity.Search
{
    public class IndexDefinition : DefaultProductsIndexDefinition
    {
        public IndexDefinition() : base()
        {
            this.Facet("");
        }
    }
}