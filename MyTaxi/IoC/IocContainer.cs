using Microsoft.Extensions.DependencyInjection;

namespace MyTaxi
{
    public static class IoC
    {
        public static Models.MyTaxiDbContext MyTaxiDbContext => IoCContainer.Provider.GetService<Models.MyTaxiDbContext>();
    }

    public static class IoCContainer
    {
        public static ServiceProvider Provider {get;set;}
    }
}
