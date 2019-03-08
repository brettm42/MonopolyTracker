
namespace MonopolyTracker.Server
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using MonopolyTracker.Server.Services.BoardService;

    public class Startup
    {
        private readonly IWebHostEnvironment environment;

        public Startup (IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression();

            //var physicalProvider = 
            //    this.environment.ContentRootFileProvider;
            //var manifestEmbeddedProvider =
            //    new ManifestEmbeddedFileProvider(Assembly.GetEntryAssembly());
            //var compositeProvider =
                //new CompositeFileProvider(physicalProvider, manifestEmbeddedProvider);

            //services.AddSingleton<IFileProvider>(compositeProvider);
            services.AddSingleton<IBoardService, BoardService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
                });

            app.UseBlazor<Client.Startup>();
            app.UseBlazorDebugging();
        }
    }
}
