using Amazon.AppConfig;
using Amazon.AppConfigData;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace aws.appconfig.core.Services;

/// <summary>
/// Extension method for resolving services
/// </summary>
public static class AppconfigServiceCollectionExtensions
{
    /// <summary>
    /// Add this method for injecting services in startup class
    /// </summary>
    /// <param name="services">Service object for injecting the services</param>
    /// <param name="configuration">Configuration object for retrieving the required information</param>
    /// <returns cref="IServiceCollection">IServiceCollection object which have required dependencies</returns>

    public static IServiceCollection AddConfigManager(this IServiceCollection services, IConfiguration configuration)
    {
        // Add AWS profile to service
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());

        // AWS DI for fetching the config details
        services.AddAWSService<IAmazonAppConfigData>();
        services.AddAWSService<IAmazonAppConfig>();
        services.AddAWSService<IAmazonDynamoDB>();

        // Return these services so that can be configured in startup class by simply calling the method name in service
        return services;
    }
}
