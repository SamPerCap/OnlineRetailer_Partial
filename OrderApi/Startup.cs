using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Data;
using OrderApi.Infastructure;
using SharedModels;
using System;

namespace OrderApi
{
    public class Startup
    {
        Uri productServiceBaseUrl = new Uri("http://productapi/api/products");
        Uri customerServceBaseUrl = new Uri("http://customerapi/api/customers");
        //Kris'
        //"host=baboon.rmq.cloudamqp.com;virtualHost=xzmfsdno;username=xzmfsdno;password=bbqqKyO5uEP8XgIy921h3unMiAwZUleX";
        //Sam's
        string cloudAMQPConnectionString =
          "host=hawk.rmq.cloudamqp.com;virtualHost=embszgpu;username=embszgpu;password=Maer_MLg3Ib4341CdUnICTys6OBO53Lq";

        //To start up
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // In-memory database:
            services.AddDbContext<OrderApiContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));

            // Register repositories for dependency injection
            services.AddScoped<IRepository<SharedOrders>, OrderRepository>();

            // Register database initializer for dependency injection
            services.AddTransient<IDbInitializer, DbInitializer>();

            // Register product service gateway for dependency injection
            services.AddSingleton<IServiceGateway<SharedProducts>>(new
                ProductServiceGateway(productServiceBaseUrl));
            services.AddSingleton<IServiceGateway<SharedCustomers>>(new
                CustomerServiceGateway(customerServceBaseUrl));

            // Register MessagePublisher (a messaging gateway) for dependency injection
            services.AddSingleton<IMessagePublisher>(new
                MessagePublisher(cloudAMQPConnectionString));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Initialize the database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetService<OrderApiContext>();
                var dbInitializer = services.GetService<IDbInitializer>();
                dbInitializer.Initialize(dbContext);
            }

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
