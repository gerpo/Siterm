using Microsoft.Extensions.DependencyInjection;
using Siterm.Excel.Services;

namespace Siterm.Excel
{
    public static class ExcelServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<ReadPSentencesService>();
            services.AddTransient<ReadHSentencesService>();
        }
    }
}