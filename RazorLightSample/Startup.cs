using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RazorClassLibary;
using RazorLight;
using RazorLight.Extensions;

namespace RazorLightSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();

            var engine = new RazorLightEngineBuilder()
                  .UseEmbeddedResourcesProject(typeof(Dummy)) // exception without this (or another project type)
                  .UseMemoryCachingProvider()
                  .Build();

            services.AddRazorLight(() => engine);
            services.AddSingleton(engine.Options);
            services.AddSingleton(engine.Handler.Compiler);
            services.AddSingleton(engine.Handler.FactoryProvider);
            services.AddSingleton(engine.Handler.Cache);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
