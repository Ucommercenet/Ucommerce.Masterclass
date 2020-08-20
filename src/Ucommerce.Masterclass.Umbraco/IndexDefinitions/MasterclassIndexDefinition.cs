using Ucommerce.Search.Definitions;
using Ucommerce.Search.Extensions;

namespace Ucommerce.Masterclass.Umbraco.IndexDefinitions
{
    public class MasterclassIndexDefinition : DefaultProductsIndexDefinition
    {
        public MasterclassIndexDefinition() : base()
        {
            this.Field(p => p["Color"], typeof(string));
            this.Field(p => p["ShoeSize"], typeof(string));

            this.Facet("Color");
            this.Facet("ShoeSize");
        }
    }
}