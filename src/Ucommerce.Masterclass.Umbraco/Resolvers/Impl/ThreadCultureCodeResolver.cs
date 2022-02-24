using System.Threading;

namespace Ucommerce.Masterclass.Umbraco.Resolvers.Impl
{
    public class ThreadCultureCodeResolver : ICultureCodeResolver
    {
        public string GetCultureCode()
        {
            return Thread.CurrentThread.CurrentUICulture.ToString(); ;
        }
    }
}