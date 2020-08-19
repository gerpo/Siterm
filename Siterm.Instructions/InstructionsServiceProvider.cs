using Microsoft.Extensions.DependencyInjection;
using Siterm.Instructions.Services;

namespace Siterm.Instructions
{
    public static class InstructionsServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<InstructionService>();
            services.AddTransient<InstructionPdfService>();
        }
    }
}