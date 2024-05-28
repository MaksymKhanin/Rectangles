using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Api.Configurations
{
    public static class Dependencies
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["ConnectionStrings:MongoDB"]);
            var database = mongoClient.GetDatabase("RectanglesDB");

            return services.AddSingleton(database);
        }
    }
}
