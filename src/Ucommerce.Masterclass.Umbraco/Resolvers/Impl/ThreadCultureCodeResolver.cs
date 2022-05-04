using System.Threading;

namespace MC_Headless.Resolvers.Impl
{
    public class ThreadCultureCodeResolver : ICultureCodeResolver
    {
        public string GetCultureCode()
        {
            return Thread.CurrentThread.CurrentUICulture.ToString();
        }
    }
}