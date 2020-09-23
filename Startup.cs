using ar.AzureFunctions.Sales.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ar.AzureFunctions.Sales.Startup))]
namespace ar.AzureFunctions.Sales
{
    // Implement IWebJobStartup interface.
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddSingleton<OpenInvoiceBuyerClient>();

            builder.Services.AddTransient<ISalesService, SalesService>();
        }
    }
}
