using Api.Configurations;
using Api.Mapping;
using Api.Middleware;
using Business.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using SegmentRectangleIntersection.Services;

namespace SegmentRectangleIntersection
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
            services.AddScoped<ICalculation, Calculation>();
            services.AddScoped<IRectangleService, RectangleService>();

            services.AddHttpContextAccessor();
            services.AddScoped<IHttpContextService, HttpContextService>();
            services.AddSingleton<IMaskingService, MaskingService>();

            //services.AddScoped<IStorage, InMemoryStorage>();
            services.AddScoped<IStorage, MongoDbStorage>();

            services.AddCors();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rectangles and Lines", Version = "v1" });
            });

            services.AddControllers();
            services.AddAutoMapper(typeof(EntityToDomainMapping));
            services.AddMongoDb(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rectangle Api v1"));

                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));

            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
