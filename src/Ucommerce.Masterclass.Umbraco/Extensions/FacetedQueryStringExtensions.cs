using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Ucommerce.Search.Facets;
using Umbraco.Core;

namespace Ucommerce.Masterclass.Umbraco.Extensions
{
    public static class FacetedQueryStringExtensions
    {
        public static IList<Facet> ToFacets(this NameValueCollection target)
        {
            var parameters = new Dictionary<string, string>();
            foreach (var queryString in HttpContext.Current.Request.QueryString.AllKeys)
            {
                parameters[queryString] = HttpContext.Current.Request.QueryString[queryString];
            }

            parameters.RemoveAll(kvp =>
                new[] { "umbDebugShowTrace", "product", "variant", "category", "categories", "catalog" }
                    .Contains(kvp.Key));

            var facetsForQuerying = parameters.Select(parameter => new Facet
            {
                Name = parameter.Key,
                FacetValues = parameter.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(
                    value => new FacetValue
                    {
                        Value = value
                    }).ToList()
            }).ToList();

            return facetsForQuerying;
        }
    }
}