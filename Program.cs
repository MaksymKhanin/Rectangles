using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SegmentRectangleIntersection.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICalculation, Calculation>();
builder.Services.AddScoped<IRectangleService, RectangleService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rectangles and Lines", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rectangle Api v1"));

    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();