using Command.Contracts.Events;
using Common.Infrastructure.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ResilientMicroservices.Sample.Customers.Web.Handlers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ResilientMicroservices.Sample.Customers.Web
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
            services.Configure<KafkaEventConsumerConfiguration>(Configuration.GetSection("KafkaConsumer"));
            services.PostConfigure<KafkaEventConsumerConfiguration>(options =>
            {
                options.RegisterConsumer<CustomerRegisteredEvent, CustomerRegisteredEventHandler>();
            });
            services.AddSingleton<IHostedService, KafkaConsumer>();
            services.Configure<KafkaEventProducerConfiguration>(Configuration.GetSection("KafkaProducer"));
            services.PostConfigure<KafkaEventProducerConfiguration>(options =>
            {
                options.SerializerSettings =
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            });
            services.AddTransient<IKakfaProducer, KafkaProducer>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
