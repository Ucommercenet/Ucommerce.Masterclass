using System.Collections.Generic;
using Ucommerce.Search;
using Ucommerce.Search.Definitions;
using Ucommerce.Search.Extensions;

namespace Ucommerce.Masterclass.Umbraco.IndexDefinitions
{
    public class MasterclassIndexDefinition : DefaultProductsIndexDefinition
    {
        public MasterclassIndexDefinition() : base()
        {
            this.Field(p => p["Color"], typeof(IEnumerable<string>));
            this.Field(p => p["ShoeSize"], typeof(string));

            this.Field(p => p.Name, IndexOptions.FullText);

            this.Facet("Color");
            
            this.Facet("Color");
            this.Facet("ShoeSize");
        }
    }
}