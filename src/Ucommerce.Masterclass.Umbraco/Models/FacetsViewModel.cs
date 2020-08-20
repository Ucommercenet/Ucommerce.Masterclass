using System.Collections.Generic;

namespace Ucommerce.Masterclass.Umbraco.Models
{
    public class FacetsViewModel
    {
        public FacetsViewModel()
        {
            FacetValues = new List<FacetValueViewModel>();
        }
        public string Key { get; set; }
        
        public IList<FacetValueViewModel> FacetValues { get; set; }
        public string DisplayName { get; set; }
    }


    public class FacetValueViewModel
    {
        public string Key { get; set; }
        
        public uint Count { get; set; }
    }
}