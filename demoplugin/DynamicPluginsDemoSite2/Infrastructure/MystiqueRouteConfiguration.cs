using Microsoft.AspNetCore.Builder;

namespace DynamicPluginsDemoSite2.Infrastructure
{
    public static class MystiqueRouteConfiguration
    {
        public static IApplicationBuilder MystiqueRoute(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "Customer",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                routes.MapControllerRoute(
                    name: "Customer",
                    pattern: "Modules/{area}/{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }
    }
}
