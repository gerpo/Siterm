using Microsoft.Extensions.DependencyInjection;
using Siterm.Signature.Services;
using Siterm.Signature.ViewModels;
using Siterm.Signature.Views;

namespace Siterm.Signature
{
    public static class SignatureServiceProvider
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<SignatureService>();
            services.AddScoped<SignatureViewModel>();
            services.AddTransient<SignatureView>();
        }
    }
}