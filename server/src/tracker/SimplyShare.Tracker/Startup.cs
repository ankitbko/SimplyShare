using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using SimplyShare.Core;
using SimplyShare.Core.Models;
using SimplyShare.Tracker.Formatters;
using SimplyShare.Tracker.Models;

namespace SimplyShare.Tracker
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
            services
                .AddMvc(options =>
                {
                    options.OutputFormatters.Insert(0, new BencodeOutputFormatter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<SharingOption>(Configuration.GetSection("ShareConfig"));
            services.Configure<CosmosOption>(Configuration.GetSection("Cosmos"));
            ConfigureBsonClassMap();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var cosmosOption = new CosmosOption();
            Configuration.GetSection("Cosmos").Bind(cosmosOption);
            cosmosOption.MongoConnectionString = Configuration.GetConnectionString("MongoConnectionString");
            builder.RegisterModule(new AutofacModule(cosmosOption));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void ConfigureBsonClassMap()
        {
            BsonClassMap.RegisterClassMap<MetaInfo>(cm => {
                cm.AutoMap();
                cm.SetIsRootClass(true);
            });
            BsonClassMap.RegisterClassMap<SingleFileInfo>();
            BsonClassMap.RegisterClassMap<MultipleFileInfo>();
        }
    }
}
