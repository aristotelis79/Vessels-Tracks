using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;
using VesselTrackApi.Controllers;
using VesselTrackApi.Data;
using VesselTrackApi.Data.Entities;
using VesselTrackApi.Repositories;
using VesselTrackApi.Services;
using VesselTrackApi.SwaggerHelpers;
using WebApiContrib.Core.Formatter.Csv;

namespace VesselTrackApi
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
            #region DI

            services.AddTransient<IDbContext, VesselsTrackDbContext>();
            services.AddTransient<IRepository<VesselPositionEntity,long>, EfRepository<VesselPositionEntity,long>>();
            services.AddTransient<IImportService, ImportService>();
            services.AddTransient<ITrackService, TrackService>();
            services.AddTransient<DateActionFilter>();

            #endregion

            #region Database

            services.AddDbContext<VesselsTrackDbContext>(o =>
            {
                o.UseLoggerFactory(new LoggerFactory(new[] {new DebugLoggerProvider()}))
                    .UseSqlServer(Configuration["ConnectionString"]);
            }, ServiceLifetime.Transient);

            #endregion

            #region Swagger Documentation

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "VESSEL TRACK API",
                });
                c.OperationFilter<SwaggerFileOperationFilter>();
                c.CustomSchemaIds(d => d.DefaultSchemaIdSelector());

                // XML Documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
            });


            #endregion

            #region ApiResponseConfigurations

            services.AddResponseCompression(o =>
            {
                o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/csv" });
            });

            services
                .AddRouting(options => options.LowercaseUrls = true)
                .AddMvcCore(o =>
                {
                    o.Filters.AddService<DateActionFilter>();
                    o.RespectBrowserAcceptHeader = true;
                    o.ReturnHttpNotAcceptable = true;
                    o.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
                    
                })
                .AddJsonFormatters()
                .AddXmlSerializerFormatters()
                .AddFormatterMappings()
                .AddCsvSerializerFormatters(new CsvFormatterOptions {CsvDelimiter = ","})
                .AddApiExplorer()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #endregion

            #region AspNetCoreRateLimit

            services.AddOptions()
                    .AddMemoryCache()
                    .Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"))
                    .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
                    .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()
                    .AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>()
                    .AddHttpContextAccessor();
            #endregion

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

            app.UseStaticFiles();

            app.UseIpRateLimiting();

            //app.UseHttpsRedirection();//TODO
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vessel Track API V1");
                c.RoutePrefix = string.Empty;
            });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<VesselsTrackDbContext>();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
