using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductApi.Data;
using ProductApi.Infastructure;
using ProductApi.Models;
using SharedModels;

namespace ProductApi
{
    public class Startup
    {
        //Sam's
        string cloudAMQPConnectionString =
          "host=hawk.rmq.cloudamqp.com;virtualHost=embszgpu;username=embszgpu;password=Maer_MLg3Ib4341CdUnICTys6OBO53Lq";
        //Kris'
        //"host=baboon.rmq.cloudamqp.com;virtualHost=xzmfsdno;username=xzmfsdno;password=bbqqKyO5uEP8XgIy921h3unMiAwZUleX";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // In-memory database:
            services.AddDbContext<ProductApiContext>(opt => opt.UseInMemoryDatabase("ProductsDb"));

            // Register repositories for dependency injection
            services.AddScoped<IRepository<SharedProducts>, ProductRepository>();

            // Register database initializer for dependency injection
            services.AddTransient<IDbInitializer, DbInitializer>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            // Initialize the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                // Initialize the database
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<ProductApiContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer.Initialize(dbContext);
            }

            // Create a message listener in a separate thread.
            Task.Factory.StartNew(() =>
                new MessageListener(app.ApplicationServices, cloudAMQPConnectionString).Start());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
