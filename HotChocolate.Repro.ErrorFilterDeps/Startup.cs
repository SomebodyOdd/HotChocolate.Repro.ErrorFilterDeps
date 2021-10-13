using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotChocolate.Repro.ErrorFilterDeps
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<SomeCoolService>();

            services.AddGraphQLServer()
                    .AddQueryType<Query>()
                    .AddErrorFilter<ErrorFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapGraphQL();
            });
        }
    }

    public class SomeCoolService
    {
        public bool CheckSomething()
        {
            return true;
        }
    }

    public class ErrorFilter : IErrorFilter
    {
        private readonly SomeCoolService _service;
        public ErrorFilter(SomeCoolService service)
            => _service = service;

        public IError OnError(IError error)
        {
            return error.SetExtension("Check", _service.CheckSomething() ? "Passed" : "Failed");
        }
    }
}
